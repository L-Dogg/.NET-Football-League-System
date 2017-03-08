using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using FluentAssertions;
using FootballManager.Domain.Entity.Contexts.LeagueContext;
using FootballManager.Domain.Entity.Enums;

namespace FootballManager.Domain.UnitTests
{
	/// <summary>
	/// Tests Update operations on League Context.
	/// </summary>
	[TestClass]
	public class UpdateTest
	{
		private ILeagueContext context;
		[TestInitialize]
		public void Initialize()
		{
			this.context = new LeagueContext();
		}

		[TestCleanup]
		public void Cleanup()
		{
			this.context.Dispose();
		}

		[TestMethod]
		public void UpdateAddress()
		{
			Action act = () =>
			{
				var address = context.Addresses.First();
				var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString();
				address.City = timestamp;
				context.SaveChanges();

				context.Addresses.First().City.Should().Be(timestamp);
			};

			act.ShouldNotThrow();
		}

		[TestMethod]
		public void UpdateCoach()
		{
			Action act = () =>
			{
				var coach = context.Coaches.First();
				var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString();
				coach.FirstName = timestamp;
				context.SaveChanges();

				context.Coaches.First().FirstName.Should().Be(timestamp);
			};

			act.ShouldNotThrow();
		}

		[TestMethod]
		public void UpdateFootballer()
		{
			Action act = () =>
			{
				var footballer = context.Footballers.First();
				var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString();
				footballer.FirstName = timestamp;
				context.SaveChanges();

				context.Footballers.First().FirstName.Should().Be(timestamp);
			};

			act.ShouldNotThrow();
		}

		[TestMethod]
		public void UpdateGoal()
		{
			Action act = () =>
			{
				var goal = context.Goals.First();
				GoalType myType;
				if (goal.Type == GoalType.Freekick || goal.Type == GoalType.Penalty)
					myType = GoalType.Normal;
				else
					myType = GoalType.Penalty;

				goal.Type = myType;
				context.SaveChanges();

				context.Goals.First().Type.Should().Be(myType);
			};

			act.ShouldNotThrow();
		}

		[TestMethod]
		public void UpdateMatch()
		{
			Action act = () =>
			{
				var match = context.Matches.First();
				var myDate = match.Date.AddDays(1);
				match.Date = myDate;
				context.SaveChanges();

				context.Matches.First().Date.Should().Be(myDate);
			};

			act.ShouldNotThrow();
		}

		[TestMethod]
		public void UpdateReferee()
		{
			Action act = () =>
			{
				var referee = context.Referees.First();
				var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString();
				referee.FirstName = timestamp;
				context.SaveChanges();

				context.Referees.First().FirstName.Should().Be(timestamp);
			};

			act.ShouldNotThrow();
		}

		[TestMethod]
		public void UpdateSeason()
		{
			Action act = () =>
			{
				var season = context.Seasons.First();
				var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString();
				season.Name = timestamp;
				context.SaveChanges();

				context.Seasons.First().Name.Should().Be(timestamp);
			};

			act.ShouldNotThrow();
		}

		[TestMethod]
		public void UpdateStadium()
		{
			Action act = () =>
			{
				var stadium = context.Stadiums.First();
				var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString();
				stadium.Name = timestamp;
				context.SaveChanges();

				context.Stadiums.First().Name.Should().Be(timestamp);
			};

			act.ShouldNotThrow();
		}

		[TestMethod]
		public void UpdateTeam()
		{
			Action act = () =>
			{
				var team = context.Teams.First();
				var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString();
				team.Name = timestamp;
				context.SaveChanges();

				context.Teams.First().Name.Should().Be(timestamp);
			};

			act.ShouldNotThrow();
		}
	}
}
