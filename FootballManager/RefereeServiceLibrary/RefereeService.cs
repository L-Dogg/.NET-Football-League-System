using FootballManager.AuthenticationLayer;
using FootballManager.BusinessLayer;
using FootballManager.Domain.Entity.Models;
using FootballManager.Domain.Entity.Models.Authentication.Enums;
using RefereeServiceLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RefereeServiceLibrary
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
	public class RefereeService : IRefereeService
	{
		private readonly IRefereeLeagueService leagueService;
		private readonly IAuthenticationService authenticationService;

		public RefereeService()
		{

		}

		public RefereeService(IRefereeLeagueService leagueService, IAuthenticationService authenticationService)
		{
			this.leagueService = leagueService;
			this.authenticationService = authenticationService;
		}

		public int AuthenticateReferee(string username, string password, UserType userType = UserType.Referee)
		{
			return this.authenticationService.AuthenticateUser(username, password, userType);
		}

		public bool ChangePassword(int userID, string oldPassword, string newPassword)
		{
			return this.authenticationService.ChangePassword(userID, oldPassword, newPassword);
		}

		public PlayerListItem GetPlayer()
		{
			return new PlayerListItem() { FirstName = "Andrzej" };
		}

		public async Task<List<MatchListItem>> GetMatchesList(int refereeId)
		{
			return (await this.leagueService.GetRefereeByUserId(refereeId)).Matches.Select(x => new MatchListItem()
			{
				Id = x.Id,
				Date = x.Date,
				HomeTeamName = x.HomeTeam.Name,
				AwayTeamName = x.AwayTeam.Name,
				HomeTeamGoals = x.HomeGoals,
				AwayTeamGoals = x.AwayGoals
			}).OrderBy(x => x.Date).ToList();
		}

		public async Task<MatchDTO> GetMatch(int id)
		{
			var match = await this.leagueService.GetMatch(id);
			return new MatchDTO()
			{
				Id = match.Id,
				HomeTeamName = match.HomeTeam.Name,
				AwayTeamName = match.AwayTeam.Name,
				Date = match.Date,
				Referee = new RefereeDTO
				{
					Id = match.RefereeId,
					FirstName = match.Referee.FirstName,
					LastName = match.Referee.Surname
				},
				HomeTeamId = match.HomeTeamId.Value,
				AwayTeamId = match.AwayTeamId.Value,
				Stadium = new StadiumDTO
				{
					Id = match.StadiumId,
					Name = match.Stadium.Name,
					Address = new AddressDTO()
					{
						City = match.Stadium.Address.City,
						Street = match.Stadium.Address.Street,
						Number = match.Stadium.Address.Number,
						Zipcode = match.Stadium.Address.Zipcode,
					}
				},
				HomeTeamScore = match.HomeGoals,
				AwayTeamScore = match.AwayGoals,
				HomeTeamGoals = match.Goals.Where(x => x.TeamID == match.HomeTeamId).Select(goal => new GoalDTO
				{
					Id = goal.Id,
					Scorer = new PlayerListItem
					{
						Id = goal.FootballerId,
						FirstName = goal.Footballer.FirstName,
						LastName = goal.Footballer.Surname
					},
					GoalType = goal.Type,
					TeamId = goal.TeamID,
					Time = goal.Time
				}).ToList(),
				AwayTeamGoals = match.Goals.Where(x => x.TeamID == match.AwayTeamId).Select(goal => new GoalDTO
				{
					Id = goal.Id,
					Scorer = new PlayerListItem
					{
						Id = goal.FootballerId,
						FirstName = goal.Footballer.FirstName,
						LastName = goal.Footballer.Surname
					},
					GoalType = goal.Type,
					TeamId = goal.TeamID,
					Time = goal.Time
				}).ToList(),
				HomeTeamPlayers = match.HomeTeam.Footballers.Select(player => new PlayerListItem
				{
					Id = player.Id,
					FirstName = player.FirstName,
					LastName = player.Surname
				}).ToList(),
				AwayTeamPlayers = match.AwayTeam.Footballers.Select(player => new PlayerListItem
				{
					Id = player.Id,
					FirstName = player.FirstName,
					LastName = player.Surname
				}).ToList(),
				Attendance = match.Attendance
			};
		}

		public async Task SaveGoals(MatchDTO match)
		{
			match.HomeTeamGoals.AddRange(match.AwayTeamGoals);
			var matchObject = new Match()
			{
				Id = match.Id,
				Attendance = match.Attendance,
				Date = match.Date,
				HomeGoals = match.HomeTeamScore,
				AwayGoals = match.AwayTeamScore,
				Goals = match.HomeTeamGoals.Select(x => new Goal
				{
					FootballerId = x.Scorer.Id,
					MatchId = match.Id,
					TeamID = x.TeamId,
					Type = x.GoalType,
					Time = x.Time
				}).ToList(),
				RefereeId = match.Referee.Id,
			};
			await this.leagueService.UpdateMatch(matchObject);
		}
	}
}
