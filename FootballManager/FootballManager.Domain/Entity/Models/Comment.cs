using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Domain.Entity.Models
{
	public class Comment : Entity
	{
		public int MatchId { get; set; }
		public string Username { get; set; }
		public int? UserId { get; set; }
		public string FacebookId { get; set; }
		public DateTime Date { get; set; }
		public int Rating { get; set; }
		public string Text { get; set; }
	}
}
