using FootballManager.Domain.Entity.Enums;

namespace FootballManager.Domain.Entity.Models
{
	public class Goal : Entity
	{
		public int FootballerId { get; set; }
		public virtual Footballer Footballer { get; set; }

		public int? MatchId { get; set; }
		public virtual Match Match { get; set; }

		public int TeamID { get; set; }
		public virtual Team Team { get; set; }
		
		public GoalType Type { get; set; }
		public int Time { get; set; }
	}
}
