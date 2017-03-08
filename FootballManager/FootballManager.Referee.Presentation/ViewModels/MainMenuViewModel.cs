using FootballManager.Referee.Presentation.Infrastructure;
using FootballManager.Referee.Presentation.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Input;
using FootballManager.Referee.Presentation.RefereeService;

namespace FootballManager.Referee.Presentation.ViewModels
{
	public class MainMenuViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly IRefereeService service;
		private readonly IInteractionService interaction;

		public ICommand ChooseMatchCommand { get; }
		public ICommand ChangePasswordCommand { get; }
		public ICommand GetPlayer { get; }

		//private string username;

		public MainMenuViewModel(IRegionManager regionManager, IRefereeService service, IInteractionService interaction)
		{
			this.regionManager = regionManager;
			this.service = service;
			this.interaction = interaction;
			this.ChangePasswordCommand = new DelegateCommand(() => 
				regionManager.RequestNavigate(UiRegions.MainRegion, nameof(ChangePasswordForm)));
			this.ChooseMatchCommand = new DelegateCommand(() =>
				regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MatchListForm)));
			this.GetPlayer = new DelegateCommand(() => this.interaction.ShowMessageBox("title", (this.service.GetPlayer()).FirstName));
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
