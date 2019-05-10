using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.ViewModels;

namespace WpfApp1.Commands
{
	public abstract class BaseCommand : ICommand
	{
		protected MainViewModel vm;

		public BaseCommand(MainViewModel viewModel)
		{
			vm = viewModel;
		}

		abstract public event EventHandler CanExecuteChanged;
		abstract public bool CanExecute(object parameter);
		abstract public void Execute(object parameter);
	}
}
