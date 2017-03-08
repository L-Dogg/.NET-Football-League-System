using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballManager.Domain.Entity.Models
{
	[Table("Stadiums")]
	public class Stadium : Entity
	{
		public Stadium()
		{
		}

		public string Name { get; set; }
		public int Capacity { get; set; }
		public string PictureUrl { get; set; }

		public int? AddressId { get; set; }
		public virtual Address Address { get; set; }
		public virtual ICollection<Match> Matches { get; set; }
        public virtual ICollection<Team> Teams { get; set; } 
	}
}
