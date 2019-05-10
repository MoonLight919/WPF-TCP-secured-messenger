using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
    class AddUserToContactsCommand : BaseCommand
    {
        AllUsersViewModel auvm;
        int siOfAllContactsLb;

        public AddUserToContactsCommand(MainViewModel viewModels, AllUsersViewModel allUsersViewModel) : base(viewModels)
        {
            auvm = allUsersViewModel;
            siOfAllContactsLb = -1;
        }

        public override event EventHandler CanExecuteChanged;

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            siOfAllContactsLb = (int)parameter;
            vm.AddContactToUserMethod(auvm.Contacts[siOfAllContactsLb]);
        }
    }
}