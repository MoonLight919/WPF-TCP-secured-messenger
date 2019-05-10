using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Commands;
using WpfApp1.ViewModels;

namespace Kursach.Commands
{
    class PlayRequestedCommand : BaseCommand
	{
		public override event EventHandler CanExecuteChanged;

		public PlayRequestedCommand(MainViewModel viewModel) : base(viewModel) { }

		public override bool CanExecute(object parameter)
		{
			return true;
		}

		public override void Execute(object parameter)
		{
			vm.OnPlayRequested();
		}
	}
}
