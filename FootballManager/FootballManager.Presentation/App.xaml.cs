using FootballManager.BusinessLayer;
using Microsoft.Practices.Unity;
using System.Windows;
using System.Windows.Threading;
using FootballManager.Domain.Entity.Contexts.AuthenticationContext;
using FootballManager.Domain.Entity.Contexts.LeagueContext;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using FootballManager.Presentation.Infrastructure;

namespace FootballManager.Presentation
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
			new FootballManagerBootstrapper().Run();
		}
		/// <summary>
		/// Global exception handler. When exception is thrown within application, it displays dialog modal window with information.
		/// </summary>
		private async void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			e.Handled = true;

			var metroWindow = Current.MainWindow as MetroWindow;
			//TODO: message
			await metroWindow.ShowMessageAsync("Exception caught", "Unexpected situation occurred.");
		}
	}
}
