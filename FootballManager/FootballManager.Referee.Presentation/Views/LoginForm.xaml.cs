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
using FootballManager.Referee.Presentation.RefereeService;
using FootballManager.Referee.Presentation.ViewModels;

namespace FootballManager.Referee.Presentation.Views
{
	/// <summary>
	/// Interaction logic for LoginForm.xaml
	/// </summary>
	public partial class LoginForm : UserControl
	{
		public LoginForm()
		{
			InitializeComponent();
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
			var viewModel = this.DataContext as LoginFormViewModel;
			viewModel.PerformLogin(this.passwordBox.Password);
		}

		private void PasswordBox_OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				this.PerformLogin();
		}

	}
}
