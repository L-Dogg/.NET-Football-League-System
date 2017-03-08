namespace FootballManager.Domain.Entity.Models
{
	public class Table : Entity
	{
		public int TeamId { get; set; }
		public virtual Team Team { get; set; }

		public int? SeasonId { get; set; }
		public virtual Season Season { get; set; }

		public int Position { get; set; }
		public int Points { get; set; }
		public int GoalsScored { get; set; }
		public int GoalsConceded { get; set; }
	}
}
