using FootballManager.Referee.Presentation.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Input;
using FootballManager.Domain.Entity.Models.Authentication.Enums;
using FootballManager.Referee.Presentation.RefereeService;
using FootballManager.Referee.Presentation.Views;

namespace FootballManager.Referee.Presentation.ViewModels
{
	public class LoginFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly IRefereeService refereeService;
		private readonly IInteractionService interactionService;

		public string userName { get; set; }

		public ICommand NavigateAwayCommand { get; }

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

		public LoginFormViewModel(IRegionManager regionManager, IInteractionService interactionService, IRefereeService refereeService)
		{
			this.regionManager = regionManager;
			this.interactionService = interactionService;
			this.refereeService = refereeService;
			this.NavigateAwayCommand = new DelegateCommand(() =>
			{
				regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
			});
		}
		
		public async void PerformLogin(string password)
		{
			this.IsEnabled = true;
			CurrentUser.Id = await refereeService.AuthenticateRefereeAsync(this.userName, password, UserType.Referee);
			this.IsEnabled = false;
			if (CurrentUser.Id != -1)
			{
				this.NavigateAwayCommand.Execute(null);
				return;
			}
			else
			{
				await this.interactionService.ShowMessageBox("Wrong credentials", "Username or password is incorrect.");
			}
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
		}
	}
}
