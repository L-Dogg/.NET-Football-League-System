using FootballManager.Referee.Presentation.Infrastructure;
using FootballManager.Referee.Presentation.RefereeService;
using FootballManager.Referee.Presentation.Views;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FootballManager.Referee.Presentation.ViewModels
{
	public class MatchListFormViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly IRefereeService refereeService;
		private readonly IViewToDataService viewToDataService;
		public ObservableCollection<MatchListItem> Matches { get; set; } = new ObservableCollection<MatchListItem>();

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

		public MatchListFormViewModel(IRegionManager regionManager, IRefereeService refereeService, IViewToDataService viewToDataService)
		{
			this.regionManager = regionManager;
			this.refereeService = refereeService;
			this.viewToDataService = viewToDataService;
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
			var match = row as MatchListItem;
			parameter.Add("id", match.Id);
			bool isEditing = false;
			if (DateTime.Today.Day - match.Date.Day == 0 || DateTime.Today.Day - match.Date.Day == 1)
			{
				isEditing = true;
			}
			parameter.Add("isEditing", isEditing);
			this.regionManager.RequestNavigate(UiRegions.MainRegion, nameof(EditMatchForm), parameter);
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return false;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
		}

		public async void OnNavigatedTo(NavigationContext navigationContext)
		{
			this.Matches.AddRange(await this.refereeService.GetMatchesListAsync(CurrentUser.Id));
			this.IsEnabled = false;
		}
	}
}
