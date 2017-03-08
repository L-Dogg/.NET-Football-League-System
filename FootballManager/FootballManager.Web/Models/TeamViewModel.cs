using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class TeamViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Founded { get; set; }
		public string LogoUrl { get; set; }
		public AddressViewModel Address { get; set; }
		public KeyValuePair<int,string> Stadium { get; set; }
		public KeyValuePair<int, string> Coach { get; set; }
		public List<TeamFootballerViewModel> Players { get; set; }
		public List<KeyValuePair<int, string>> Seasons { get; set; }
		public ICollection<MatchViewModel> Matches { get; set; }
	}
}