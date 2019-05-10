using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
    public class AddContactToUserCommand : BaseCommand
    {
        int SiOfAllContactsLb;
        AllUsersViewModel auvm;

        public AddContactToUserCommand(MainViewModel viewModels, AllUsersViewModel allUsersViewModel) : base(viewModels)
        {
            auvm = allUsersViewModel;
            SiOfAllContactsLb = -1;
        }

        public override event EventHandler CanExecuteChanged;
        public override bool CanExecute(object parameter)
        {
            int SiOfAllContactsLb = (int)parameter;
            if (SiOfAllContactsLb == -1)
                return true;
            else
                return false;
        }

        public override void Execute(object parameter)
        {
            auvm.ShowWindow();
        }
    }
}
