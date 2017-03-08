using System;
using System.Collections.Generic;

namespace FootballManager.Domain.Entity.Models
{
	public class Footballer : Entity
	{
		public Footballer()
		{
			this.BirthDate = DateTime.Today;
		}

		public string FirstName { get; set; }
		public string Surname { get; set; }
        public int Salary { get; set; }
		public string PictureUrl { get; set; }

		public int? TeamId { get; set; }
		public virtual Team Team { get; set; }

		public DateTime BirthDate { get; set; }

		public virtual ICollection<Goal> Goals { get; set; }
	}
}
