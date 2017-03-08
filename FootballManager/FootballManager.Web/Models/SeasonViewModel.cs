using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class SeasonViewModel
	{
		public int Id { get; set; }
		public ICollection<TableViewModel> Tables { get; set; }
		public List<List<MatchViewModel>> Schedule { get; set; }
		public List<KeyValuePair<ScorersViewModel, int>> ScorersTable { get; set; }
	}
}