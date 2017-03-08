using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FootballManager.Referee.Presentation.Infrastructure
{
	public class MyDelegateCommand : ICommand
	{
		private Action _execute;
		public MyDelegateCommand(Action execute)
		{
			_execute = execute;
		}
		public bool CanExecute(object parameter)
		{
			return true;
		}
		public event EventHandler CanExecuteChanged;
		public void Execute(object parameter)
		{
			_execute.Invoke();
		}
	}
}
