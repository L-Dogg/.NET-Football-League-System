using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro;
using MahApps.Metro.Controls;
using FootballManager.BusinessLayer;
using System.Collections.ObjectModel;

namespace FootballManager.Presentation.Views
{
	/// <summary>
	/// Interaction logic for AccentStyleWindow.xaml
	/// </summary>
	public partial class AccentStyleWindow : UserControl
	{
		public static readonly DependencyProperty ColorsProperty
			= DependencyProperty.Register("Colors",
										  typeof(List<KeyValuePair<string, Color>>),
										  typeof(AccentStyleWindow),
										  new PropertyMetadata(default(List<KeyValuePair<string, Color>>)));

		public List<KeyValuePair<string, Color>> Colors
		{
			get { return (List<KeyValuePair<string, Color>>)GetValue(ColorsProperty); }
			set { SetValue(ColorsProperty, value); }
		}

		public ObservableCollection<KeyValuePair<string, string>> animations { get; set; }

		private Window window;

		public AccentStyleWindow()
		{
			InitializeComponent();

			this.DataContext = this;
			this.window = Application.Current.MainWindow;

			this.Colors = typeof(Colors)
				.GetProperties()
				.Where(prop => typeof(Color).IsAssignableFrom(prop.PropertyType))
				.Select(prop => new KeyValuePair<String, Color>(prop.Name, (Color)prop.GetValue(null)))
				.ToList();

			this.animations = new ObservableCollection<KeyValuePair<string, string>>()
			{
				new KeyValuePair<string, string>("Default", "Default"),
				new KeyValuePair<string, string>("Down", "Down"),
				new KeyValuePair<string, string>("Up", "Up"),
				new KeyValuePair<string, string>("Right", "Right"),
				new KeyValuePair<string, string>("Left", "Left"),
				new KeyValuePair<string, string>("RightReplace", "Right replace"),
				new KeyValuePair<string, string>("LeftReplace", "Left replace"),
			};

			var theme = ThemeManager.DetectAppStyle(Application.Current);
			ThemeManager.ChangeAppStyle(window, theme.Item2, theme.Item1);
		}

		private void ChangeWindowThemeButtonClick(object sender, RoutedEventArgs e)
		{
			var theme = ThemeManager.DetectAppStyle(window);
			ThemeManager.ChangeAppStyle(window, theme.Item2, ThemeManager.GetAppTheme("Base" + ((Button)sender).Content));
		}

		private void ChangeWindowAccentButtonClick(object sender, RoutedEventArgs e)
		{
			var theme = ThemeManager.DetectAppStyle(window);
			ThemeManager.ChangeAppStyle(window, ThemeManager.GetAccent(((Button)sender).Content.ToString()), theme.Item1);
		}

		private void ChangeAppThemeButtonClick(object sender, RoutedEventArgs e)
		{
			var theme = ThemeManager.DetectAppStyle(Application.Current);
			ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, ThemeManager.GetAppTheme("Base" + ((Button)sender).Content));
		}

		private void ChangeAppAccentButtonClick(object sender, RoutedEventArgs e)
		{
			var theme = ThemeManager.DetectAppStyle(Application.Current);
			ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(((Button)sender).Content.ToString()), theme.Item1);
		}

		private void CustomThemeAppButtonClick(object sender, RoutedEventArgs e)
		{
			var theme = ThemeManager.DetectAppStyle(Application.Current);
			ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, ThemeManager.GetAppTheme("CustomTheme"));
		}

		private void CustomAccent1AppButtonClick(object sender, RoutedEventArgs e)
		{
			var theme = ThemeManager.DetectAppStyle(Application.Current);
			ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("CustomAccent1"), theme.Item1);
		}

		private void CustomAccent2AppButtonClick(object sender, RoutedEventArgs e)
		{
			var theme = ThemeManager.DetectAppStyle(Application.Current);
			ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("CustomAccent2"), theme.Item1);
		}

		private void AccentSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedAccent = AccentSelector.SelectedItem as Accent;
			if (selectedAccent != null)
			{
				var theme = ThemeManager.DetectAppStyle(Application.Current);
				ThemeManager.ChangeAppStyle(Application.Current, selectedAccent, theme.Item1);
				Application.Current.MainWindow.Activate();
			}
		}

		private void AnimationSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedAnimation = AnimationSelector.SelectedItem as KeyValuePair<string, string>?;
			if(selectedAnimation != null)
			{
				var content = this.window.Content as MahApps.Metro.Controls.TransitioningContentControl;
				TransitionType transition;
				if(Enum.TryParse(selectedAnimation.Value.Key, out transition))
				{
					content.Transition = transition;
				}

			}
		}
	}
}
