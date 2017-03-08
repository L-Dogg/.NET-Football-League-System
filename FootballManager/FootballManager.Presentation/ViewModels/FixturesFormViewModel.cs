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
using System.Windows.Input;
using System.Threading.Tasks;
using System.Linq;

namespace FootballManager.Presentation.ViewModels
{
	public class FixturesFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly ILeagueService leagueService;
		private readonly IInteractionService interactionService;

		/// <summary>
		/// Date of league start (date of first round).
		/// </summary>
		private DateTime _startingDate { get; set; } = DateTime.Today;
		/// <summary>
		/// Interval between rounds.
		/// </summary>
		private int _intervalValue { get; set; } = 7;

		public DateTime StartingDate
		{
			get { return _startingDate;}
			set
			{
				_startingDate = value;
				OnPropertyChanged("StartingDate");
			}
		}

		public int IntervalValue
		{
			get { return _intervalValue; }
			set
			{
				_intervalValue = value;
				OnPropertyChanged("IntervalValue");
			}
		}

		public ObservableCollection<SelectItem> Seasons { get; set; } = new ObservableCollection<SelectItem>();
		private SelectItem _selectedItem;
		public SelectItem SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				this._selectedItem = value;
				OnPropertyChanged("SelectedItem");
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

		public FixturesFormViewModel(IRegionManager regionManager, IInteractionService interactionService, ILeagueService leagueService)
		{
			this.regionManager = regionManager;
			this.leagueService = leagueService;
			this.interactionService = interactionService;
			this.OKCommand = new DelegateCommand(async () =>
			{
				this.IsEnabled = GlobalCommands.BlockWindowButtons();
				var result = await this.leagueService.GenerateFixtures(this.SelectedItem.Id, this.StartingDate, this.IntervalValue);
				if (!result)
				{
					this.IsEnabled = GlobalCommands.UnlockWindowButtons();
					await this.interactionService.ShowConfirmationMessage("Fixture already exists", "Fixture can't be generated, because it already exists. Do you want to override?", async () =>
					{
						this.IsEnabled = GlobalCommands.BlockWindowButtons();
						await this.leagueService.GenerateFixtures(this.SelectedItem.Id, this.StartingDate, this.IntervalValue, true);
						regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
					});
				}
				else
				{
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
				}
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
			this.Seasons.AddRange(await this.leagueService.GetSeasonsList());
			this.IsEnabled = GlobalCommands.UnlockWindowButtons();
		}
	}
}
