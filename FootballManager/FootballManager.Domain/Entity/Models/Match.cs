using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballManager.Domain.Entity.Models
{
	public class Match : Entity
	{
        public int? HomeTeamId { get; set; }
        [ForeignKey("HomeTeamId")]
        public virtual Team HomeTeam { get; set; }

        public int? AwayTeamId { get; set; }
        [ForeignKey("AwayTeamId")]
        public virtual Team AwayTeam { get; set; }

		public int RefereeId { get; set; }
		public virtual Referee Referee { get; set; }

		public int StadiumId { get; set; }
		public virtual Stadium Stadium { get; set; }

		public int SeasonId { get; set; }
		public virtual Season Season { get; set; }

		public DateTime Date { get; set; }
		public int Round { get; set; }

		public int Attendance { get; set; }

		public int? HomeGoals { get; set; }
		public int? AwayGoals { get; set; }
		public virtual ICollection<Goal> Goals { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
	}
}
