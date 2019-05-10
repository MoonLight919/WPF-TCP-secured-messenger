using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
    class MenuStateChangeCommand : BaseCommand
	{
		public MenuStateChangeCommand(MainViewModel viewModel) : base(viewModel) { }

		public override event EventHandler CanExecuteChanged;
		public override bool CanExecute(object parameter)
		{
			return true;
		}
		public override void Execute(object parameter)
		{
			if(vm.ButtonCloseMenuVisibility == System.Windows.Visibility.Collapsed)
			{
				vm.ButtonCloseMenuVisibility = System.Windows.Visibility.Visible;
				vm.ButtonOpenMenuVisibility = System.Windows.Visibility.Collapsed;
			}
			else
			{
				vm.ButtonCloseMenuVisibility = System.Windows.Visibility.Collapsed;
				vm.ButtonOpenMenuVisibility = System.Windows.Visibility.Visible;
			}
		}
	}
}
