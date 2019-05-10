using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
    class EmailPasswordEnteredCommand : BaseCommand
    {
        public EmailPasswordEnteredCommand(MainViewModel viewModels) : base(viewModels) { }

        public override event EventHandler CanExecuteChanged;

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var pb = parameter as PasswordBox;
            vm.emailPassword = pb.Password;
            vm.EmailPasswordBorderVisib = System.Windows.Visibility.Collapsed;
        }
    }
}
