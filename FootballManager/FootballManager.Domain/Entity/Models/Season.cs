using System.Collections.Generic;

namespace FootballManager.Domain.Entity.Models
{
	public class Season : Entity
	{
		public string Name { get; set; }

		public virtual ICollection<Match> Matches { get; set; }
		public virtual ICollection<Table> Table { get; set; }
	}
}
