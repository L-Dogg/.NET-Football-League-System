using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FootballManager.Referee.Presentation.Infrastructure
{
	// TODO: usunac jak nie bedzie tych przyciskow.
	public static class GlobalCommands
	{
		public static CompositeCommand ShowStylesCommand { get; set; } = new CompositeCommand();
		public static CompositeCommand HomeButtonCommand { get; set; } = new CompositeCommand();
		public static CompositeCommand GoBackCommand { get; set; } = new CompositeCommand();

		public static bool EnableClick = true;

		public static bool BlockWindowButtons()
		{
			EnableClick = false;
			return true;
		}

		public static bool UnlockWindowButtons()
		{
			EnableClick = true;
			return false;
		}
	}
}
