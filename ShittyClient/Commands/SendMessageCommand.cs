using DialogUC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
    class SendMessageCommand : BaseCommand
    {
        public SendMessageCommand(MainViewModel viewModel) : base(viewModel) { }

        public override event EventHandler CanExecuteChanged;
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override void Execute(object parameter)
        {
            string text = parameter as string;
            for (int i = 0; i < vm.ChatControls.Count; i++)
            {
                if (vm.ChatControls[i].enable == true)
                {
                    string message;
                    for (int j = 0; j < vm.Chats[i].Members.Count; j++)
                    {
                        message = $"messageArrived#{vm.Chats[i].Name}#{vm.curr.NickName}#{text}";
                        vm.client = new TcpClient();
                        vm.client.Connect(new IPEndPoint(
                            IPAddress.Parse(vm.Chats[i].Members[j].Address), vm.Chats[i].Members[j].Port));
                        NetworkStream networkStream = vm.client.GetStream();
                        var writer = new StreamWriter(networkStream);
                        var reader = new StreamReader(networkStream);
                        writer.WriteLine(message);
                        writer.Flush();
                    }
                    return;
                }
            }
        }
    }
}