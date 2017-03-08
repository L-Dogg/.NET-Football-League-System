using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FootballManager.Domain.Entity.Models;
using FootballManager.BusinessLayer.Models;

namespace FootballManager.BusinessLayer
{
	public interface ILeagueService : IRefereeLeagueService, IDisposable
	{
		Task<List<SelectItem>> GetFootballersList();
		Task<List<SelectItem>> GetTeamsList();
		Task<List<SelectItem>> GetCoachList();
		Task<List<SelectItem>> GetStadiumsList();
		Task<List<SelectItem>> GetSeasonsList();
		Task<List<SelectItem>> GetRefereeList();

		Task<List<SelectItem>> GetFilteredSeasonsList(string filter);
		Task<List<SelectItem>> GetFilteredFootballersList(string filter);
		Task<List<SelectItem>> GetFilteredRefereeList(string filter);
		Task<List<SelectItem>> GetFilteredCoachesList(string filter);
		Task<List<SelectItem>> GetFilteredStadiumsList(string filter);
		Task<List<SelectItem>> GetFilteredTeamsList(string filter);
		
		Task<bool> AddFootballer(Footballer player);
		Task AddTeam(Team team);
		Task AddReferee(Referee referee);
		Task AddCoach(Coach coach);
		Task AddMatch(Match match);
		Task AddSeason(Season season);
		Task AddGoal(Goal goal);
		Task AddAddress(Address address);
		Task AddStadium(Stadium stadium);

		Task<List<Match>> GetTeamsMatches(int teamId);

		Task<Team> GetTeam(int id);
		Task<Footballer> GetFootballer(int id);
		Task<Coach> GetCoach(int id);
		Task<Stadium> GetStadium(int id);
		Task<Season> GetSeason(int id);

		void Dispose();

		/// <summary>
		/// Removes player from team.
		/// </summary>
		/// <param name="player">Player which will be removed from his current team.</param>
		void RemovePlayerFromTeam(Footballer player);

		Task<bool> AssignTeamToSeason(int teamId, int seasonId);
		Task<bool> RemoveTeamFromSeason(int tableId);

		/// <summary>
		/// Verifies if player can be add to team specified in his TeamId field.
		/// </summary>
		/// <param name="player">Player to be verified.</param>
		/// <returns>True if player has no team or can be added to team, false otherwise.</returns>
		Task<bool> CanAddFootballer(Footballer player);

		/// <summary>
		/// Calculates and updates team salaries.
		/// </summary>
		/// <param name="team">Team.</param>
		void UpdateTeamSalaries(Team team, int changeValue);

		void SaveChanges();

		/// <summary>
		/// Saves all changes made to database entries since last SaveChanges.
		/// </summary>
		Task SaveChangesAsync();

		/// <summary>
		/// Generates fixtures for given season and saves it to that season.
		/// Uses Round Robin method with Fisher-Yates shuffle algorithm.
		/// </summary>
		/// <param name="seasonId">Id of season to generate fixtures for.</param>
		/// <param name="startDate">Day of first round.</param>
		/// <param name="interval">Interval between games (in days).</param>
		/// <param name="overwrite">Determines if matches existing for season would be overwritten with newly generated matches.</param>
		Task<bool> GenerateFixtures(int seasonId, DateTime startDate, int interval = 7, bool overwrite = false);

		List<Match> GetNextRound();
		List<Match> GetPreviousRound();
		Task<List<KeyValuePair<Footballer, int>>> GetScorersTable(int seasonId, int limit);
		Task SaveComment(Comment comment);
		Task<List<Notification>> GetUserNotifications(string fbId);
		Task AddNotification(Notification notification);
		Task RemoveNotification(Notification notification);
		Task<List<Notification>> GetNotificationsToSend();
	}

	public interface IRefereeLeagueService
	{
		Task<Referee> GetReferee(int id);
		Task<Referee> GetRefereeByUserId(int id);
		Task<Match> GetMatch(int id);
		Task UpdateMatch(Match match);
	}
}