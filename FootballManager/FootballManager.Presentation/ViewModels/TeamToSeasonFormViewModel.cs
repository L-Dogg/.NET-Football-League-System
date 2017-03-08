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
	public class TeamToSeasonFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly ILeagueService leagueService;
		private readonly IInteractionService interactionService;
		private int seasonId;

		public ObservableCollection<SelectItem> Teams { get; set; } = new ObservableCollection<SelectItem>();
		private SelectItem _selectedItem;
		public SelectItem SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;
				OnPropertyChanged("SelectedItem");
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

		public TeamToSeasonFormViewModel(IRegionManager regionManager, IInteractionService interactionService, ILeagueService leagueService)
		{
			this.regionManager = regionManager;
			this.interactionService = interactionService;
			this.leagueService = leagueService;
			this.OKCommand = new DelegateCommand(async () =>
			{
				if (this.SelectedItem != null)
				{
					this.IsEnabled = GlobalCommands.BlockWindowButtons();
					var result = this.leagueService.AssignTeamToSeason(SelectedItem.Id, this.seasonId);
					if(!(await result))
					{
						await this.interactionService.ShowMessageBox("Team already added", "This team was already assigned to this season.");
						this.IsEnabled = GlobalCommands.UnlockWindowButtons();
						return;
					}
				}
				else
				{
					await this.interactionService.ShowMessageBox("No seasons available", "To assign team to season, add season first.");
				}
				GlobalCommands.GoBackCommand.Execute(null);
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
			this.seasonId = (int)navigationContext.Parameters["id"];
			this.Teams.AddRange(await this.leagueService.GetTeamsList());
			//if(this.Teams.Any())
			//{
			//	this.SelectedItem = this.Teams.First();
			//}
			this.IsEnabled = GlobalCommands.UnlockWindowButtons();
		}
	}
}
