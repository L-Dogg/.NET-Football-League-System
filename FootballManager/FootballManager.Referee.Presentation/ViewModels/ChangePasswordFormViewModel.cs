using FootballManager.Referee.Presentation.Infrastructure;
using FootballManager.Referee.Presentation.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Input;
using FootballManager.Referee.Presentation.RefereeService;

namespace FootballManager.Referee.Presentation.ViewModels
{
	public class ChangePasswordFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly IInteractionService interactionService;
		private readonly IRefereeService refereeService;

		private bool _isEnabled = false;
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set
			{
				_isEnabled = value;
				OnPropertyChanged("IsEnabled");
			}
		}

		public ChangePasswordFormViewModel(IRegionManager regionManager, IInteractionService interactionService, IRefereeService refereeService)
		{
			this.regionManager = regionManager;
			this.interactionService = interactionService;
			this.refereeService = refereeService;
		}

		public async void ChangePassword(string oldPassword, string newPassword)
		{
			IsEnabled = true;
			if (await refereeService.ChangePasswordAsync(CurrentUser.Id, oldPassword, newPassword))
			{
				this.regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
			}
			else
			{
				await this.interactionService.ShowMessageBox("Wrong credentials", "Username or password is incorrect.");
			}
			IsEnabled = false;
		}


		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return false;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{

		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			//this.username = navigationContext.Parameters["username"] as string;
		}
	}
}
