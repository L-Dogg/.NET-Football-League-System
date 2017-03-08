using FootballManager.Referee.Presentation.Infrastructure;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FootballManager.Referee.Presentation.Views
{
	/// <summary>
	/// Interaction logic for MainMenu.xaml
	/// </summary>
	public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
			UnlockButtons();
        }

		public void UnlockButtons()
		{
			//var mainWindow = Application.Current.MainWindow as MainWindow;
			//mainWindow.homeButton.IsEnabled = true;
			//mainWindow.stylesButton.IsEnabled = true;

		}
	}
}
