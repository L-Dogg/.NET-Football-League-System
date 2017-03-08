using FootballManager.Domain.Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class MatchViewModel 
	{
		public int Id { get; set; }
		public int HomeTeamId { get; set; }
		public string HomeTeamName { get; set; }
		public string HomeTeamLogoUrl { get; set; }
		public int AwayTeamId { get; set; }
		public string AwayTeamName { get; set; }
		public string AwayTeamLogoUrl { get; set; }
		public StadiumViewModel Stadium { get; set; }
		public string Referee;
		public int RefereeId;
		public DateTime Date { get; set; }
		public int? HomeGoalsCount { get; set; }
		public int? AwayGoalsCount { get; set; }
		public ICollection<GoalViewModel> HomeGoals { get; set; }
		public ICollection<GoalViewModel> AwayGoals { get; set; }
		public ICollection<Comment> Comments { get; set; }
		public int AverageRating { get; set; }
	}
}