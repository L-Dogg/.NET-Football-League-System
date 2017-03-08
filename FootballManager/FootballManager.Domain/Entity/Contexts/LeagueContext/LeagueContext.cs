using System.Data.Entity;
using FootballManager.Domain.Entity.Models;

namespace FootballManager.Domain.Entity.Contexts.LeagueContext
{
	public class LeagueContext : DbContext, ILeagueContext
	{
        public LeagueContext() : base()
        {
           Database.SetInitializer(new LeagueContextInitializer());
        }

        public virtual IDbSet<Coach> Coaches { get; set; }
		public virtual IDbSet<Footballer> Footballers { get; set; }
		public virtual IDbSet<Referee> Referees { get; set; }

		public virtual IDbSet<Address> Addresses { get; set; }
		public virtual IDbSet<Stadium> Stadiums { get; set; }
		public virtual IDbSet<Team> Teams { get; set; }

		public virtual IDbSet<Goal> Goals { get; set; }
		public virtual IDbSet<Match> Matches { get; set; }
		public virtual IDbSet<Season> Seasons { get; set; }
		public virtual IDbSet<Table> Tables { get; set; }

		public virtual IDbSet<Comment> Comments { get; set; }
		public virtual IDbSet<Notification> Notifications { get; set; }

	}
}
