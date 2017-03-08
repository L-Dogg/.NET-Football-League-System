using FootballManager.Presentation.Infrastructure;
using FootballManager.BusinessLayer;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FootballManager.Presentation.Views
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
			var mainWindow = Application.Current.MainWindow as PageSwitcher;
			mainWindow.homeButton.IsEnabled = true;
			mainWindow.stylesButton.IsEnabled = true;

		}
	}
}
