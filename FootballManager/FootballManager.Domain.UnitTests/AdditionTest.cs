using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FootballManager.Domain.Entity.Contexts.LeagueContext;
using FootballManager.Domain.Entity.Enums;
using FootballManager.Domain.Entity.Models;

namespace FootballManager.Domain.UnitTests
{
	/// <summary>
	/// Tests Add operations on League Context.
	/// </summary>
	[TestClass]
    public class AdditionTest
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

		#region Add Person Methods
		private void AddCoach()
		{
			var previousCount = context.Coaches.Count();
			Action act = () =>
			{
				context.Coaches.Add(new Coach() {BirthDate = DateTime.Today, FirstName = "Jacek", Surname = "Gmoch"});
				context.SaveChanges();
			};
			act.ShouldNotThrow();
			context.Coaches.Count().Should().Be(previousCount + 1);
		}
		
		private void AddFootballer()
		{
			var previousCount = context.Footballers.Count();
			Action act = () => {
				context.Footballers.Add(new Footballer() {BirthDate = DateTime.Today, FirstName = "Jacek", Surname = "Krzynówek"});
				context.SaveChanges();
			};
			act.ShouldNotThrow();
			context.Footballers.Count().Should().Be(previousCount + 1);
		}

		private void AddReferee()
		{
			var previousCount = context.Referees.Count();
			Action act = () =>
			{
				context.Referees.Add(new Referee() {BirthDate = DateTime.Today, FirstName = "Szymon", Surname = "Marciniak"});
				context.SaveChanges();
			};
			act.ShouldNotThrow();
			context.Referees.Count().Should().Be(previousCount + 1);
		}
		#endregion
		
		[TestMethod]
        public void AddAddress()
		{
			var previousCount = context.Addresses.Count();
			Action act = () =>
			{
				context.Addresses.Add(new Address() {City = "Warsaw", Number = "45C", Street = "Pruszkowska", Zipcode = "02-101"});
				context.SaveChanges();
			};
			act.ShouldNotThrow();
			context.Addresses.Count().Should().Be(previousCount + 1);
		}

		/// <summary>
		/// Tests all available person addition methods:
		/// AddCoach, AddFootballer, AddReferee()
		/// </summary>
	    [TestMethod]
	    public void AddPerson()
	    {
		    AddCoach();
			AddFootballer();
			AddReferee();
	    }

		[TestMethod]
	    public void AddSeason()
	    {
		    var previousCount = context.Seasons.Count();
		    Action act = () =>
		    {
			    context.Seasons.Add(new Season() {Name = "2016/2017" });
			    context.SaveChanges();
		    };
			act.ShouldNotThrow();
		    context.Seasons.Count().Should().Be(previousCount + 1);
	    }

	    [TestMethod]
	    public void AddGoal()
	    {
			AddFootballer();
		    AddReferee();

			context.Addresses.Add(new Address() { City = "Warsaw", Number = "45C", Street = "Pruszkowska", Zipcode = "02-101" });
			context.Addresses.Add(new Address() { City = "Pruszków", Number = "45C", Street = "Warszawska", Zipcode = "02-101" });
		    context.SaveChanges();

			AddCoach();
			AddCoach();

			context.Teams.Add(new Team()
			{
				Name = "Pogoń Wiskitki",
				Founded = DateTime.Today,
				Address = context.Addresses.First(),
				Budget = 50000,
				Coach = context.Coaches.First()
			});
			context.Teams.Add(new Team()
			{
				Name = "Dupa Jasiu",
				Founded = DateTime.Today,
				Address = context.Addresses.Find(2),
				Budget = 50000,
				Coach = context.Coaches.Find(2)
			});
			context.SaveChanges();

			context.Seasons.Add(new Season() { Name = "2016/2017" });

			context.Stadiums.Add(new Stadium() { Name = "Skalnik Arena", Address = context.Addresses.First(), Capacity = 5000 });

			context.SaveChanges();

			context.Matches.Add(new Match() { Season = context.Seasons.First(), HomeTeam = context.Teams.First(), AwayTeam = context.Teams.Find(2),
				Date = DateTime.Today, Referee = context.Referees.First(), Stadium = context.Stadiums.First()});
			context.SaveChanges();

		    var previousLength = context.Goals.Count();
		    Action act = () =>
		    {
			    context.Goals.Add(new Goal() { Footballer = context.Footballers.First(), Match = context.Matches.First(), Type = GoalType.Normal, Team = context.Teams.First()});
			    context.SaveChanges();
		    };

			act.ShouldNotThrow();
		    context.Goals.Count().Should().Be(previousLength + 1);
	    }

	    [TestMethod]
	    public void AddMatch()
	    {
			context.Addresses.Add(new Address() { City = "Warsaw", Number = "45C", Street = "Pruszkowska", Zipcode = "02-101" });
			context.Addresses.Add(new Address() { City = "Pruszków", Number = "45C", Street = "Warszawska", Zipcode = "02-101" });
		    context.SaveChanges();
			
			AddCoach();
			AddCoach();

			context.Teams.Add(new Team() { Address = context.Addresses.First(), Founded = DateTime.Now, Coach = context.Coaches.First()});
			context.Teams.Add(new Team() { Address = context.Addresses.Find(2), Founded = DateTime.Now, Coach = context.Coaches.Find(2)});

			context.Seasons.Add(new Season() { Name = "2016/2017" });

			context.Stadiums.Add(new Stadium() { Name = "Skalnik Arena", Address = context.Addresses.First(), Capacity = 5000 });

			AddReferee();

		    var previousCount = context.Matches.Count();
		    Action act = () =>
		    {
			    context.Matches.Add(new Match() { Season = context.Seasons.First(), HomeTeam = context.Teams.First(), AwayTeam = context.Teams.Find(2), Date = DateTime.Today,
				    Referee = context.Referees.First(), Stadium = context.Stadiums.First()});
			    context.SaveChanges();
		    };

			act.ShouldNotThrow();
			context.Matches.Count().Should().Be(previousCount + 1);
	    }

	    [TestMethod]
	    public void AddStadium()
	    {
			context.Addresses.Add(new Address() { City = "Warsaw", Number = "45C", Street = "Pruszkowska", Zipcode = "02-101" });
		    context.SaveChanges();

			var previousCount = context.Stadiums.Count();
			Action act = () =>
		    {
			    context.Stadiums.Add(new Stadium() {Name = "Skalnik Arena", Address = context.Addresses.First(), Capacity = 5000});
			    context.SaveChanges();
		    };
			
			act.ShouldNotThrow();
			context.Stadiums.Count().Should().Be(previousCount + 1);
		}

	    [TestMethod]
	    public void AddTeam()
	    {
			context.Addresses.Add(new Address() { City = "Madison, Wisconsin", Number = "997", Street = "Testoviron St.", Zipcode = "02-101" });
			context.SaveChanges();
			AddCoach();

			var previousCount = context.Teams.Count();
			Action act = () =>
		    {
			    context.Teams.Add(new Team() { Name = "Pogoń Wiskitki", Founded = DateTime.Today, Budget = 50000, Address = context.Addresses.First(), Coach = context.Coaches.First()});
			    context.SaveChanges();
		    };

			act.ShouldNotThrow();
			context.Teams.Count().Should().Be(previousCount + 1);
	    }
	}
}
