using System;
using System.Collections.Generic;

namespace FootballManager.Domain.Entity.Models
{
	public class Referee : Entity
	{
		public Referee()
		{
			this.BirthDate = DateTime.Today;
		}
		public string FirstName { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }
		public string PictureUrl { get; set; }
		public int RefereeUserId { get; set; }

		public virtual ICollection<Match> Matches { get; set; }
	}
}
