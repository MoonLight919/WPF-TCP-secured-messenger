using ClassesForNP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
    class AcceptRequestForChatCommand : BaseCommand
    {
        public AcceptRequestForChatCommand(MainViewModel viewModels) : base(viewModels) { }

        public override event EventHandler CanExecuteChanged;

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            string EncCode = parameter as string;
            string code = Encriptor.Decript(EncCode, Encriptor.ConvertBack( vm.curr.PrivKey));
            string[] parts = code.Split('#');
            WUser someOne = vm.Contacts.Where(x => x.NickName == parts[0]).First();
            WUser me = new WUser { NickName = vm.curr.NickName, Email = vm.CurrentEmail };
            vm.CreateChat(me, someOne, parts[1], Int32.Parse(parts[2]));
        }
    }
}