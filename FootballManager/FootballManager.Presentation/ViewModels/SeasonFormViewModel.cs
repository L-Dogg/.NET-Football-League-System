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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FootballManager.Presentation.ViewModels
{
	public class SeasonFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly ILeagueService leagueService;
		private readonly IViewToDataService viewToDataService;
		private Season _season;
		public Season Season
		{
			get { return _season; }
			set
			{
				_season = value;
				OnPropertyChanged("Season");
			}
		}
		public ObservableCollection<Table> Table { get; set; } = new ObservableCollection<Table>();
		private ICommand _okCommand;
		public ICommand OKCommand
		{
			get { return _okCommand; }
			set
			{
				_okCommand = value;
				OnPropertyChanged("OKCommand");
			}
		}

		private ICommand _addTeamCommand;
		public ICommand AddTeamCommand
		{
			get { return _addTeamCommand; }
			set
			{
				_addTeamCommand = value;
				OnPropertyChanged("AddTeamCommand");
			}
		}

		private bool _addTeamEnabled = false;
		public bool AddTeamEnabled
		{
			get { return _addTeamEnabled; }
			set
			{
				_addTeamEnabled = value;
				OnPropertyChanged("AddTeamEnabled");
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

		public SeasonFormViewModel(IRegionManager regionManager, IViewToDataService viewToDataService, ILeagueService leagueService)
		{
			this.regionManager = regionManager;
			this.leagueService = leagueService;
			this.viewToDataService = viewToDataService;
			this.Season = new Season();
		}

		/// <summary>
		/// Redirects to selected team's form.
		/// </summary>
		/// <param name="sender">Original sender of event</param>
		/// <param name="e">Original parameters of event</param>
		public void MouseDoubleClick(object sender, MouseEventArgs e)
		{
			var row = this.viewToDataService.DataGridToObject(sender, e);
			if (row == null)
			{
				return;
			}
			var parameter = new NavigationParameters();
			parameter.Add("id", (row as Table).TeamId);
			this.regionManager.RequestNavigate(UiRegions.MainRegion, nameof(TeamForm), parameter);
		}

		/// <summary>
		/// Redirects to selected match's form
		/// </summary>
		/// <param name="sender">>Original sender of event</param>
		/// <param name="e">Original parameters of event</param>
		public void MatchMouseDoubleClick(object sender, MouseEventArgs e)
		{
			var row = this.viewToDataService.DataGridToObject(sender, e);
			if (row == null)
			{
				return;
			}
			var parameter = new NavigationParameters();
			parameter.Add("id", (row as Match).Id);
			this.regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MatchForm), parameter);
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
			int? seasonId = (int?)navigationContext.Parameters["id"];
			if (!seasonId.HasValue)
			{
				this.OKCommand = new DelegateCommand(async () =>
				{
					this.IsEnabled = GlobalCommands.BlockWindowButtons();
					await this.leagueService.AddSeason(this.Season);
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
				});
			}
			else
			{
				this.OKCommand = new DelegateCommand(async () =>
				{
					await this.leagueService.SaveChangesAsync();
				//regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
				GlobalCommands.GoBackCommand.Execute(null);
				});
				this.AddTeamCommand = new DelegateCommand(() =>
				{
					var parameter = new NavigationParameters();
					parameter.Add("id", this.Season.Id);
					this.regionManager.RequestNavigate(UiRegions.MainRegion, nameof(TeamToSeasonForm), parameter);
				});
				this.Season = await this.leagueService.GetSeason(seasonId.Value);
				if (this.Season.Matches != null)
				{
					this.Season.Matches = this.Season.Matches.OrderBy(x => x.Date).ToList();
				}
				OnPropertyChanged("Season");
				if (this.Season.Table != null)
				{
					this.Table.AddRange(this.Season.Table.OrderByDescending(x => x.Points));
				}
				this.AddTeamEnabled = true;
			}
			this.IsEnabled = GlobalCommands.UnlockWindowButtons();
		}
	}
}
