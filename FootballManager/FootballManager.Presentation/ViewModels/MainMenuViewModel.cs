using FootballManager.Presentation.Infrastructure;
using FootballManager.Presentation.Views;
using FootballManager.BusinessLayer;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using FootballManager.BusinessLayer.Models;

namespace FootballManager.Presentation.ViewModels
{
	public class MainMenuViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly LeagueService leagueService;

		public ICommand AddTeamCommand { get; }
		public ICommand EditTeamCommand { get; }
		public ICommand AddPlayerCommand { get; }
		public ICommand EditPlayerCommand { get; }
		public ICommand AddCoachCommand { get; }
		public ICommand EditCoachCommand { get; }
		public ICommand AddStadiumCommand { get; }
		public ICommand EditStadiumCommand { get; }
		public ICommand AddSeasonCommand { get; }
		public ICommand EditSeasonCommand { get; }
		public ICommand AddRefereeCommand { get; }
		public ICommand EditRefereeCommand { get; }
		public ICommand GenerateFixturesCommand { get; }

		public MainMenuViewModel(IRegionManager regionManager, LeagueService leagueService)
		{
			this.regionManager = regionManager;
			this.leagueService = leagueService;
			this.AddTeamCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(TeamForm)));
			this.AddPlayerCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(FootballerForm)));
			this.AddCoachCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(CoachForm)));
			this.AddStadiumCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(StadiumForm)));
			this.AddRefereeCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(RefereeForm)));
			this.GenerateFixturesCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(FixturesForm)));
			this.AddSeasonCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(SeasonForm)));

			var EditTeamParameters = new NavigationParameters();
			EditTeamParameters.Add("nextView", nameof(TeamForm));
			//EditTeamParameters.Add("list", this.leagueService.GetTeamsList());
			EditTeamParameters.Add("filter", Filter.Team);
			this.EditTeamCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(SelectEditItem), EditTeamParameters));

			var EditPlayerParameters = new NavigationParameters();
			EditPlayerParameters.Add("nextView", nameof(FootballerForm));
			//EditPlayerParameters.Add("list", this.leagueService.GetFootballersList());
			EditPlayerParameters.Add("filter", Filter.Footballer);
			this.EditPlayerCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(SelectEditItem), EditPlayerParameters));

			var EditCoachParameters = new NavigationParameters();
			EditCoachParameters.Add("nextView", nameof(CoachForm));
			//EditCoachParameters.Add("list", this.leagueService.GetCoachList());
			EditCoachParameters.Add("filter", Filter.Coach);
			this.EditCoachCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(SelectEditItem), EditCoachParameters));

			var EditStadiumParameters = new NavigationParameters();
			EditStadiumParameters.Add("nextView", nameof(StadiumForm));
			//EditStadiumParameters.Add("list", this.leagueService.GetStadiumsList());
			EditStadiumParameters.Add("filter", Filter.Stadium);
			this.EditStadiumCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(SelectEditItem), EditStadiumParameters));

			var EditRefereeParameters = new NavigationParameters();
			EditRefereeParameters.Add("nextView", nameof(RefereeForm));
			//EditRefereeParameters.Add("list", this.leagueService.GetRefereeList());
			EditRefereeParameters.Add("filter", Filter.Referee);
			this.EditRefereeCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(SelectEditItem), EditRefereeParameters));

			var EditSeasonParameters = new NavigationParameters();
			EditSeasonParameters.Add("nextView", nameof(SeasonForm));
			//EditSeasonParameters.Add("list", this.leagueService.GetSeasonsList());
			EditSeasonParameters.Add("filter", Filter.Season);
			this.EditSeasonCommand = new DelegateCommand(() =>
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(SelectEditItem), EditSeasonParameters));

		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return false;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			this.leagueService.Dispose();
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
		}
	}
}
