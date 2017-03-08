using System;
using FootballManager.Domain.Entity.Models;
using FootballManager.BusinessLayer.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FootballManager.Domain.Entity.Contexts.LeagueContext;
using System.Threading.Tasks;
using FootballManager.AuthenticationLayer;

namespace FootballManager.BusinessLayer
{
	public class LeagueService : ILeagueService
	{
		private ILeagueContext leagueContext;
		private IAuthenticationService authenticationService;

		public LeagueService(ILeagueContext context, IAuthenticationService authenticationService)
		{
			this.leagueContext = context;
			this.authenticationService = authenticationService;
		}

		public void Dispose()
		{
			this.leagueContext.Dispose();
			this.authenticationService.Dispose();
		}

		#region Get Filtered SelectItem List
		/// <summary>
		/// Get Footballers whose First name or Surname matches filter.
		/// </summary>
		/// <returns>List of footballers matching criteria.</returns>
		public async Task<List<SelectItem>> GetFilteredFootballersList(string filter)
		{
			return await this.leagueContext.Footballers.Where(f => f.FirstName.Contains(filter) || f.Surname.Contains(filter)).
				OrderBy(f => f.Surname).ThenBy(f => f.FirstName).
				Select(f => new SelectItem() { Id = f.Id, Name = f.FirstName + " " + f.Surname }).ToListAsync();
		}

		/// <summary>
		/// Get Referees whose First name or Surname matches filter.
		/// </summary>
		/// <returns>List of referees matching criteria.</returns>
		public async Task<List<SelectItem>> GetFilteredRefereeList(string filter)
		{
			return await this.leagueContext.Referees.Where(f => f.FirstName.Contains(filter) || f.Surname.Contains(filter)).
				OrderBy(f => f.Surname).ThenBy(f => f.FirstName).
				Select(f => new SelectItem() { Id = f.Id, Name = f.FirstName + " " + f.Surname }).ToListAsync();
		}

		/// <summary>
		/// Get Coaches whose First name or Surname matches filter.
		/// </summary>
		/// <returns>List of coaches matching criteria.</returns>
		public async Task<List<SelectItem>> GetFilteredCoachesList(string filter)
		{
			return await this.leagueContext.Coaches.Where(f => f.FirstName.Contains(filter) || f.Surname.Contains(filter)).
				OrderBy(f => f.Surname).ThenBy(f => f.FirstName).
				Select(f => new SelectItem() { Id = f.Id, Name = f.FirstName + " " + f.Surname }).ToListAsync();
		}

		/// <summary>
		/// Get Stadiums which Name matches filter.
		/// </summary>
		/// <returns>List of stadiums matching criteria.</returns>
		public async Task<List<SelectItem>> GetFilteredStadiumsList(string filter)
		{
			return await this.leagueContext.Stadiums.Where(f => f.Name.Contains(filter)).
				OrderBy(f => f.Name).Select(f => new SelectItem() { Id = f.Id, Name = f.Name }).ToListAsync();
		}

		/// <summary>
		/// Get Seasons which Name matches filter.
		/// </summary>
		/// <returns>List of seasons matching criteria.</returns>
		public async Task<List<SelectItem>> GetFilteredSeasonsList(string filter)
		{
			return await this.leagueContext.Seasons.Where(f => f.Name.Contains(filter)).
				OrderBy(f => f.Name).Select(f => new SelectItem() { Id = f.Id, Name = f.Name }).ToListAsync();
		}

		/// <summary>
		/// Get Teams which Name matches filter.
		/// </summary>
		/// <returns>List of teams matching criteria.</returns>
		public async Task<List<SelectItem>> GetFilteredTeamsList(string filter)
		{
			return await this.leagueContext.Teams.Where(f => f.Name.Contains(filter)).
				OrderBy(f => f.Name).Select(f => new SelectItem() { Id = f.Id, Name = f.Name }).ToListAsync();
		}

		#endregion

		#region Get SelectItem List

		public async Task<List<SelectItem>> GetFootballersList()
		{
			return await this.leagueContext.Footballers.Select(footballer => new SelectItem() { Id = footballer.Id, Name = footballer.FirstName + " " + footballer.Surname }).ToListAsync();
		}

		public async Task<List<SelectItem>> GetTeamsList()
		{
			return await this.leagueContext.Teams.Select(team => new SelectItem() { Id = team.Id, Name = team.Name }).ToListAsync();
		}

		public async Task<List<SelectItem>> GetCoachList()
		{
			return await this.leagueContext.Coaches.Select(coach => new SelectItem() { Id = coach.Id, Name = coach.FirstName + " " + coach.Surname }).ToListAsync();
		}

		public async Task<List<SelectItem>> GetStadiumsList()
		{
			return await this.leagueContext.Stadiums.Select(stadium => new SelectItem() { Id = stadium.Id, Name = stadium.Name }).ToListAsync();
		}

		public async Task<List<SelectItem>> GetSeasonsList()
		{
			return await this.leagueContext.Seasons.Select(season => new SelectItem() { Id = season.Id, Name = season.Name }).ToListAsync();
		}

		public async Task<List<SelectItem>> GetRefereeList()
		{
			return await this.leagueContext.Referees.Select(referee => new SelectItem() { Id = referee.Id, Name = referee.FirstName + " " + referee.Surname }).ToListAsync();
		}

		#endregion

		#region Add Item

		/// <summary>
		/// Adds footballer to context and saves changes to database.
		/// </summary>
		/// <param name="player">Footballer to add to database</param>
		/// <returns></returns>
		public async Task<bool> AddFootballer(Footballer player)
		{
			if (player.TeamId != null)
			{
				if (!await CanAddFootballer(player))
					return false;

				this.leagueContext.Footballers.Add(player);
				this.UpdateTeamSalaries(player.Team, player.Salary);
				await this.leagueContext.SaveChangesAsync();
				return true;
			}
			this.leagueContext.Footballers.Add(player);
			await this.leagueContext.SaveChangesAsync();
			return true;
		}

		/// <summary>
		/// Adds team to context and saves changes to database.
		/// </summary>
		/// <param name="team">Team to add to database</param>
		/// <returns></returns>
		public async Task AddTeam(Team team)
		{
			this.leagueContext.Addresses.Add(team.Address);
			this.leagueContext.Teams.Add(team);
			await this.leagueContext.SaveChangesAsync();
		}

		/// <summary>
		/// Adds referee to context and saves changes to database.
		/// </summary>
		/// <param name="referee">Referee to add to database</param>
		/// <returns></returns>
		public async Task AddReferee(Referee referee)
		{
			int userId = await this.authenticationService.CreateRefereeAccount(referee.FirstName, referee.Surname);
			referee.RefereeUserId = userId;
			this.leagueContext.Referees.Add(referee);
			await this.leagueContext.SaveChangesAsync();
		}

		/// <summary>
		/// Adds coach to context and saves changes to database.
		/// </summary>
		/// <param name="coach">Coach to add to database</param>
		/// <returns></returns>
		public async Task AddCoach(Coach coach)
		{
			this.leagueContext.Coaches.Add(coach);
			await this.leagueContext.SaveChangesAsync();
		}

		/// <summary>
		/// Adds match to context and saves changes to database.
		/// </summary>
		/// <param name="match">Match to add to database</param>
		/// <returns></returns>
		public async Task AddMatch(Match match)
		{
			this.leagueContext.Matches.Add(match);
			await this.leagueContext.SaveChangesAsync();
		}

		/// <summary>
		/// Adds season to context and saves changes to database.
		/// </summary>
		/// <param name="season">Season to add to database</param>
		/// <returns></returns>
		public async Task AddSeason(Season season)
		{
			this.leagueContext.Seasons.Add(season);
			await this.leagueContext.SaveChangesAsync();
		}

		/// <summary>
		/// Adds goal to context and saves changes to database.
		/// </summary>
		/// <param name="goal">Goal to add to database</param>
		/// <returns></returns>
		public async Task AddGoal(Goal goal)
		{
			this.leagueContext.Goals.Add(goal);
			await this.leagueContext.SaveChangesAsync();
		}

		/// <summary>
		/// Adds address to context and saves changes to database.
		/// </summary>
		/// <param name="address">Address to add to databse</param>
		/// <returns></returns>
		public async Task AddAddress(Address address)
		{
			this.leagueContext.Addresses.Add(address);
			await this.leagueContext.SaveChangesAsync();
		}

		/// <summary>
		/// Adds stadium to context and saves changes to database.
		/// </summary>
		/// <param name="stadium">Stadium to add to database</param>
		/// <returns></returns>
		public async Task AddStadium(Stadium stadium)
		{
			this.leagueContext.Addresses.Add(stadium.Address);
			this.leagueContext.Stadiums.Add(stadium);
			await this.leagueContext.SaveChangesAsync();
		}
		#endregion

		#region Get Single Item

		/// <summary>
		/// Gets referee from database.
		/// </summary>
		/// <param name="id">Id of referee to find</param>
		/// <returns></returns>
		public async Task<Referee> GetReferee(int id)
		{
			var result = await this.leagueContext.Referees.SingleOrDefaultAsync(referee => referee.Id == id);
			if(result == null)
			{
				throw new Exception("Referee with given id doesn't exist");
			}
			return result;
		}

		/// <summary>
		/// Gets referee from database by userId.
		/// </summary>
		/// <param name="id">Id of referee's user id to find</param>
		/// <returns></returns>
		public async Task<Referee> GetRefereeByUserId(int id)
		{
			return await this.leagueContext.Referees.SingleOrDefaultAsync(referee => referee.RefereeUserId == id);
		}

		/// <summary>
		/// Gets team from database.
		/// </summary>
		/// <param name="id">Id of team to find</param>
		/// <returns></returns>
		public async Task<Team> GetTeam(int id)
		{
			var result = await this.leagueContext.Teams.Include(x => x.Address).SingleOrDefaultAsync(team => team.Id == id);
			if (result == null)
			{
				throw new Exception("Team with given id doesn't exist");
			}
			return result;
		}

		/// <summary>
		/// Gets footballer from database.
		/// </summary>
		/// <param name="id">Id of footballer to find</param>
		/// <returns></returns>
		public async Task<Footballer> GetFootballer(int id)
		{
			var result = await this.leagueContext.Footballers.SingleOrDefaultAsync(footballer => footballer.Id == id);
			if (result == null)
			{
				throw new Exception("Footballer with given id doesn't exist");
			}
			return result;
		}

		/// <summary>
		/// Gets coach from database.
		/// </summary>
		/// <param name="id">Id of coach to find</param>
		/// <returns></returns>
		public async Task<Coach> GetCoach(int id)
		{
			var result = await this.leagueContext.Coaches.SingleOrDefaultAsync(coach => coach.Id == id);
			if (result == null)
			{
				throw new Exception("Coach with given id doesn't exist");
			}
			return result;
		}

		/// <summary>
		/// Gets stadium from database.
		/// </summary>
		/// <param name="id">Id of stadium to find</param>
		/// <returns></returns>
		public async Task<Stadium> GetStadium(int id)
		{
			var result = await this.leagueContext.Stadiums.Include(x => x.Address).SingleOrDefaultAsync(stadium => stadium.Id == id);
			if (result == null)
			{
				throw new Exception("Stadium with given id doesn't exist");
			}
			return result;
		}

		/// <summary>
		/// Gets season from database.
		/// </summary>
		/// <param name="id">Id of season to find</param>
		/// <returns></returns>
		public async Task<Season> GetSeason(int id)
		{
			var result = await this.leagueContext.Seasons.Include(x => x.Table).SingleOrDefaultAsync(season => season.Id == id);
			if (result == null)
			{
				throw new Exception("Season with given id doesn't exist");
			}
			return result;
		}

		/// <summary>
		/// Gets match from database.
		/// </summary>
		/// <param name="id">Id of match to find</param>
		/// <returns></returns>
		public async Task<Match> GetMatch(int id)
		{
			var result = await this.leagueContext.Matches.SingleOrDefaultAsync(match => match.Id == id);
			if (result == null)
			{
				throw new Exception("Match with given id doesn't exist");
			}
			return result;
		}

		#endregion

		public async Task<List<Match>> GetTeamsMatches(int teamId)
		{
			return await this.leagueContext.Matches.Where(x => x.HomeTeamId == teamId || x.AwayTeamId == teamId).ToListAsync();
		}

		/// <summary>
		/// Removes player from team.
		/// </summary>
		/// <param name="player">Player which will be removed from his current team.</param>
		public void RemovePlayerFromTeam(Footballer player)
		{
			player.TeamId = null;
			this.UpdateTeamSalaries(player.Team, -player.Salary);
		}

		/// <summary>
		/// Adds selected team to selected season.
		/// </summary>
		/// <param name="teamId">Id of team to add</param>
		/// <param name="seasonId">Id of season to add team to</param>
		/// <returns></returns>
		public async Task<bool> AssignTeamToSeason(int teamId, int seasonId)
		{
			var team = await this.GetTeam(teamId);
			var season = await this.GetSeason(seasonId);
			if(await this.leagueContext.Tables.AnyAsync(x => x.TeamId == teamId && x.SeasonId == seasonId))
			{
				return false;
			}

			this.leagueContext.Tables.Add(new Table() { TeamId = teamId, SeasonId = seasonId });
			await this.leagueContext.SaveChangesAsync();
			return true;
		}

		/// <summary>
		/// Removes selected team from season, by removing it from season's table.
		/// </summary>
		/// <param name="tableId">Id of table entry to remove</param>
		/// <returns></returns>
		public async Task<bool> RemoveTeamFromSeason(int tableId)
		{
			var table = await this.leagueContext.Tables.SingleAsync(x => x.Id == tableId);
			if(await this.leagueContext.Matches.AnyAsync(x => x.SeasonId == table.SeasonId && (x.AwayTeamId == table.TeamId || x.HomeTeamId == table.TeamId)))
			{
				return false;
			}
			this.leagueContext.Tables.Remove(table);
			return true;
		}

		/// <summary>
		/// Verifies if player can be add to team specified in his TeamId field.
		/// </summary>
		/// <param name="player">Player to be verified.</param>
		/// <returns>True if player has no team or can be added to team, false otherwise.</returns>
	    public async Task<bool> CanAddFootballer(Footballer player)
	    {
			var team = await this.leagueContext.Teams.SingleOrDefaultAsync(x => x.Id == player.TeamId);

			if (team == null)
			    return true;

		    if (team.Salaries + player.Salary > team.Budget)
				return false;

		    await this.leagueContext.SaveChangesAsync();
		    return true;
	    }

		/// <summary>
		/// Calculates and updates team salaries.
		/// </summary>
		/// <param name="team">Team.</param>
		public void UpdateTeamSalaries(Team team, int changeValue)
	    {
			if (team == null)
				return;

			team.Salaries += changeValue;
	    }

		/// <summary>
		/// Saves all changes made to database entries since last SaveChanges synchronously.
		/// </summary>
		public void SaveChanges()
		{
			this.leagueContext.SaveChanges();
		}

		/// <summary>
		/// Saves all changes made to database entries since last SaveChanges asynchronously.
		/// </summary>
		public async Task SaveChangesAsync()
        {
			await this.leagueContext.SaveChangesAsync();
        }

		/// <summary>
		/// Generates fixtures for given season and saves it to that season.
		/// Uses Round Robin method with Fisher-Yates shuffle algorithm.
		/// </summary>
		/// <param name="seasonId">Id of season to generate fixtures for.</param>
		/// <param name="startDate">Day of first round.</param>
		/// <param name="interval">Interval between games (in days).</param>
		/// <param name="overwrite">Determines if matches existing for season would be overwritten with newly generated matches.</param>
		public async Task<bool> GenerateFixtures(int seasonId, DateTime startDate, int interval = 7, bool overwrite = false)
	    {
			var matches = await this.leagueContext.Matches.Where(match => match.SeasonId == seasonId).ToListAsync();
			if (!overwrite && matches.Any())
			{
				return false;
			}
		    if (overwrite)
		    {
			    foreach (var match in matches)
			    {
				    var goals = this.leagueContext.Goals.Where(goal => goal.MatchId == match.Id);
				    foreach (var goal in goals)
				    {
					    this.leagueContext.Goals.Remove(goal);
				    }
				    this.leagueContext.Matches.Remove(match);
			    }
				foreach(var table in await this.leagueContext.Tables.Where(table => table.SeasonId == seasonId).ToListAsync())
				{
					table.GoalsConceded = 0;
					table.GoalsScored = 0;
					table.Points = 0;
					table.Position = 1;
				}
		    }

		    var ListTeam = this.ShuffleTeams(seasonId);
		    var count = ListTeam.Count;
			int numDays = (count - 1);
			int halfSize = count / 2;
			var referees = await this.GetRefereeList();
			var date = startDate;
			var secondRoundDate = startDate.AddDays(interval * (halfSize + 1));
			if (referees.Count == 0)
			{
				return false;
			}
			int refereeIdx = 0;
			List<Team> teams = new List<Team>();

			teams.AddRange(ListTeam.Skip(halfSize).Take(halfSize));
			teams.AddRange(ListTeam.Skip(1).Take(halfSize - 1).ToArray().Reverse());

			int teamsSize = teams.Count;

			for (int day = 0; day < numDays; day++)
			{
				int teamIdx = day % teamsSize;
				var match = new Match()
				{
					AwayTeamId = ListTeam[0].Id,
					Date = date,
					HomeTeamId = teams[teamIdx].Id,
					StadiumId = teams[teamIdx].Stadium.Id,
					SeasonId = seasonId,
					RefereeId = referees[(refereeIdx++) % referees.Count].Id,
					Round = day + 1
				};

				this.leagueContext.Matches.Add(match);

				match = new Match()
				{
					AwayTeamId = teams[teamIdx].Id,
					Date = secondRoundDate,
					HomeTeamId = ListTeam[0].Id,
					StadiumId = ListTeam[0].Stadium.Id,
					SeasonId = seasonId,
					RefereeId = referees[(refereeIdx++) % referees.Count].Id,
					Round = numDays + 1 + day
				};
				this.leagueContext.Matches.Add(match);

				for (int idx = 1; idx < halfSize; idx++)
				{
					int firstTeam = (day + idx) % teamsSize;
					int secondTeam = (day + teamsSize - idx) % teamsSize;
					match = new Match()
					{
						AwayTeamId = teams[firstTeam].Id,
						Date = date,
						HomeTeamId = teams[secondTeam].Id,
						StadiumId = teams[secondTeam].Stadium.Id,
						SeasonId = seasonId,
						RefereeId = referees[(refereeIdx++) % referees.Count].Id,
						Round = day + 1
					};

					this.leagueContext.Matches.Add(match);
					this.leagueContext.Matches.Add(new Match()
					{
						AwayTeamId = teams[secondTeam].Id,
						Date = secondRoundDate,
						HomeTeamId = teams[firstTeam].Id,
						StadiumId = teams[firstTeam].Stadium.Id,
						SeasonId = seasonId,
						RefereeId = referees[(refereeIdx++) % referees.Count].Id,
						Round = numDays + 1 + day
					});
				}
				date = date.AddDays(interval);
				secondRoundDate = secondRoundDate.AddDays(interval);
			}

			await SaveChangesAsync();
			return true;
	    }

		public async Task UpdateMatch(Match match)
		{
			await this.RemoveGoalsFromMatch(match.Id);
			var matchObject = await this.GetMatch(match.Id);
			var homeTable = await this.leagueContext.Tables.SingleAsync(x => x.SeasonId == matchObject.SeasonId && x.TeamId == matchObject.HomeTeamId);
			var awayTable = await this.leagueContext.Tables.SingleAsync(x => x.SeasonId == matchObject.SeasonId && x.TeamId == matchObject.AwayTeamId);
			if(matchObject.HomeGoals != null)
			{
				homeTable.GoalsScored -= matchObject.HomeGoals.Value;
				homeTable.GoalsConceded -= matchObject.AwayGoals.Value;
				awayTable.GoalsConceded -= matchObject.HomeGoals.Value;
				awayTable.GoalsScored -= matchObject.AwayGoals.Value;
				this.AddPoints(matchObject, homeTable, awayTable, -3, -1);
			}
			matchObject.Attendance = match.Attendance;
			matchObject.HomeGoals = match.HomeGoals;
			matchObject.AwayGoals = match.AwayGoals;
			foreach(var goal in match.Goals)
			{
				await this.AddGoal(new Goal
				{
					FootballerId = goal.FootballerId,
					MatchId = match.Id,
					TeamID = goal.TeamID,
					Type = goal.Type,
					Time = goal.Time
				});
			}
			homeTable.GoalsScored += match.HomeGoals.Value;
			homeTable.GoalsConceded += match.AwayGoals.Value;
			awayTable.GoalsConceded += match.HomeGoals.Value;
			awayTable.GoalsScored += match.AwayGoals.Value;
			this.AddPoints(match, homeTable, awayTable, 3, 1);
			await this.SaveChangesAsync();
		}

		public List<Match> GetNextRound()
		{
			var group = this.leagueContext.Matches.Where(x => x.Date >= DateTime.Now).GroupBy(x => x.Round).OrderBy(x => x.Key).FirstOrDefault();
			return group?.ToList() ?? new List<Match>();
		}

		public List<Match> GetPreviousRound()
		{
			var group = this.leagueContext.Matches.Where(x => x.Date < DateTime.Now).GroupBy(x => x.Round).OrderByDescending(x => x.Key).FirstOrDefault();
			return group?.ToList() ?? new List<Match>();
		}

		public async Task<List<KeyValuePair<Footballer, int>>> GetScorersTable(int seasonId, int limit)
		{
			var goals = await this.leagueContext.Goals.Where(x => x.Match.SeasonId == seasonId && x.Type != Domain.Entity.Enums.GoalType.Own).ToListAsync();
			var groupedGoals = goals.GroupBy(x => x.Footballer);
			return groupedGoals.Select(x => new KeyValuePair<Footballer, int>(x.Key, x.Count())).OrderByDescending(x => x.Value).Take(limit).ToList();
		}

		public async Task SaveComment(Comment comment)
		{
			this.leagueContext.Comments.Add(comment);
			await this.SaveChangesAsync();
		}

		public async Task<List<Notification>> GetUserNotifications(string fbId)
		{
			var notifications = await this.leagueContext.Notifications.Where(x => string.Equals(x.FacebookId, fbId)).ToListAsync();
			return notifications;
		}

		public async Task AddNotification(Notification notification)
		{
			this.leagueContext.Notifications.Add(notification);
			await this.SaveChangesAsync();
		}

		public async Task RemoveNotification(Notification notification)
		{
			var obj = this.leagueContext.Notifications.Single(x => x.Id == notification.Id);
			this.leagueContext.Notifications.Remove(obj);
			await this.SaveChangesAsync();
		}

		public async Task<List<Notification>> GetNotificationsToSend()
		{
			DateTime nextDay = DateTime.Today.AddDays(1);
			var matches = await this.leagueContext.Matches.Where(x => (x.Date.Day == nextDay.Date.Day && x.Date.Month == nextDay.Month && x.Date.Year == nextDay.Year)).ToListAsync();
			List<Notification> notifications = new List<Notification>();
			foreach(var notification in this.leagueContext.Notifications)
			{
				if(matches.Any(x => x.HomeTeamId == notification.TeamId || x.AwayTeamId == notification.TeamId))
				{
					notifications.Add(notification);
				}
			}
			return notifications;
		}

		/// <summary>
		/// Removes all goals with given parameter as MatchId.
		/// </summary>
		/// <param name="matchId">Id of match to remove goals from</param>
		private async Task RemoveGoalsFromMatch(int matchId)
		{
			var goals = await this.leagueContext.Goals.Where(x => x.MatchId == matchId).ToListAsync();
			foreach(var goal in goals)
			{
				this.leagueContext.Goals.Remove(goal);
			}
			await this.SaveChangesAsync();
		}

		/// <summary>
		/// Adds or removes points from tables according to match result
		/// </summary>
		/// <param name="match">Match object with result</param>
		/// <param name="homeTable">Table of home team</param>
		/// <param name="awayTable">Table of away team</param>
		/// <param name="winPoints">Points to add for winning team</param>
		/// <param name="drawPoints">Points to add in case of draw</param>
		private void AddPoints(Match match, Table homeTable, Table awayTable, int winPoints, int drawPoints)
		{
			if (match.AwayGoals > match.HomeGoals)
			{
				awayTable.Points += winPoints;
			}
			else
			{
				if (match.HomeGoals > match.AwayGoals)
				{
					homeTable.Points += winPoints;
				}
				else
				{
					awayTable.Points+= drawPoints;
					homeTable.Points+= drawPoints;
				}
			}
		}


		/// <summary>
		/// Shuffles teams using Fisher-Yates algorithm.
		/// </summary>
		/// <returns>Shuffled array of teams.</returns>
		/// <param name="seasonId">Id of season to shuffle teams belonging to this season for</param>
	    private IList<Team> ShuffleTeams(int seasonId)
	    {
			var teams = this.leagueContext.Tables.Where(table => table.SeasonId == seasonId).Select(x => x.Team).ToArray();
			var random = new Random();

		    for (var i = teams.Length; i > 1; i--)
		    {
			    var j = random.Next(i);
			    var tmp = teams[j];
			    teams[j] = teams[i - 1];
			    teams[i - 1] = tmp;
		    }

		    return teams;
	    }
    }
}
