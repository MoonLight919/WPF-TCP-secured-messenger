using ClassesForNP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
    class StartChatWithCommand : BaseCommand
    {
        public StartChatWithCommand(MainViewModel viewModels) : base(viewModels){}
        public override event EventHandler CanExecuteChanged;
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            int siOfAllContactsLb = (int)parameter;
            WUser me = new WUser
            {
                NickName = vm.curr.NickName,
                Email = vm.curr.Email,
                Id = vm.curr.Id,
                IsSecret = vm.IsCurrentSecured,
                PubKey = vm.curr.PubKey
            };
            WUser someOne = vm.Contacts[siOfAllContactsLb];
            if(someOne.IsSecret)
            {
                if (vm.emailPassword==null)
                {
                    MessageBox.Show("Please, enter password for email account!");
                    vm.EmailPasswordBorderVisib = Visibility.Visible;
                    return;
                }
                MailAddress from = new MailAddress(me.Email);
                MailAddress to = new MailAddress(someOne.Email);
                MailMessage m = new MailMessage(from, to);

                m.Subject = "Request to start chat with " + me.NickName;
                Encriptor enc = new Encriptor();
                m.Body = Encriptor.Encript(
                    $"{vm.curr.NickName}#{vm.curr.Address}#{vm.curr.Port}",
                    Encriptor.ConvertBack(someOne.PubKey));
                int port = 587;
                string server = "smtp.gmail.com";
                string login = me.Email;
                string password = vm.emailPassword;

                SmtpClient c = new SmtpClient(server, port);
                c.Credentials = new NetworkCredential(login, password);
                c.EnableSsl = true;
                try
                {
                    c.Send(m);
                }
                catch(Exception exc)
                {
                    MessageBox.Show(exc.Message);
                    return;
                }
                MessageBox.Show("Message has been successfully sent");
                return;
            }
            string message = $"getTruUser#{someOne.NickName}";
            vm.client = new TcpClient();
            vm.client.Connect(vm.endPointOfServer);
            NetworkStream networkStream = vm.client.GetStream();
            StreamReader reader = new StreamReader(networkStream);
            StreamWriter writer = new StreamWriter(networkStream);
            writer.WriteLine(message);
            writer.Flush();

            int tuSize = Int32.Parse(reader.ReadLine());
            byte[] tuArr = new byte[tuSize];
            networkStream.Read(tuArr, 0, tuSize);
            WTruUser wTru = vm.FromByteArray<WTruUser>(tuArr);
            vm.CreateChat(me, someOne, wTru.Address, wTru.Port);
        }
    }
}
