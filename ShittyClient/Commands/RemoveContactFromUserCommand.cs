using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
    class RemoveContactFromUserCommand : BaseCommand
    {
        public RemoveContactFromUserCommand(MainViewModel viewModels) : base(viewModels) { }

        public override event EventHandler CanExecuteChanged;

        public override bool CanExecute(object parameter)
        {
            int SiOfAllContactsLb = (int)parameter;
            if (SiOfAllContactsLb == -1)
                return false;
            else
                return true;
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
