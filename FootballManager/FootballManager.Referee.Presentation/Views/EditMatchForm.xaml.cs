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
using FootballManager.Referee.Presentation.ViewModels;
using MahApps.Metro.Controls;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Design;

namespace FootballManager.Referee.Presentation.Views
{
	/// <summary>
	/// Interaction logic for EditMatchForm.xaml
	/// </summary>
	public partial class EditMatchForm : UserControl
	{
		public EditMatchForm()
		{
			InitializeComponent();
			(this.DataContext as EditMatchFormViewModel).DirectionsMap = this.directionsMap;

			var window = Application.Current.MainWindow as MainWindow;
			window.directionList.DataContext = this.DataContext;

			
			window.directionFlyout.CloseCommand = new ActionCommand(() =>
			{
				var margin = this.okButton2.Margin;
				this.okButton2.Margin = new Thickness(margin.Left, margin.Top, margin.Right - window.directionFlyout.Width, margin.Bottom);
				window.directionFlyout.IsOpen = false;
			});
		}
		
		
		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			var window = Application.Current.MainWindow as MainWindow;
			var open = !window.directionFlyout.IsOpen;
			window.directionFlyout.IsOpen = open;
			var margin = this.okButton2.Margin;
			if (open)
			{
				this.okButton2.Margin = new Thickness(margin.Left, margin.Top, margin.Right + window.directionFlyout.Width, margin.Bottom);
				this.ContentPopup.Visibility = Visibility.Hidden;
			}
			
		}

		private void Route_MouseEnter(object sender, MouseEventArgs e)
		{
			FrameworkElement pin = sender as FrameworkElement;
			MapLayer.SetPosition(ContentPopup, MapLayer.GetPosition(pin));
			MapLayer.SetPositionOffset(ContentPopup, new Point(20, -15));

			var location = (Direction)pin.Tag;

			ContentPopupText.Text = location.Description;
			ContentPopup.Visibility = Visibility.Visible;
		}

		private void Route_MouseLeave(object sender, MouseEventArgs e)
		{
			ContentPopup.Visibility = Visibility.Collapsed;
		}

		private void MetroTabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var window = Application.Current.MainWindow as MainWindow;
			if (window.directionFlyout.CloseCommand == null || !window.directionFlyout.IsOpen) return;

			window.directionFlyout.CloseCommand.Execute(null);
		}

		private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			var pin = sender as FrameworkElement;
			MapLayer.SetPosition(ContentPopup, MapLayer.GetPosition(pin));
			MapLayer.SetPositionOffset(ContentPopup, new Point(20, -15));

			var location = (Direction)pin.Tag;

			ContentPopupText.Text = location.Description;
			ContentPopup.Visibility = Visibility.Visible;

			var window = Application.Current.MainWindow as MainWindow;
			if (!window.directionFlyout.IsOpen) return;
			
			window.directionList.SelectedIndex = window.directionList.Items.IndexOf(location);
		}
	}
}
