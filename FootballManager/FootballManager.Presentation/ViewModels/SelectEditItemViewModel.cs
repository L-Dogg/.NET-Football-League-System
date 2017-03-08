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
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FootballManager.Domain.Entity.Models;

namespace FootballManager.Presentation.ViewModels
{
	public class SelectEditItemViewModel : BindableBase, INavigationAware
	{
		private readonly IRegionManager regionManager;
		private readonly ILeagueService leagueService;
		private readonly IViewToDataService viewToDataService;

		/// <summary>
		/// Function that performs query to database. 
		/// It should be given in NavigationContext.Parameters["filter"].
		/// </summary>
		private Func<string, ILeagueService, Task<List<SelectItem>>> filterFunc;

		public ObservableCollection<SelectItem> List { get; set; } = new ObservableCollection<SelectItem>();

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

		public ICommand NextViewCommand { get; } 
		private string nextView;

		public SelectEditItemViewModel(IRegionManager regionManager, IViewToDataService viewToDataService, ILeagueService leagueService)
		{
			this.regionManager = regionManager;
			this.leagueService = leagueService;
			this.viewToDataService = viewToDataService;
			this.NextViewCommand = new DelegateCommand(() =>
			{
				if(this.SelectedItem == null)
				{
					return;
				}
				var parameter = new NavigationParameters();
				parameter.Add("id", this.SelectedItem.Id);
				regionManager.RequestNavigate(UiRegions.MainRegion, this.nextView, parameter);
			});
		}
		
		/// <summary>
		/// Performs search query using filterFunc for text input.
		/// </summary>
		/// <param name="text">User input for search query.</param>
		public async void Search(string text)
		{
			this.List.Clear();
			if (text.Length == 0)
				return;

			var result = await this.filterFunc.Invoke(text, this.leagueService);
			this.List.AddRange(result);
			if (List.Count > 0)
				this.SelectedItem = List.First();
			GlobalCommands.UnlockWindowButtons();
		}

		public void DataGridItemClicked(object sender, MouseEventArgs e)
		{
			var row = this.viewToDataService.DataGridToObject(sender, e);
			if (row == null)
			{
				return;
			}
			var parameter = new NavigationParameters();
			parameter.Add("id", ((SelectItem)row).Id);
			this.regionManager.RequestNavigate(UiRegions.MainRegion, this.nextView, parameter);
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return false;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			this.leagueService.Dispose();
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			this.nextView = (string)navigationContext.Parameters["nextView"];
			this.filterFunc = FilterDictionary.GetFilter((Filter) navigationContext.Parameters["filter"]);
		}
	}
}
