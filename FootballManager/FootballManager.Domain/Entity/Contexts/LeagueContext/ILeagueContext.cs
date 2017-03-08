using System.Data.Entity;
using FootballManager.Domain.Entity.Models;

namespace FootballManager.Domain.Entity.Contexts.LeagueContext
{
    public interface ILeagueContext : IDbContext
    {
        IDbSet<Coach> Coaches { get; set; }
        IDbSet<Footballer> Footballers { get; set; }
        IDbSet<Referee> Referees { get; set; }

        IDbSet<Address> Addresses { get; set; }
        IDbSet<Stadium> Stadiums { get; set; }
        IDbSet<Team> Teams { get; set; }

        IDbSet<Goal> Goals { get; set; }
        IDbSet<Match> Matches { get; set; }
        IDbSet<Season> Seasons { get; set; }
        IDbSet<Table> Tables { get; set; }

		IDbSet<Comment> Comments { get; set; }
		IDbSet<Notification> Notifications { get; set; }
    }
}
