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
	public class RefereeFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly ILeagueService leagueService;
		private readonly IViewToDataService viewToDataService;
		private Referee _referee;
		public Referee Referee
		{
			get { return _referee; }
			set
			{
				_referee = value;
				OnPropertyChanged("Referee");
			}
		}
		
		public ObservableCollection<Match> Matches { get; set; } = new ObservableCollection<Match>();
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

		public RefereeFormViewModel(IRegionManager regionManager, IViewToDataService viewToDataService, ILeagueService leagueService)
		{
			this.regionManager = regionManager;
			this.leagueService = leagueService;
			this.viewToDataService = viewToDataService;
			this.Referee = new Referee();
			
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
			int? refereeId = (int?)navigationContext.Parameters["id"];
			if (!refereeId.HasValue)
			{
				this.OKCommand = new DelegateCommand(async () =>
				{
					this.IsEnabled = GlobalCommands.BlockWindowButtons();
					await this.leagueService.AddReferee(this.Referee);
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
				this.Referee = await this.leagueService.GetReferee(refereeId.Value);
				if (this.Referee.Matches != null)
				{
					this.Matches.AddRange(this.Referee.Matches);
				}
			}
			this.IsEnabled = GlobalCommands.UnlockWindowButtons();
		}

		/// <summary>
		/// Redirects to selected match's form
		/// </summary>
		/// <param name="sender">Original sender of event</param>
		/// <param name="e">Original parameters of event</param>
		public void MatchesMouseDoubleClick(object sender, MouseEventArgs e)
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
	}
}
