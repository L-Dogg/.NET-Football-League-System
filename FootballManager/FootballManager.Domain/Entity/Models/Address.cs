using System.Collections.Generic;

namespace FootballManager.Domain.Entity.Models
{
	public class Address : Entity
	{
		public string City { get; set; }
		public string Zipcode { get; set; }
		public string Street { get; set; }
		public string Number { get; set; }

		public virtual ICollection<Team> Team { get; set; }
		public virtual ICollection<Stadium> Stadium { get; set; }
	}
}
