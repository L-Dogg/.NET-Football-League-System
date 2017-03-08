using System;
using System.Collections.Generic;

namespace FootballManager.Domain.Entity.Models
{
	public class Team : Entity
	{
		public Team()
		{
			this.Founded = DateTime.Today;
		}

		public string Name { get; set; }
		public DateTime Founded { get; set; }
        public int Budget { get; set; }
        public int Salaries { get; set; }
		public string LogoUrl { get; set; }

		public int AddressId { get; set; }
		public virtual Address Address { get; set; }
		
        public int? StadiumId { get; set; }
        public virtual Stadium Stadium { get; set; }

		public int CoachId { get; set; }
		public virtual Coach Coach { get; set; }

		public virtual ICollection<Footballer> Footballers { get; set; }
		public virtual ICollection<Match> Matches { get; set; }
		public virtual ICollection<Table> Table { get; set; }
	}
}
