using System;
using System.Collections.Generic;

namespace FootballManager.Domain.Entity.Models
{
	public class Coach : Entity
	{
		
		public Coach()
		{
			this.BirthDate = DateTime.Today;
		}

		public string FirstName { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }
		public string PictureUrl { get; set; }

		public virtual ICollection<Team> Teams { get; set; }
	}

}
