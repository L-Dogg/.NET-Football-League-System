using FootballManager.Referee.Presentation.RefereeService;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FootballManager.Referee.Presentation.Infrastructure;
using FootballManager.Referee.Presentation.Views;
using Microsoft.Maps.MapControl.WPF;

namespace FootballManager.Referee.Presentation
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void DirectionList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.directionList.SelectedItem == null)
				return;

			var emf = (this.Content as TransitioningContentControl).Content as EditMatchForm;
			var dir = this.directionList.SelectedItem as Direction;

			MapLayer.SetPosition(emf.ContentPopup, dir.Location);
			MapLayer.SetPositionOffset(emf.ContentPopup, new Point(20, -15));

			var location = dir;

			emf.ContentPopupText.Text = location.Description;
			emf.ContentPopup.Visibility = Visibility.Visible;

			emf.directionsMap.SetView(location.Location, emf.directionsMap.ZoomLevel);
		}
	}
}
