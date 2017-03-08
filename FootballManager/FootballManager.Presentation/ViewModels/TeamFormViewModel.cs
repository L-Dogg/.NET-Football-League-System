using FootballManager.Domain.Entity.Models;
using FootballManager.Presentation.Infrastructure;
using FootballManager.Presentation.Views;
using FootballManager.BusinessLayer;
using FootballManager.BusinessLayer.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FootballManager.Presentation.ViewModels
{
	public class TeamFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly ILeagueService leagueService;
		private readonly IViewToDataService viewToDataService;
		private readonly IInteractionService interactionService;
		private Team _team;
		public Team Team
		{
			get { return _team; }
			set
			{
				_team = value;
				OnPropertyChanged("Team");
			} 
	    }

		private SelectItem _selectedCoach;

		public SelectItem SelectedCoach
		{
			get { return _selectedCoach; }
			set
			{
				_selectedCoach = value;
				OnPropertyChanged("SelectedCoach");
				if (this._selectedCoach != null)
					this.Team.CoachId = this._selectedCoach.Id;
			}
		}

		private SelectItem _selectedStadium;

		public SelectItem SelectedStadium
		{
			get { return _selectedStadium; }
			set
			{
				_selectedStadium = value;
				OnPropertyChanged("SelectedStadium");
				if (this._selectedStadium != null)
					this.Team.StadiumId = this._selectedStadium.Id;
			}
		}

		public ObservableCollection<SelectItem> Coaches { get; set; } = new ObservableCollection<SelectItem>();
		public ObservableCollection<SelectItem> Stadiums { get; set; } = new ObservableCollection<SelectItem>();
		public ObservableCollection<Footballer> Footballers { get; set; } = new ObservableCollection<Footballer>();
		public ObservableCollection<Table> Tables { get; set; } = new ObservableCollection<Table>();

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

		private ICommand _okCommand;
		public ICommand OKCommand {
			get { return _okCommand; }
			set
			{
				_okCommand = value;
				OnPropertyChanged("OKCommand");
			}
		}

		private ICommand _removePlayerCommand;
		public ICommand RemovePlayerCommand
		{
			get { return _removePlayerCommand; }
			set
			{
				_removePlayerCommand = value;
				OnPropertyChanged("RemovePlayerCommand");
			}
		}

		private ICommand _removeSeasonCommand;
		public ICommand RemoveSeasonCommand
		{
			get { return _removeSeasonCommand; }
			set
			{
				_removeSeasonCommand = value;
				OnPropertyChanged("RemoveSeasonCommand");
			}
		}

		public TeamFormViewModel(IRegionManager regionManager, IViewToDataService viewToDataService, ILeagueService leagueService, IInteractionService interactionService)
		{
			this.regionManager = regionManager;
			this.leagueService = leagueService;
			this.viewToDataService = viewToDataService;
			this.interactionService = interactionService;
			this.Team = new Team() { Address = new Address()};
			this.RemovePlayerCommand = new DelegateCommand<Footballer>((Footballer player) =>
			{
				if (player != null)
				{
					this.leagueService.RemovePlayerFromTeam(player);
					this.Footballers.Remove(player);
					OnPropertyChanged("Team");
				}
			});

			this.RemoveSeasonCommand = new DelegateCommand<Table>(async (Table table) =>
			{
				if (table != null)
				{
					this.IsEnabled = GlobalCommands.BlockWindowButtons();
					var result = await this.leagueService.RemoveTeamFromSeason(table.Id);
					if (result)
					{
						this.Tables.Remove(table);
					}
					else
					{
						await this.interactionService.ShowMessageBox("Cannot remove team from season", "This team is already included in fixtures");
					}
					this.IsEnabled = GlobalCommands.UnlockWindowButtons();
				}
			});
		}

		/// <summary>
		/// Redirects to selected player's form.
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
			parameter.Add("id", (row as Footballer).Id);
			this.regionManager.RequestNavigate(UiRegions.MainRegion, nameof(FootballerForm), parameter);
		}

		/// <summary>
		/// Redirects to selected season's form
		/// </summary>
		/// <param name="sender">Original sender of event</param>
		/// <param name="e">Original parameters of event</param>
		public void SeasonMouseDoubleClick(object sender, MouseEventArgs e)
		{
			var row = this.viewToDataService.DataGridToObject(sender, e);
			if (row == null)
			{
				return;
			}
			var parameter = new NavigationParameters();
			parameter.Add("id", (row as Table).SeasonId);
			this.regionManager.RequestNavigate(UiRegions.MainRegion, nameof(SeasonForm), parameter);
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
			this.Coaches.AddRange(await this.leagueService.GetCoachList());
			this.Stadiums.AddRange(await this.leagueService.GetStadiumsList());
			int? teamId = (int?)navigationContext.Parameters["id"];
			if (!teamId.HasValue)
			{
				this.OKCommand = new DelegateCommand(async () =>
				{
					this.IsEnabled = GlobalCommands.BlockWindowButtons();
					await this.leagueService.AddTeam(this.Team);
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
				});
			}
			else
			{
				this.OKCommand = new DelegateCommand(async () =>
				{
					this.IsEnabled = GlobalCommands.BlockWindowButtons();
					this.Team.CoachId = this.SelectedCoach.Id;
					this.Team.StadiumId = this.SelectedStadium.Id;
					await this.leagueService.SaveChangesAsync();
				//regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
					GlobalCommands.GoBackCommand.Execute(null);
				});
				this.Team = await this.leagueService.GetTeam(teamId.Value);
				this.SelectedCoach = Coaches.FirstOrDefault(c => c.Id == Team.CoachId);
				this.SelectedStadium = Stadiums.FirstOrDefault(s => s.Id == Team.StadiumId);
				if (this.Team.Footballers != null)
				{
					this.Footballers.AddRange(Team.Footballers);
				}
				if (this.Team.Table != null)
				{
					this.Tables.AddRange(Team.Table);
				}
			}
			this.IsEnabled = GlobalCommands.UnlockWindowButtons();
		}
	}
}
