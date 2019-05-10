using ClassesForNP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
    class ConnectToServerCommand : BaseCommand
	{
		public ConnectToServerCommand(MainViewModel viewModel) : base(viewModel) { }

		public override event EventHandler CanExecuteChanged;
		public override bool CanExecute(object parameter)
		{
			return true;
		}
		public override void Execute(object parameter)
		{
            bool successful = true;
			var values = (object[])parameter;
			string nick = (string)values[1];
			string password = ((PasswordBox)values[0]).Password;
			string email = (string)values[2];
            bool IsSecured = (bool)values[3];

            string message;
			if (IsSecured)
				message = $"auth#{nick}#{password}#{email}#";
			else
				message = $"auth#{nick}#{password}#{email}#{vm.GetLocalIpAddress()}";
            vm.IsCurrentSecured = IsSecured;
			vm.client = new TcpClient();
			vm.client.Connect(vm.endPointOfServer);
			NetworkStream networkStream = vm.client.GetStream();
			StreamWriter writer = new StreamWriter(networkStream);
			StreamReader reader = new StreamReader(networkStream);

			writer.WriteLine(message);
			writer.Flush();
			string response1 = reader.ReadLine();
			if(response1.Length > 0)
			{
                Encriptor enc = new Encriptor();
                Random rand = new Random();
				vm.curr = new WTruUser
                {
                    NickName = nick,
                    PubKey = Encriptor.ConvertToString(enc.pubKey),
                    PrivKey = Encriptor.ConvertToString(enc.privKey),
					Address = vm.GetLocalIpAddress(),
                    Online = true,
                    Password = password,
                    Port = rand.Next(9000, 65000),
                    Email = email
                };
                switch (response1)
				{
					case "createFileReg#":
						{
							if (vm.curr != null)
							{
                                byte[] pubKarr = vm.ObjectToByteArray(Encriptor.ConvertBack(vm.curr.PubKey));
                                writer.WriteLine(pubKarr.Length);
                                writer.Flush();
                                networkStream.Write(pubKarr, 0, pubKarr.Length);
                                byte[] ser = vm.ObjectToByteArray(vm.curr);
								File.WriteAllBytes(Environment.CurrentDirectory + @"\User", ser);
							}
							break;
						}
					case "sendMeWTruUserReg#":
						{
							byte[] arr1 = vm.ObjectToByteArray(vm.curr);
							writer.WriteLine(arr1.Length);
							writer.Flush();
							networkStream.Write(arr1, 0, arr1.Length);
							break;
						}
					case "RecieveTruUser#":
						{
                            int tuSize = Int32.Parse(reader.ReadLine());
                            byte[] tuArr = new byte[tuSize];
                            Thread.Sleep(3000);
                            networkStream.Read(tuArr, 0, tuSize);
                            vm.curr = vm.FromByteArray<WTruUser>(tuArr);
							break;
						}
                    case "ReadUpTruUser#":
                        {
                            byte[] arr;
                            if (!File.Exists(Environment.CurrentDirectory + @"\User"))
                                MessageBox.Show("You have got a secured account and you " +
                                    "can't sign in from place, where you didn't sign up");
                            else
                            {
                                arr = File.ReadAllBytes(Environment.CurrentDirectory + @"\User");
                                var buff = vm.FromByteArray<WTruUser>(arr);
                                if (buff.Password == password)
                                {
                                    vm.curr = buff;
                                    vm.curr.Address = vm.GetLocalIpAddress();
                                }
                                else
                                    successful = false;
                            }
                            break;
                        }
                    case "WrongPassword#":
                        {
                            MessageBox.Show("Wrong password");
                            successful = false;
                            break;
                        }
                    default:
                        {
                            MessageBox.Show("Connection failed");
                            successful = false;
                            break;
                        }
                }
			}
            if (successful)
            {
                vm.HelloS = "Hello, " + vm.curr.NickName;
                MessageBox.Show(reader.ReadLine());
                vm.client.Close();
                vm.RefreshContacts();
                vm.OnRefreshListBoxRequested();
                if (vm.listener == null)
                {
                    vm.endPointOfClient = new IPEndPoint(IPAddress.Parse(vm.curr.Address), vm.curr.Port);
                    vm.listener = new TcpListener(vm.endPointOfClient);
                    vm.listener.Start();
                    vm.listenerThread = new Thread(vm.ListenerTargetFunction);
                    vm.listenerThread.SetApartmentState(ApartmentState.STA);
                    vm.listenerThread.IsBackground = true;
                    vm.listenerThread.Start();
                }
            }
            else
                vm.HelloS = "Hello, Stranger";
        }
    }
}
