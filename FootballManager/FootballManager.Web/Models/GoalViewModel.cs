using FootballManager.BusinessLayer.Models;
using FootballManager.Domain.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class GoalViewModel
	{
		public int Id { get; set; }
		//public FootballerViewModel Footballer { get; set; }
		//public MatchViewModel Match { get; set; }
		//public TeamViewModel Team { get; set; }
		public ScorersViewModel Footballer { get; set; }
		public string HomeTeam { get; set; }
		public string AwayTeam { get; set; }
		public string Type { get; set; }
		public int Time { get; set; }
	}
}