using FootballManager.Referee.Presentation.RefereeService;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
//using BingMapsRESTService.Common.JSON;
using FootballManager.Referee.Presentation.Infrastructure;
using Microsoft.Maps.MapControl.WPF;
using Prism.Commands;
using Location = Microsoft.Maps.MapControl.WPF.Location;
using System.Text.RegularExpressions;
using System.Windows.Media;
using FootballManager.Referee.Presentation.Properties;
using System.Globalization;
using FootballManager.Domain.Entity.Enums;
using FootballManager.Referee.Presentation.BingServices;

namespace FootballManager.Referee.Presentation.ViewModels
{
	public class EditMatchFormViewModel : BindableBase, INavigationAware
	{
		public Map DirectionsMap { get; set; }

		private bool _flyoutOpen;
		public bool FlyoutOpen
		{
			get { return this._flyoutOpen; }
			set
			{
				_flyoutOpen = value;
				OnPropertyChanged("FlyoutOpen");
			}
		}

		private readonly IRegionManager regionManager;
		private readonly IRefereeService refereeService;
		private readonly IInteractionService interactionService;
		private readonly IGeocodingService geocodingService;

		#region Match Fields
		public ObservableCollection<GoalDTO> HomeGoals { get; set; } = new ObservableCollection<GoalDTO>();
		public ObservableCollection<GoalDTO> AwayGoals { get; set; } = new ObservableCollection<GoalDTO>();
		public ObservableCollection<Tuple<int, string>> Teams { get; set; } = new ObservableCollection<Tuple<int, string>>();
		public ObservableCollection<PlayerListItem> Players { get; set; } = new ObservableCollection<PlayerListItem>();
		public ObservableCollection<GoalType> GoalTypes { get; set; } = new ObservableCollection<GoalType>() { GoalType.Normal, GoalType.Freekick, GoalType.Penalty, GoalType.Own };
		
		private Tuple<int, string> _selectedTeam;
		public Tuple<int, string> SelectedTeam
		{
			get { return _selectedTeam; }
			set
			{
				_selectedTeam = value;
				Players.Clear();
				Players.AddRange(value.Item1 == Match.HomeTeamId ? Match.HomeTeamPlayers : Match.AwayTeamPlayers);
				SelectedPlayer = Players.First();
				OnPropertyChanged("SelectedTeam");
			}
		}

		private PlayerListItem _selectedPlayer;
		public PlayerListItem SelectedPlayer
		{
			get { return _selectedPlayer; }
			set
			{
				_selectedPlayer = value;
				OnPropertyChanged("SelectedPlayer");
			}
		}

		private GoalType _selectedType;
		public GoalType SelectedType
		{
			get { return _selectedType; }
			set
			{
				_selectedType = value;
				OnPropertyChanged("SelectedType");
			}
		}

		private int _time = 1;
		public int Time
		{
			get { return _time; }
			set
			{
				_time = value;
				OnPropertyChanged("Time");
			}
		}

		private bool _isEditing { get; set; } = true;
		public bool IsEditing
		{
			get { return _isEditing; }
			set
			{
				_isEditing = value;
				OnPropertyChanged("IsEditing");
			}
		}

		private MatchDTO _match;
		public MatchDTO Match
		{
			get { return _match; }
			set
			{
				_match = value;
				OnPropertyChanged("Match");
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

		#endregion

		private ICommand _searchCommand;
		public ICommand SearchCommand
		{
			get { return _searchCommand; }
			set
			{
				_searchCommand = value;
				OnPropertyChanged("SearchCommand");
			}
		}

		public ICommand AddGoalCommand { get; set; }
		public ICommand RemoveGoalCommand { get; set; }
		public ICommand OkCommand { get; set; }
		public ICommand CalculateRouteCommand { get; private set; }

		#region Map Properties
		private string searchQuery;
		public string SearchQuery
		{
			get { return searchQuery; }
			set
			{
				searchQuery = value;
				OnPropertyChanged("SearchQuery");
			}
		}

		private string _from = "Politechnika Warszawska, Warsaw, Poland";
		public string From
		{
			get { return _from; }
			set
			{
				_from = value;
				OnPropertyChanged("From");
			}
		}

		private string _to = "Piastow, Poland";
		public string To
		{
			get { return _to; }
			set
			{
				_to = value;
				OnPropertyChanged("To");
			}
		}

		private RouteResult _routeResult;
		public RouteResult RouteResult
		{
			get { return _routeResult; }
			set
			{
				_routeResult = value;
				OnPropertyChanged("RouteResult");
			}
		}

		private ObservableCollection<Direction> _directions;
		public ObservableCollection<Direction> Directions
		{
			get { return _directions; }
			set
			{
				_directions = value;
				OnPropertyChanged("Directions");
			}
		}

		#endregion

		public EditMatchFormViewModel(IRegionManager regionManager, IRefereeService refereeService, IInteractionService interactionService, IGeocodingService geocodingService)
		{
			CalculateRouteCommand = new MyDelegateCommand(CalculateRoute);

			this.regionManager = regionManager;
			this.refereeService = refereeService;
			this.interactionService = interactionService;
			this.geocodingService = geocodingService;

			AddGoalCommand = new DelegateCommand(() =>
			{
				var teamId = Match.HomeTeamPlayers.Contains(SelectedPlayer) ? (SelectedType == GoalType.Own ? Match.AwayTeamId : Match.HomeTeamId) :
				(SelectedType == GoalType.Own ? Match.HomeTeamId : Match.AwayTeamId);
				var goal = new GoalDTO()
				{
					Scorer = SelectedPlayer,
					GoalType = SelectedType,
					TeamId = teamId,
					Time = Time
				};
				if(teamId == Match.HomeTeamId)
				{
					HomeGoals.Add(goal);
					var ordered = this.HomeGoals.OrderBy(x => x.Time).ToList();
					this.HomeGoals.Clear();
					this.HomeGoals.AddRange(ordered);
					Match.HomeTeamScore = (Match.HomeTeamScore ?? 0) + 1;
				}
				else
				{
					AwayGoals.Add(goal);
					var ordered = this.AwayGoals.OrderBy(x => x.Time).ToList();
					this.AwayGoals.Clear();
					this.AwayGoals.AddRange(ordered);
					Match.AwayTeamScore = (Match.AwayTeamScore ?? 0) + 1;
				}
				this.interactionService.ShowMessageBox("Success", "Goal was added successfully");
			});

			RemoveGoalCommand = new DelegateCommand<GoalDTO>((goal) =>
			{
				if (HomeGoals.Contains(goal))
				{
					HomeGoals.Remove(goal);
					Match.HomeTeamScore--;
				}
				else
				{
					AwayGoals.Remove(goal);
					Match.AwayTeamScore--;
				}
			});

			OkCommand = new DelegateCommand(async () => 
			{
				if (IsEditing)
				{

					IsEnabled = true;
					Match.HomeTeamGoals = HomeGoals.ToArray();
					Match.AwayTeamGoals = AwayGoals.ToArray();
					if (Match.HomeTeamScore == null)
					{
						Match.HomeTeamScore = 0;
					}
					if (Match.AwayTeamScore == null)
					{
						Match.AwayTeamScore = 0;
					}
					await this.refereeService.SaveGoalsAsync(Match);
					IsEnabled = false;
				}
				GlobalCommands.GoBackCommand.Execute(null);
			});
		}

		#region Geocoding
		
		#endregion
		
		#region Map Route
		
		private async void CalculateRoute()
		{
			var from = await GeocodeAddress(From);
			var to = await GeocodeAddress(To);

			if (from == null)
				return;

			if (DirectionsMap.Children.Count > 4)
				DirectionsMap.Children.RemoveAt(DirectionsMap.Children.Count - 1);
			
			var p = geocodingService.GetLocationFromStringQuery(this.From);
			
			DirectionsMap.Children.Add(new Pushpin { Location = p });

			CalculateRoute(from, to);
		}

		private async Task<GeocodeResult> GeocodeAddress(string address)
		{
			GeocodeResult result = null;

			using (GeocodeServiceClient client = new GeocodeServiceClient("CustomBinding_IGeocodeService"))
			{
				GeocodeRequest request = new GeocodeRequest();
				request.Credentials = new Credentials() { ApplicationId = Resources.BingApiKey };
				request.Query = address;

				try
				{
					result = client.Geocode(request).Results[0];
				}
				catch (Exception)
				{
					await this.interactionService.ShowMessageBox("Sorry", $"Could not find: {this.From}");
				}

			}

			return result;
		}

		private async void CalculateRoute(GeocodeResult from, GeocodeResult to)
		{
			using (RouteServiceClient client = new RouteServiceClient("CustomBinding_IRouteService"))
			{
				RouteRequest request = new RouteRequest
				{
					Credentials = new Credentials() { ApplicationId = Resources.BingApiKey },
					Waypoints = new ObservableCollection<Waypoint> { ConvertResultToWayPoint(@from), ConvertResultToWayPoint(to) },
					Options = new RouteOptions { RoutePathType = RoutePathType.Points },
				};

				try
				{
					RouteResult = client.CalculateRoute(request).Result;
				}
				catch (Exception)
				{
					await this.interactionService.ShowMessageBox("Sorry", $"Could not find: {this.From}");
					return;
				}
			}

			GetDirections();
		}

		private void GetDirections()
		{
			Directions = new ObservableCollection<Direction>();

			foreach (var item in RouteResult.Legs[0].Itinerary)
			{
				var direction = new Direction();
				direction.Description = GetDirectionText(item);
				direction.Location = new Location(item.Location.Latitude, item.Location.Longitude);
				Directions.Add(direction);
			}
		}

		private static string GetDirectionText(BingServices.ItineraryItem item)
		{
			string contentString = item.Text;
			//Remove tags from the string
			Regex regex = new Regex("<(.|\n)*?>");
			contentString = regex.Replace(contentString, string.Empty);
			return contentString;
		}

		private Waypoint ConvertResultToWayPoint(GeocodeResult result)
		{
			Waypoint waypoint = new Waypoint();
			waypoint.Description = result.DisplayName;
			waypoint.Location = result.Locations[0];
			return waypoint;
		}

		#endregion

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return false;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			this.FlyoutOpen = false;
		}

		public async void OnNavigatedTo(NavigationContext navigationContext)
		{
			var id = (int)navigationContext.Parameters["id"];
			this.IsEditing = (bool)navigationContext.Parameters["isEditing"];
			Match = await refereeService.GetMatchAsync(id);
			if(Match.HomeTeamScore == null && this.IsEditing)
			{
				Match.HomeTeamScore = 0;
				Match.AwayTeamScore = 0;
			}
			Teams.Add(new Tuple<int, string>(Match.HomeTeamId, Match.HomeTeamName));
			Teams.Add(new Tuple<int, string>(Match.AwayTeamId, Match.AwayTeamName));
			SelectedTeam = Teams.First();
			HomeGoals.AddRange(Match.HomeTeamGoals.OrderBy(x => x.Time));
			AwayGoals.AddRange(Match.AwayTeamGoals.OrderBy(x => x.Time));
			SelectedPlayer = Players.First();
			//if (Match.HomeTeamScore != null)
			//{
			//	IsEditing = false;
			//}

			// Center the map to stadium:
			var address = Match.Stadium.Address;
			this.To = $"{address.Street}, {address.Number}, {address.City}, {address.Zipcode}";

			var p = geocodingService.GetLocationFromStringQuery(this.To);
			DirectionsMap.Center = p;
			DirectionsMap.ZoomLevel = 14;
			DirectionsMap.Children.Add(new Pushpin { Location = p });

			IsEnabled = false;
		}
	}
}
