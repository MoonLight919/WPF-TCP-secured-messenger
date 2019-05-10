using DialogUC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
    class ClickOnChatsAreaCommand : BaseCommand
    {
        public ClickOnChatsAreaCommand(MainViewModel viewModels) : base(viewModels) { }

        public override event EventHandler CanExecuteChanged;

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            for (int i = 0; i < vm.ChatControls.Count; i++)
            {
                if (vm.ChatControls[i].marker == 1)
                {
                    vm.ChatControls[i].marker = 0;
                    vm.ChatControls[i].chatLb_MouseDoubleClick(null, null);
                }
                else if (vm.ChatControls[i].marker == 2)
                    vm.ChatControls[i].marker = 1;
            }

            bool x = false;
            for (int i = 0; i < vm.ChatControls.Count; i++)
            {
                if (vm.ChatControls[i].marker == 1)
                    x = true;
            }
            if (x)
                vm.ForMessageBorderEnabled = true;
            else
                vm.ForMessageBorderEnabled = false;
        }
    }
}