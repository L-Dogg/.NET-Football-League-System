using FootballManager.Domain.Entity.Models;
using FootballManager.Presentation.Infrastructure;
using FootballManager.Presentation.Views;
using FootballManager.BusinessLayer;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FootballManager.BusinessLayer.Models;
using System.Linq;

namespace FootballManager.Presentation.ViewModels
{
	public class CoachFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly ILeagueService leagueService;
		private Coach _coach;
		public Coach Coach
		{
			get { return _coach; }
			set
			{
				_coach = value;
				OnPropertyChanged("Coach");
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

		private string _teamLabel;
		public string TeamLabel
		{
			get { return _teamLabel; }
			set
			{
				_teamLabel = value;
				OnPropertyChanged("TeamLabel");
			}
		}

		private string _header = "Add new coach";
		public string Header
		{
			get { return _header; }
			set
			{
				_header = value;
				OnPropertyChanged("Header");
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

		public CoachFormViewModel(IRegionManager regionManager, ILeagueService leagueService)
		{
			this.regionManager = regionManager;
			this.leagueService = leagueService;
			this.Coach = new Coach() { BirthDate = DateTime.Today };
			this.TeamLabel = "No team";
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
			int? coachId = (int?)navigationContext.Parameters["id"];
			if (!coachId.HasValue)
			{
				this.OKCommand = new DelegateCommand(async () =>
				{
					this.IsEnabled = GlobalCommands.BlockWindowButtons();
					await this.leagueService.AddCoach(this.Coach);
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
				});
			}
			else
			{
				this.OKCommand = new DelegateCommand(async () =>
				{
					this.IsEnabled = GlobalCommands.BlockWindowButtons();
					await this.leagueService.SaveChangesAsync();
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
				});
				this.Coach = await this.leagueService.GetCoach(coachId.Value);
				this.TeamLabel = (this.Coach.Teams != null && this.Coach.Teams.Any()) ? string.Join(", ", Array.ConvertAll(this.Coach.Teams.ToArray(), x => x.Name)) : "No team";
				this.Header = "Edit existing coach";
			}
			this.IsEnabled = GlobalCommands.UnlockWindowButtons();
		}
	}
}
