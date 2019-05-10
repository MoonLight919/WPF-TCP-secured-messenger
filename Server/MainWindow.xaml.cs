using ClassesForNP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NPExamF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		IPEndPoint endPoint;
		TcpListener listener;
		Thread listenerThread;
		Model1 context;

		public MainWindow()
		{
			InitializeComponent();

			context = new Model1();
			ipTb.Text = GetLocalIpAddress();

		}

		private void ListenerTargetFunction()
		{
			while (true)
			{
				TcpClient acceptor = listener.AcceptTcpClient();
				NetworkStream networkStream = acceptor.GetStream();
				StreamReader reader = new StreamReader(networkStream);
				StreamWriter writer = new StreamWriter(networkStream);
				string message = reader.ReadLine();
				string[] parts = message.Split('#');
				switch (parts[0])
				{
					case "auth":
						{
							string nickName = parts[1];
							string password = parts[2];
							string eMail = parts[3];
							string ipAddress = parts[4];
							string response1 = "", response2;
							bool isSecret;
							DBUser buff = context.DBUsers.Where(x => x.NickName == nickName).FirstOrDefault();
							if (buff == null)
							{
								if (eMail.Length == 0)
								{
									writer.WriteLine(response1);
									writer.Flush();
									response2 = "Please, input your Email address for registration.";
                                    writer.WriteLine(response2);
                                    writer.Flush();
                                    return;
								}
								else
								{
                                    string pubK;
                                    if (ipAddress.Length == 0)
									{
										response1 = "createFileReg#";
										writer.WriteLine(response1);
										writer.Flush();
										isSecret = true;
                                        int pubKSize = Int32.Parse(reader.ReadLine());
                                        byte[] pubKArr = new byte[pubKSize];
                                        networkStream.Read(pubKArr, 0, pubKSize);
                                        RSAParameters parameters = FromByteArray<RSAParameters>(pubKArr);
                                        pubK = Encriptor.ConvertToString(parameters);
                                    }
                                    else
									{
										response1 = "sendMeWTruUserReg#";
										writer.WriteLine(response1);
										writer.Flush();
										int userSize = Int32.Parse(reader.ReadLine());
										byte[] userArr = new byte[userSize];
										networkStream.Read(userArr, 0, userSize);
										WTruUser u = FromByteArray<WTruUser>(userArr);
										context.TruUsers.Add(new DBTruUser { Address = u.Address, Id = u.Id, NickName = u.NickName,
											Online = u.Online, Password = u.Password, Port = u.Port, PrivKey = u.PrivKey, PubKey = u.PubKey, Email = eMail });
										isSecret = false;
                                        pubK = u.PubKey;
									}
									context.DBUsers.Add(new DBUser() { NickName = nickName, Email = eMail, IsSecret = isSecret, PubKey = pubK});
									context.SaveChanges();
									logLb.Dispatcher.Invoke(new Action(() => { logLb.Items.Add($"User {parts[1]} has been registered"); logLb.Items.Refresh(); }));
									response2 = "Hello for the first time, " + nickName;
								}
							}
							else
							{
                                if (!buff.IsSecret)
                                {
                                    DBTruUser u = context.TruUsers.Where(x => x.NickName == nickName).FirstOrDefault();
                                    if (password != u.Password)
                                    {
                                        response1 = "WrongPassword#";
                                        writer.WriteLine(response1);
                                        writer.Flush();
                                    }
                                    else
                                    {
                                        response1 = "RecieveTruUser#";
                                        u.Address = parts[4];
                                        u.Online = true;
                                        context.SaveChanges();
                                        writer.WriteLine(response1);
                                        writer.Flush();
                                        var tuObj = new WTruUser
                                        {
                                            Address = u.Address,
                                            Id = u.Id,
                                            NickName = u.NickName,
                                            Online = u.Online,
                                            Password = u.Password,
                                            Port = u.Port,
                                            PrivKey = u.PrivKey,
                                            PubKey = u.PubKey,
                                            Email = u.Email
                                        };
                                        byte[] tu = ObjectToByteArray(tuObj);
                                        writer.WriteLine(tu.Length);
                                        writer.Flush();
                                        Thread.Sleep(3000);
                                        networkStream.Flush();
                                        networkStream.Write(tu, 0, tu.Length);
                                        networkStream.Flush();
                                    }
                                }
                                else
                                {
                                    response1 = "ReadUpTruUser#";
                                    writer.WriteLine(response1);
                                    writer.Flush();
                                }
								logLb.Dispatcher.Invoke(new Action(() => { logLb.Items.Add($"User {nickName} signed in"); logLb.Items.Refresh(); }));
								response2 = "Hi " + nickName + ", hope you're doing well";
                            };
                            writer.WriteLine(response2);
							writer.Flush();
							break;
						}
					case "getAllContacts":
						{
							string isSecret = parts[1];
                            List<WUser> forSend = new List<WUser>();
                            List<DBUser> dBUsers = context.DBUsers.ToList();
                            dBUsers.ForEach(new Action<DBUser>((DBUser x) => forSend.Add(new WUser { Id = x.Id, Email = x.Email,
                                IsSecret = x.IsSecret, NickName = x.NickName, PubKey = x.PubKey })));
							byte[] nicksBytes1 = ObjectToByteArray(forSend);
							writer.WriteLine(nicksBytes1.Length);
							writer.Flush();
							networkStream.Write(nicksBytes1, 0, nicksBytes1.Length);
							break;
						}
					case "addContactToUser":
						{
							string whoS = parts[1];
							string toS = parts[2];
							string isSecret = parts[3];
							if (isSecret == "false")
							{
								DBTruUser wT = context.TruUsers.Where(x => x.NickName == whoS).First();
								DBTruUser tT = context.TruUsers.Where(x => x.NickName == toS).First();
								if (tT.Contacts == null)
									tT.Contacts = new List<DBTruUser>() { wT };
								else
									tT.Contacts.Add(wT);
							}
							DBUser w = context.DBUsers.Where(x => x.NickName == whoS).First();
							DBUser t = context.DBUsers.Where(x => x.NickName == toS).First();
							if (t.Contacts == null)
								t.Contacts = new List<DBUser>() { w };
							else
								t.Contacts.Add(w);
                            context.SaveChanges();
							break;
						}
					case "getUserContacts":
						{
							string curr = parts[1];
							bool isSecret = true;
							if (context.TruUsers.Where(x => x.NickName == curr) != null)
								isSecret = false;
							if (isSecret)
							{
								List<DBUser> contacts = context.DBUsers.Where(x => x.NickName == curr).First().Contacts;
								if (contacts.Count == 0)
								{
									writer.WriteLine("-1");
									writer.Flush();
								}
								byte[] nicksBytes1 = ObjectToByteArray(contacts);
								writer.WriteLine(nicksBytes1.Length);
								writer.Flush();
								networkStream.Write(nicksBytes1, 0, nicksBytes1.Length);
							}
							else
							{
								List<DBUser> contacts = context.DBUsers.Where(x => x.NickName == curr).First().Contacts;
                                if (contacts == null)
                                {
                                    writer.WriteLine("-1");
                                    writer.Flush();
                                }
                                else
                                {
                                    List<WUser> liteUsers = new List<WUser>();
                                    contacts.ForEach(new Action<DBUser>(u =>
                                    {
                                        liteUsers.Add(new WUser { Id = u.Id, Email = u.Email, IsSecret = u.IsSecret, NickName = u.NickName, PubKey = u.PubKey });
                                    }));
                                    byte[] nicksBytes = ObjectToByteArray(liteUsers);
                                    writer.WriteLine(nicksBytes.Length);
                                    writer.Flush();
                                    networkStream.Write(nicksBytes, 0, nicksBytes.Length);
                                }
							}
							break;
						}
                    case "getTruUser":
                        {
                            string nick = parts[1];
                            DBTruUser u = context.TruUsers.Where(x => x.NickName == nick).First();
                            byte[] tuArr = ObjectToByteArray(new WTruUser
                            {
                                Address = u.Address,
                                Id = u.Id,
                                NickName = u.NickName,
                                Online = u.Online,
                                Password = u.Password,
                                Port = u.Port,
                                PrivKey = u.PrivKey,
                                PubKey = u.PubKey
                            });
                            writer.WriteLine(tuArr.Length);
                            writer.Flush();
                            networkStream.Write(tuArr, 0, tuArr.Length);
                            break;
                        }
					case "getSpecificUsers":
						{
							int size = Int32.Parse(parts[1]);
							byte[] arr = new byte[size];
							networkStream.Read(arr, 0, size);
							List<string> nicks = FromByteArray<List<string>>(arr);
							break;
						}
				}
				logLb.Dispatcher.Invoke(new Action(() => { logLb.Items.Add(DateTime.Now.ToString() + " > " + message); logLb.Items.Refresh(); }));
				acceptor.Close();
			}
		}

		private string GetLocalIpAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var item in host.AddressList)
			{
				if (item.AddressFamily == AddressFamily.InterNetwork)
					return item.ToString();
			}
			return "No connected networkes with IPv4";
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			listener?.Stop();
		}

		byte[] ObjectToByteArray(object obj)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream())
			{
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
		}

		public T FromByteArray<T>(byte[] data)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream(data))
			{
				object obj = bf.Deserialize(ms);
				return (T)obj;
			}
		}

		private void saveConfigB_Click(object sender, RoutedEventArgs e)
		{
			endPoint = new IPEndPoint(IPAddress.Parse(ipTb.Text), Int32.Parse(portTb.Text));
		}

		private void stopServerB_Click(object sender, RoutedEventArgs e)
		{
			//stop thread
			listenerThread.Suspend();
			listener.Stop();
			//informing
			logLb.Dispatcher.Invoke(new Action(() => { logLb.Items.Add($"<{DateTime.Now.ToString()}> Server has been stopped"); logLb.Items.Refresh(); }));
			stateL.Dispatcher.Invoke(new Action(() => { stateL.Content = "Server is down"; stateL.Foreground = Brushes.Red; }));
		}

		private void startServerB_Click(object sender, RoutedEventArgs e)
		{
			listener = new TcpListener(endPoint);
			listener.Start();
			logLb.Dispatcher.Invoke(new Action(() => { logLb.Items.Add(listener.LocalEndpoint); logLb.Items.Refresh(); }));


			//recreate thread
			listenerThread = new Thread(new ThreadStart(ListenerTargetFunction));
			listenerThread.IsBackground = true;
			listenerThread.Start();

			//informing
			logLb.Dispatcher.Invoke(new Action(() => { logLb.Items.Add($"<{DateTime.Now.ToString()}> Server has been started"); logLb.Items.Refresh(); }));
			stateL.Dispatcher.Invoke(new Action(() => { stateL.Content = "Server is running"; stateL.Foreground = Brushes.Green; }));

		}
	}
}
