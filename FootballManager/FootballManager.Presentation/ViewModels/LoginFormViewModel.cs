using FootballManager.Presentation.Infrastructure;
using FootballManager.Presentation.Views;
using FootballManager.BusinessLayer;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FootballManager.AuthenticationLayer;
using FootballManager.Domain.Entity.Models.Authentication.Enums;

namespace FootballManager.Presentation.ViewModels
{
	public class LoginFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly IAuthenticationService authenticationService;
		private readonly IInteractionService interactionService;
		public string userName { get; set; }

		public ICommand NavigateAwayCommand { get; }

		public LoginFormViewModel(IRegionManager regionManager, IInteractionService interactionService, IAuthenticationService leagueService)
		{
			this.regionManager = regionManager;
			this.interactionService = interactionService;
			this.NavigateAwayCommand = new DelegateCommand(() =>
			{
				regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
			});
			this.authenticationService = leagueService;
		}


		public async void PerformLogin(string password)
		{
			if (authenticationService.AuthenticateUser(this.userName, password, UserType.Admin) != -1)
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
			this.authenticationService.Dispose();
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
		}
	}
}
