using FootballManager.Referee.Presentation.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FootballManager.Referee.Presentation
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			this.Dispatcher.UnhandledException += App_DispatcherUnhandledException;
			new RefereeBootstrapper().Run();
		}
		/// <summary>
		/// Global exception handler. When exception is thrown within application, it displays dialog modal window with information.
		/// </summary>
		private async void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			e.Handled = true;

			var metroWindow = Current.MainWindow as MetroWindow;
			await metroWindow.ShowMessageAsync("Exception caught", "Unexpected situation occurred.");
		}
	}
}
