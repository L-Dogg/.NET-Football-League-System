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
	public class StadiumFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly ILeagueService leagueService;
		private Stadium _stadium;
		public Stadium Stadium
		{
			get { return _stadium; }
			set
			{
				_stadium = value;
				OnPropertyChanged("Stadium");
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

		private string _header = "Add new stadium";
		public string Header
		{
			get { return _header; }
			set
			{
				_header = value;
				OnPropertyChanged("Header");
			}
		}

		public StadiumFormViewModel(IRegionManager regionManager, ILeagueService leagueService)
		{
			this.regionManager = regionManager;
			this.leagueService = leagueService;
			this.Stadium = new Stadium() { Address = new Address() };
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
			int? stadiumId = (int?)navigationContext.Parameters["id"];
			if (!stadiumId.HasValue)
			{
				this.OKCommand = new DelegateCommand(async () =>
				{
					await this.leagueService.AddStadium(this.Stadium);
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
				});
			}
			else
			{
				this.OKCommand = new DelegateCommand(async () =>
				{
					await this.leagueService.SaveChangesAsync();
					regionManager.RequestNavigate(UiRegions.MainRegion, nameof(MainMenu));
				});
				this.Stadium = await this.leagueService.GetStadium(stadiumId.Value);
				this.Header = "Edit existing stadium";
			}
			this.IsEnabled = GlobalCommands.UnlockWindowButtons();
		}
	}
}
