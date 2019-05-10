using ClassesForNP;
using DialogUC;
using Kursach.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfApp1.Commands;

namespace WpfApp1.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public AllUsersViewModel allUsersViewModel;
        public WTruUser curr;
        public bool IsCurrentSecured;
        public string CurrentEmail;
        public IPEndPoint endPointOfServer;
        public IPEndPoint endPointOfClient;
        public TcpClient client;
        public TcpListener listener;
        public Thread listenerThread;
        public bool mediaPlayerIsPlaying;
        public string emailPassword;
        public DispatcherTimer timer;

        public ObservableCollection<WChat> Chats { get; set; }
        public List<UserControl1> ChatControls { get; set; }
        public ObservableCollection<WUser> Contacts { get; set; }
        public ObservableCollection<WTruUser> RichContacts { get; set; }
        public List<ObservableCollection<ListBoxItem>> SourcesOFChats { get; set; }

        public bool userIsDraggingSlider;
        private string helloS;
        public string HelloS
        {
            get { return helloS; }
            set
            {
                helloS = value;
                OnPropertyChanged();
            }
        }
        private bool forMessageBorderEnabled;
        public bool ForMessageBorderEnabled
        {
            get { return forMessageBorderEnabled; }
            set
            {
                forMessageBorderEnabled = value;
                OnPropertyChanged();
            }
        }
        private double playerSliderValue;
        public double PlayerSliderValue
        {
            get { return playerSliderValue; }
            set
            {
                playerSliderValue = value;
                OnPropertyChanged();
            }
        }
        private string playerTimingValue;
        public string PlayerTimingValue
        {
            get { return playerTimingValue; }
            set
            {
                playerTimingValue = value;
                OnPropertyChanged();
            }
        }
        private string currentFile;
        public string CurrentFile
        {
            get { return currentFile; }
            set
            {
                currentFile = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan playerPosition;
        public TimeSpan PlayerPosition
        {
            get { return playerPosition; }
            set
            {
                playerPosition = value;
                OnPropertyChanged();
            }
        }
        private Visibility buttonCloseMenuVisibility;
        public Visibility ButtonCloseMenuVisibility
        {
            get { return buttonCloseMenuVisibility; }
            set
            {
                buttonCloseMenuVisibility = value;
                OnPropertyChanged();
            }
        }
        private Visibility buttonOpenMenuVisibility;
        public Visibility ButtonOpenMenuVisibility
        {
            get { return buttonOpenMenuVisibility; }
            set
            {
                buttonOpenMenuVisibility = value;
                OnPropertyChanged();
            }
        }
        private Visibility emailPasswordBorderVisib;
        public Visibility EmailPasswordBorderVisib
        {
            get { return emailPasswordBorderVisib; }
            set
            {
                emailPasswordBorderVisib = value;
                OnPropertyChanged();
            }
        }

        private PlayRequestedCommand changeStateOfPlayer;
        public BaseCommand ChangeStateOfPlayer
        {
            get
            {
                return changeStateOfPlayer;
            }
        }
        private DragCompletedCommand dragCompleted;
        public BaseCommand DragCompleted
        {
            get
            {
                return dragCompleted;
            }
        }
        private OpenFileCommand openFile;
        public BaseCommand OpenFile
        {
            get
            {
                return openFile;
            }
        }
        private SliderValueChangedCommand sliderValueChanged;
        public BaseCommand SliderValueChanged
        {
            get
            {
                return sliderValueChanged;
            }
        }
        private MenuStateChangeCommand menuStateChange;
        public BaseCommand MenuStateChange
        {
            get
            {
                return menuStateChange;
            }
        }
        private PlayRequestedCommand playRequested_C;
        public BaseCommand PlayRequested_C
        {
            get
            {
                return playRequested_C;
            }
        }
        private ConnectToServerCommand connectToServer;
        public BaseCommand ConnectToServer
        {
            get
            {
                return connectToServer;
            }
        }
        private AddContactToUserCommand addContactToUser;
        public BaseCommand AddContactToUser
        {
            get
            {
                return addContactToUser;
            }
        }
        private StartChatWithCommand startChatWith;
        public BaseCommand StartChatWith
        {
            get
            {
                return startChatWith;
            }
        }
        private EmailPasswordEnteredCommand emailPasswordEntered;
        public BaseCommand EmailPasswordEntered
        {
            get
            {
                return emailPasswordEntered;
            }
        }
        private AcceptRequestForChatCommand acceptRequestForChat;
        public BaseCommand AcceptRequestForChat
        {
            get
            {
                return acceptRequestForChat;
            }
        }
        private ClickOnChatsAreaCommand clickOnChatsArea;
        public BaseCommand ClickOnChatsArea
        {
            get
            {
                return clickOnChatsArea;
            }
        }
        private SendMessageCommand sendMessage;
        public BaseCommand SendMessage
        {
            get
            {
                return sendMessage;
            }
        }
        private RemoveContactFromUserCommand removeContactFromUser;
        public BaseCommand RemoveContactFromUser
        {
            get
            {
                return removeContactFromUser;
            }
        }
        private AddToChatCommand addToChat;
        public BaseCommand AddToChat
        {
            get
            {
                return addToChat;
            }
        }
        public event EventHandler PlayRequested;
        public event EventHandler ChangePositionRequested;
        public event EventHandler FileOpenedRequested;
        public event EventHandler RefreshListBoxRequested;
        public event EventHandler CreateChatRequested;
        public event EventHandler SendMessageEvent;
        public event EventHandler TickEvent;

        public MainViewModel()
        {
            allUsersViewModel = new AllUsersViewModel(this);
            Contacts = new ObservableCollection<WUser>();
            RichContacts = new ObservableCollection<WTruUser>();
            ChatControls = new List<UserControl1>();
            Chats = new ObservableCollection<WChat>();
            endPointOfServer = new IPEndPoint(
                IPAddress.Parse(GetLocalIpAddress()), 9197); // !!!Must be adapted to specific situation!!!
            SourcesOFChats = new List<ObservableCollection<ListBoxItem>>();

            mediaPlayerIsPlaying = false;
            clickOnChatsArea = new ClickOnChatsAreaCommand(this);
            emailPasswordEntered = new EmailPasswordEnteredCommand(this);
            acceptRequestForChat = new AcceptRequestForChatCommand(this);
            startChatWith = new StartChatWithCommand(this);
            changeStateOfPlayer = new PlayRequestedCommand(this);
            dragCompleted = new DragCompletedCommand(this);
            openFile = new OpenFileCommand(this);
            sliderValueChanged = new SliderValueChangedCommand(this);
            menuStateChange = new MenuStateChangeCommand(this);
            playRequested_C = new PlayRequestedCommand(this);
            connectToServer = new ConnectToServerCommand(this);
            addContactToUser = new AddContactToUserCommand(this, allUsersViewModel);
            sendMessage = new SendMessageCommand(this);
            removeContactFromUser = new RemoveContactFromUserCommand(this);
            addToChat = new AddToChatCommand(this);
            ButtonCloseMenuVisibility = Visibility.Collapsed;
            ButtonOpenMenuVisibility = Visibility.Visible;
            EmailPasswordBorderVisib = Visibility.Collapsed;
            ForMessageBorderEnabled = false;
            HelloS = "Hello, Stranger";
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += OnTickMethod;
            timer.Start();
        }

        private void OnTickMethod(object sender, EventArgs e)
        {
            if (TickEvent != null)
                TickEvent(this, EventArgs.Empty);
            else
            {
                MessageBox.Show("If you suddenly get this message, please inform developer team!");
                throw new NotImplementedException("Not implemented event handler 'TickEvent'.");
            }
        }

        public void ListenerTargetFunction()
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
                    case "messageArrived":
                        {
                            string chatName = parts[1];
                            string sender = parts[2];
                            string text = parts[3];
                            for (int i = 0; i < Chats.Count; i++)
                            {
                                if (Chats[i].Name == chatName)
                                {
                                    if (sender == curr.NickName)
                                        Application.Current.Dispatcher.Invoke(new Action(() => 
                                        SourcesOFChats[i].Add(new ListBoxItem
                                        {
                                            Content = text,
                                            HorizontalContentAlignment = HorizontalAlignment.Right
                                        })));
                                    else
                                        Application.Current.Dispatcher.Invoke(new Action(() => SourcesOFChats[i].Add(
                                            new ListBoxItem
                                            {
                                                Content = text,
                                                HorizontalContentAlignment = HorizontalAlignment.Left
                                            })));
                                    Application.Current.Dispatcher.Invoke(new Action(() => 
                                    {
                                        ChatControls[i].UpdateLayout();
                                    }));
                                }
                            }
                            break;
                        }
                    case "createChat":
                        
                            int size = Int32.Parse(reader.ReadLine());
                            byte[] arr = new byte[size];
                            Thread.Sleep(2000);
                            networkStream.Read(arr, 0, size);
                            WChat chat = FromByteArray<WChat>(arr);
                            Chats.Add(chat);
                            OnCreateChatRequested();
                            break;
                        
                    //case "addUserToChat":
                    //    {
                    //        int size = Int32.Parse(reader.ReadLine());
                    //        byte[] arr = new byte[size];
                    //        Thread.Sleep(2000);
                    //        networkStream.Read(arr, 0, size);
                    //        WUser chat = FromByteArray<WUser>(arr);
                    //        break;
                    //    }
                }
                acceptor.Close();
            }
        }

        public void CreateChat(WUser me, WUser someOne, string address, int port)
        {
            WChat chat = new WChat();
            chat.Members = new List<WTruUser>
            {
                curr,
                new WTruUser
                {
                    NickName = someOne.NickName,
                    Address = address,
                    Port = port,
                    Id = someOne.Id,
                    PubKey = someOne.PubKey
                }
            };
            chat.Name = $"{me.NickName}_{someOne.NickName}";
            Chats.Add(chat);
            OnCreateChatRequested();

            string message = "createChat#";
            var client = new TcpClient();
            client.Connect(address, port);
            NetworkStream networkStream = client.GetStream();
            StreamWriter writer = new StreamWriter(networkStream);
            StreamReader reader = new StreamReader(networkStream);
            writer.WriteLine(message);
            writer.Flush();

            byte[] arr = ObjectToByteArray(chat);
            writer.WriteLine(arr.Length);
            writer.Flush();
            Thread.Sleep(2000);
            networkStream.Write(arr, 0, arr.Length);
            Thread.Sleep(2000);
        }

        public void RefreshContacts()
        {
            string message = $"getUserContacts#{curr.NickName}";
            client = new TcpClient();
            client.Connect(endPointOfServer);
            NetworkStream networkStream = client.GetStream();
            StreamReader reader = new StreamReader(networkStream);
            StreamWriter writer = new StreamWriter(networkStream);

            writer.WriteLine(message);
            writer.Flush();
            int size = Int32.Parse(reader.ReadLine());
            if (size != -1)
            {
                byte[] arr = new byte[size];
                networkStream.Read(arr, 0, size);
                List<WUser> c ;
                try
                {
                    c = FromByteArray<List<WUser>>(arr);
                    Contacts.Clear();
                    c.ForEach(new Action<WUser>(x => Contacts.Add(x)));
                }
                catch
                {
                    RefreshContacts();
                }
            }
        }

        public void AddContactToUserMethod(WUser who)
        {
            string message = $"addContactToUser#{who.NickName}#{curr.NickName}#{IsCurrentSecured}";
            client = new TcpClient();
            client.Connect(endPointOfServer);
            NetworkStream networkStream = client.GetStream();
            StreamReader reader = new StreamReader(networkStream);
            StreamWriter writer = new StreamWriter(networkStream);

            writer.WriteLine(message);
            writer.Flush();
            RefreshContacts();
        }

        public void AddContactToUserMethod(string who)
        {
            string message = $"addContactToUser#{who}#{curr.NickName}";
            client = new TcpClient();
            client.Connect(endPointOfServer);
            NetworkStream networkStream = client.GetStream();
            StreamReader reader = new StreamReader(networkStream);
            StreamWriter writer = new StreamWriter(networkStream);

            writer.WriteLine(message);
            writer.Flush();
            RefreshContacts();
        }

        public void GetAllContacts<WUser>(ObservableCollection<WUser> collection)
        {
            //if(curr == null)
            string message = "getAllContacts#";
            client = new TcpClient();
            client.Connect(endPointOfServer);
            NetworkStream networkStream = client.GetStream();
            StreamReader reader = new StreamReader(networkStream);
            StreamWriter writer = new StreamWriter(networkStream);

            writer.WriteLine(message);
            writer.Flush();
            int size = Int32.Parse(reader.ReadLine());
            byte[] arr = new byte[size];
            networkStream.Read(arr, 0, size);
            List<WUser> contacts = FromByteArray<List<WUser>>(arr);
            collection.Clear();
            contacts.ForEach(new Action<WUser>((WUser x) => collection.Add(x)));
        }

        public byte[] ObjectToByteArray(object obj)
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
                ms.Seek(0, SeekOrigin.Begin);
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
        public string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var item in host.AddressList)
                if (item.AddressFamily == AddressFamily.InterNetwork)
                    return item.ToString();

            string res = host.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
            return "No connected networkes with IPv4";

        }

        public void OnPlayRequested()
        {
            if (PlayRequested != null)
                PlayRequested(this, EventArgs.Empty);
            else
            {
                MessageBox.Show("If you suddenly get this message, please inform developer team!");
                throw new NotImplementedException("Not implemented event handler 'PlayRequested'.");
            }
        }
        public void OnChangePositionRequested()
        {
            if (ChangePositionRequested != null)
                ChangePositionRequested(this, EventArgs.Empty);
            else
            {
                MessageBox.Show("If you suddenly get this message, please inform developer team!");
                throw new NotImplementedException("Not implemented event handler 'ChangePositionRequested'.");
            }
        }
        public void OnFileOpenedRequested()
        {
            if (FileOpenedRequested != null)
                FileOpenedRequested(this, EventArgs.Empty);
            else
            {
                MessageBox.Show("If you suddenly get this message, please inform developer team!");
                throw new NotImplementedException("Not implemented event handler 'FileOpenedRequested'.");
            }
        }
        public void OnRefreshListBoxRequested()
        {
            if (RefreshListBoxRequested != null)
                RefreshListBoxRequested(this, EventArgs.Empty);
            else
            {
                MessageBox.Show("If you suddenly get this message, please inform developer team!");
                throw new NotImplementedException("Not implemented event handler 'RefreshListBoxRequested'.");
            }
        }
        public void OnCreateChatRequested()
        {
            if (CreateChatRequested != null)
                CreateChatRequested(this, EventArgs.Empty);
            else
            {
                MessageBox.Show("If you suddenly get this message, please inform developer team!");
                throw new NotImplementedException("Not implemented event handler 'CreateChatRequested'.");
            }
        }
        public void OnSendMessageEvent()
        {
            if (SendMessageEvent != null)
                SendMessageEvent(this, EventArgs.Empty);
            else
            {
                MessageBox.Show("If you suddenly get this message, please inform developer team!");
                throw new NotImplementedException("Not implemented event handler 'SendMessageEvent'.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
