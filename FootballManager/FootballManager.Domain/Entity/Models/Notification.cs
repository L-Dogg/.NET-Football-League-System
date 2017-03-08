using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Domain.Entity.Models
{
	public class Notification : Entity
	{
		public string FacebookId { get; set; }
		public int TeamId { get; set; }

		public virtual Team Team { get; set; }
	}
}
