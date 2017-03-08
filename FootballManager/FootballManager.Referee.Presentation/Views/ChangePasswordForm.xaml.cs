using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using FootballManager.Referee.Presentation.Infrastructure;
using FootballManager.Referee.Presentation.ViewModels;
using FunctionalFun.UI;

namespace FootballManager.Referee.Presentation.Views
{
	/// <summary>
	/// Interaction logic for ChangePasswordForm.xaml
	/// </summary>
	public partial class ChangePasswordForm : UserControl
	{
		private PasswordWrapper pw = new PasswordWrapper();
		private PasswordWrapper pw1 = new PasswordWrapper();
		private PasswordWrapper pw2 = new PasswordWrapper();
		
		public ChangePasswordForm()
		{
			InitializeComponent();
			this.oldPasswordBox.DataContext = pw;
			this.newPasswordBox.DataContext = pw1;
			this.repeatPasswordBox.DataContext = pw2;
			this.okButton.DataContext = pw;
		}

		private void ChangePassword()
		{
			var viewModel = this.DataContext as ChangePasswordFormViewModel;
			viewModel.ChangePassword(this.oldPasswordBox.Password, this.newPasswordBox.Password);
		}

		private void OkButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.ChangePassword();
		}

		private void OnPasswordChanged(object sender, RoutedEventArgs e)
		{
			PasswordValidation();
		}

		private void PasswordValidation()
		{
			var newPassword = newPasswordBox.GetBindingExpression(PasswordBoxAssistant.BoundPassword);
			var repeatPassword = repeatPasswordBox.GetBindingExpression(PasswordBoxAssistant.BoundPassword);
			
			if (this.newPasswordBox.Password != this.repeatPasswordBox.Password)
			{

				var validationError = new ValidationError(new DataErrorValidationRule(), newPassword)
				{
					ErrorContent = "Passwords do not match."
				};
				Validation.MarkInvalid(newPassword, validationError);


				validationError = new ValidationError(new DataErrorValidationRule(), repeatPassword)
				{
					ErrorContent = "Passwords do not match."
				};
				Validation.MarkInvalid(repeatPassword, validationError);
			}
			else
			{
				Validation.ClearInvalid(newPassword);
				Validation.ClearInvalid(repeatPassword);
			}
		}
	}
}
