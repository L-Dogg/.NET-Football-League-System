using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManager.BusinessLayer;
using FootballManager.BusinessLayer.Models;

namespace FootballManager.Presentation.Infrastructure
{
	/// <summary>
	/// Filter types.
	/// </summary>
	public enum Filter
	{
		Footballer,
		Referee,
		Coach,
		Team,
		Season,
		Stadium
	}

	/// <summary>
	/// "Proxy" class containing filter definitions, which can be later accessed via GetFilter method.
	/// </summary>
	public static class FilterDictionary
	{
		private static Dictionary<Filter, Func<string, ILeagueService, Task<List<SelectItem>>>> _dictionary = 
			new Dictionary<Filter, Func<string, ILeagueService, Task<List<SelectItem>>>>();
		
		/// <summary>
		/// Static class initializer populating filter dictionary with basic filters from viewmodel.
		/// </summary>
		static FilterDictionary()
		{
			_dictionary.Add(Filter.Footballer, async (s, model) => await model.GetFilteredFootballersList(s));
			_dictionary.Add(Filter.Referee, async (s, model) => await model.GetFilteredRefereeList(s));
			_dictionary.Add(Filter.Coach, async (s, model) => await model.GetFilteredCoachesList(s));
			_dictionary.Add(Filter.Team, async (s, model) => await model.GetFilteredTeamsList(s));
			_dictionary.Add(Filter.Season, async (s, model) => await model.GetFilteredSeasonsList(s));
			_dictionary.Add(Filter.Stadium, async (s, model) => await model.GetFilteredStadiumsList(s));
		}

		/// <summary>
		/// "Indexer" for filter dictionary.
		/// </summary>
		/// <param name="filter">Filter type.</param>
		/// <returns>Filter function or null if dictionary does not contain this filter type.</returns>
		public static Func<string, ILeagueService, Task<List<SelectItem>>> GetFilter(Filter filter)
		{
			Func<string, ILeagueService, Task<List<SelectItem>>> f;
			if (_dictionary.TryGetValue(filter, out f))
				return f;
			else
				return null;
		}
	}
}
