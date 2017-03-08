using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using FootballManager.Presentation.ViewModels;

namespace FootballManager.Presentation.Views
{
	/// <summary>
	/// Interaction logic for LoginForm.xaml
	/// </summary>
	public partial class LoginForm : UserControl
	{
		public LoginForm()
		{
			InitializeComponent();
			this.usernameTextbox.Focusable = true;
			//this.usernameTextbox.Focus();
			//Keyboard.Focus(this.usernameTextbox);
		}
		
		/// <summary>
		/// Shuts down application.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CancelButton_OnClick(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		/// <summary>
		/// Perform login operation.
		/// </summary>
		private void OkButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.PerformLogin();
		}

		private void PerformLogin()
		{
			LoginFormViewModel viewModel = this.DataContext as LoginFormViewModel;
			viewModel.PerformLogin(this.passwordBox.Password);
		}

		private void PasswordBox_OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				this.PerformLogin();
		}

	}
}
