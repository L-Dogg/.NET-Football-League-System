using System;
using FootballManager.Domain.Entity.Models;
using FootballManager.Presentation.Infrastructure;
using FootballManager.Presentation.Views;
using FootballManager.BusinessLayer;
using FootballManager.BusinessLayer.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace FootballManager.Presentation.ViewModels
{
	public class MatchFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly ILeagueService leagueService;
		private Match _match;
		public Match Match
		{
			get { return _match; }
			set
			{
				_match = value;
				OnPropertyChanged("Match");
			}
		}

		private bool _isEnabled = true;
		public bool IsEnabled
		{
			get { return _isEnabled; }
			set
			{
				_isEnabled = value;
				OnPropertyChanged("IsEnabled");
			}
		}
		public ObservableCollection<Goal> HomeGoals { get; set; } = new ObservableCollection<Goal>();
		public ObservableCollection<Goal> AwayGoals { get; set; } = new ObservableCollection<Goal>();

		public MatchFormViewModel(IRegionManager regionManager, ILeagueService leagueService)
		{
			this.regionManager = regionManager;
			this.leagueService = leagueService;
			this.Match = new Match() { Date = DateTime.Today };
		}

		/// <summary>
		/// Redirects to home team's form
		/// </summary>
		/// <param name="sender">Original sender of event</param>
		/// <param name="e">Original parameters of event</param>
		public void HomeTeamMouseClick(object sender, MouseEventArgs e)
		{
			var parameter = new NavigationParameters();
			parameter.Add("id", this.Match.HomeTeamId);
			this.regionManager.RequestNavigate(UiRegions.MainRegion, nameof(TeamForm), parameter);
		}

		/// <summary>
		/// Redirect to away team's form.
		/// </summary>
		/// <param name="sender">Original sender of event</param>
		/// <param name="e">Original parameters of event</param>
		public void AwayTeamMouseClick(object sender, MouseEventArgs e)
		{
			var parameter = new NavigationParameters();
			parameter.Add("id", this.Match.AwayTeamId);
			this.regionManager.RequestNavigate(UiRegions.MainRegion, nameof(TeamForm), parameter);
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return false;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			this.IsEnabled = GlobalCommands.UnlockWindowButtons();
			this.leagueService.Dispose();
		}

		public async void OnNavigatedTo(NavigationContext navigationContext)
		{
			this.IsEnabled = GlobalCommands.BlockWindowButtons();
			this.Match = await this.leagueService.GetMatch((int)navigationContext.Parameters["id"]);
			this.AwayGoals.AddRange(Match.Goals.Where(g => g.TeamID == this.Match.AwayTeamId));
			this.HomeGoals.AddRange(Match.Goals.Where(g => g.TeamID == this.Match.HomeTeamId));
			this.IsEnabled = GlobalCommands.UnlockWindowButtons();
		}
	}
}
