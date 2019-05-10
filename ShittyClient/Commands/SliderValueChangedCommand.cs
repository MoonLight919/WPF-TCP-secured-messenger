using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Commands;
using WpfApp1.ViewModels;

namespace Kursach.Commands
{
    class SliderValueChangedCommand : BaseCommand
	{
		public SliderValueChangedCommand(MainViewModel viewModel) : base(viewModel) { }

		public override event EventHandler CanExecuteChanged;
		public override bool CanExecute(object parameter)
		{
			return true;
		}
		public override void Execute(object parameter)
		{
			vm.PlayerTimingValue = TimeSpan.FromSeconds(vm.PlayerSliderValue).ToString(@"hh\:mm\:ss");
		}
	}
}
