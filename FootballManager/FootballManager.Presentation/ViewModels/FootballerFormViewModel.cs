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
using System.Windows.Input;

namespace FootballManager.Presentation.ViewModels
{
	public class FootballerFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly ILeagueService leagueService;
		private readonly IInteractionService interactionService;
		private Footballer _footballer;
		public Footballer Footballer
		{
			get { return _footballer; }
			set
			{
				_footballer = value;
				OnPropertyChanged("Footballer");
			}
		}

		private SelectItem _selectedTeam;

		public SelectItem SelectedTeam
		{
			get { return _selectedTeam; }
			set
			{
				_selectedTeam = value;
				OnPropertyChanged("SelectedTeam");
				if (this._selectedTeam != null)
					this.Footballer.TeamId = _selectedTeam.Id;
			}
		}

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

		private ICommand _clearTeamCommand;
		public ICommand ClearTeamCommand
		{
			get { return _clearTeamCommand; }
			set
			{
				_clearTeamCommand = value;
				OnPropertyChanged("ClearTeamCommand");
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

		private string _searchedText;
		public string SearchedText
		{
			get { return _searchedText; }
			set
			{
				_searchedText = value;
				OnPropertyChanged("SearchedText");
			}
		}

		public ObservableCollection<Goal> Goals { get; set; } = new ObservableCollection<Goal>();
		public ObservableCollection<SelectItem> Teams { get; set; } = new ObservableCollection<SelectItem>();

		public FootballerFormViewModel(IRegionManager regionManager, IInteractionService interactionService, ILeagueService leagueService)
		{
			this.regionManager = regionManager;
			this.leagueService = leagueService;
			this.interactionService = interactionService;
			this.Footballer = new Footballer();
			this.ClearTeamCommand = new DelegateCommand(() =>
			{
				this.Footballer.TeamId = null;
				OnPropertyChanged("Footballer");
				this.SelectedTeam = null;
				OnPropertyChanged("SelectedTeam");
				this.SearchedText = "";
			});
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
			this.Teams.AddRange(await this.leagueService.GetTeamsList());
			int? playerId = (int?)navigationContext.Parameters["id"];
			if (!playerId.HasValue)
			{
				this.OKCommand = new DelegateCommand(async () =>
				{
					this.IsEnabled = GlobalCommands.BlockWindowButtons();
					this.Footballer.TeamId = SelectedTeam?.Id;
					var result = await this.leagueService.AddFootballer(this.Footballer);
					if (!result)
					{
						await this.interactionService.ShowMessageBox("Cannot add player", "Salary is too high.");
						this.IsEnabled = GlobalCommands.UnlockWindowButtons();
					}
					else
					{
						regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
					}
				});
			}
			else
			{
				this.OKCommand = new DelegateCommand(async () =>
				{
					this.IsEnabled = GlobalCommands.BlockWindowButtons();
					if (await this.leagueService.CanAddFootballer(this.Footballer))
					{
						this.Footballer.TeamId = SelectedTeam?.Id;
						await this.leagueService.SaveChangesAsync();
					GlobalCommands.GoBackCommand.Execute(null);
					}
					else
					{
						await this.interactionService.ShowMessageBox("Cannot edit player", "Salary is too high.");
						this.IsEnabled = GlobalCommands.UnlockWindowButtons();
					}
				});
				this.Footballer = await this.leagueService.GetFootballer(playerId.Value);
				this.SelectedTeam = Teams.FirstOrDefault(t => t.Id == this.Footballer.TeamId);
				if (this.Footballer.Goals != null)
				{
					this.Goals.AddRange(this.Footballer.Goals);
				}
			}
			this.IsEnabled = GlobalCommands.UnlockWindowButtons();
		}
	}
}
