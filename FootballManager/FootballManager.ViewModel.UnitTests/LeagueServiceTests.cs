using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FootballManager.Domain.Entity.Models;
using System.Collections.Generic;
using System.Data.Entity;
using FluentAssertions;
using System.Linq;
using FootballManager.Domain.Entity.Contexts.LeagueContext;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using TestingDemo;
using FootballManager.AuthenticationLayer;

namespace FootballManager.BusinessLayer.UnitTests
{
	[TestClass]
	public class LeagueServiceTests
	{
		private Mock<LeagueService> viewModel;
		private Mock<LeagueContext> context;
		private List<Address> addressList = new List<Address>();
		private List<Coach> coachesList = new List<Coach>();
		private List<Footballer> footballersList = new List<Footballer>();
		private List<Referee> refereesList = new List<Referee>();
		private List<Stadium> stadiumsList = new List<Stadium>();
		private List<Team> teamsList = new List<Team>();
		private List<Goal> goalsList = new List<Goal>();
		private List<Domain.Entity.Models.Match> matchesList = new List<Domain.Entity.Models.Match>();
		private List<Season> seasonsList = new List<Season>();
		private List<Table> tablesList = new List<Table>();
		private List<Comment> commentsList = new List<Comment>();
		private List<Notification> notificationsList = new List<Notification>();
		private Mock<IAuthenticationService> authenticationService;

		[TestInitialize]
		public void Initialize()
		{

			var contextMock = new Mock<LeagueContext>();

			seasonsList.Add(new Season() { Id = 1, Name = "Sezon 2016/17" });
			coachesList.Add(new Coach() { Id = 1, FirstName = "Piotr", Surname = "Nowak" });
			stadiumsList.Add(new Stadium() { Id = 1, Name = "Stadion Energa Gdańsk", Capacity = 43165, Address = new Address() { Street = "Pokoleń Lechii Gdańsk", Number = "1", Zipcode = "80-560", City = "Gdańsk" } });
			teamsList.Add(new Team() { Id = 1, Name = "Lechia Gdańsk", Budget = 100, CoachId = 1, StadiumId = 1, Salaries = 11, Founded = new DateTime(1945, 1, 1), Address = new Address() { Street = "Pokoleń Lechii Gdańsk", Number = "1", Zipcode = "80-560", City = "Gdańsk" } });
			footballersList.Add(new Footballer() { Id = 1, FirstName = "Mateusz", Surname = "Bąk", BirthDate = new DateTime(1983, 2, 24), TeamId = 1, Salary = 1 });
			refereesList.Add(new Referee() { Id = 1, FirstName = "Andrzej", Surname = "Badgirl" });
			matchesList.Add(new Domain.Entity.Models.Match() { Id = 1, HomeTeamId = 1, AwayTeamId = 2, Date = DateTime.Today, RefereeId = 1, SeasonId = 1, Round = 1, StadiumId = 1 });
			goalsList.Add(new Goal() { Id = 1, MatchId = 1, Type = Domain.Entity.Enums.GoalType.Normal, FootballerId = 1, Match = new Domain.Entity.Models.Match() { SeasonId = 1 } });
			tablesList.Add(new Table() { Id = 1, TeamId = 1, SeasonId = 1, GoalsScored = 10, GoalsConceded = 5, Points = 10, Position = 2 });
			commentsList.Add(new Comment() { Id = 1 });
			notificationsList.Add(new Notification() { Id = 1, FacebookId = "1", TeamId = 1 });

			contextMock.Setup(x => x.Addresses).Returns(CreateDbSetMock(addressList).Object);
			contextMock.Setup(x => x.Coaches).Returns(CreateDbSetMock(coachesList).Object);
			contextMock.Setup(x => x.Footballers).Returns(CreateDbSetMock(footballersList).Object);
			contextMock.Setup(x => x.Referees).Returns(CreateDbSetMock(refereesList).Object);
			contextMock.Setup(x => x.Stadiums).Returns(CreateDbSetMock(stadiumsList).Object);
			contextMock.Setup(x => x.Teams).Returns(CreateDbSetMock(teamsList).Object);
			contextMock.Setup(x => x.Goals).Returns(CreateDbSetMock(goalsList).Object);
			contextMock.Setup(x => x.Matches).Returns(CreateDbSetMock(matchesList).Object);
			contextMock.Setup(x => x.Seasons).Returns(CreateDbSetMock(seasonsList).Object);
			contextMock.Setup(x => x.Tables).Returns(CreateDbSetMock(tablesList).Object);
			contextMock.Setup(x => x.Comments).Returns(CreateDbSetMock(commentsList).Object);
			contextMock.Setup(x => x.Notifications).Returns(CreateDbSetMock(notificationsList).Object);

			this.authenticationService = new Mock<IAuthenticationService>();
			this.viewModel = new Mock<LeagueService>(contextMock.Object, authenticationService.Object);
			this.context = contextMock;

		}

		[TestCleanup]
		public void Cleanup()
		{
			this.context.Object.Dispose();
		}

		#region Get List Tests
		[TestMethod]
		public async Task get_coaches_list_count()
		{
			var list = await this.viewModel.Object.GetCoachList();
			list.Count.Should().Be(this.coachesList.Count);
		}

		[TestMethod]
		public async Task get_teams_list_count()
		{
			var list = await this.viewModel.Object.GetTeamsList();
			list.Count.Should().Be(this.teamsList.Count);
		}

		[TestMethod]
		public async Task get_footballers_list_count()
		{
			var list = await this.viewModel.Object.GetFootballersList();
			list.Count.Should().Be(this.footballersList.Count);
		}

		[TestMethod]
		public async Task get_stadiums_list_count()
		{
			var list = await this.viewModel.Object.GetStadiumsList();
			list.Count.Should().Be(this.stadiumsList.Count);
		}

		[TestMethod]
		public async Task get_seasons_list_count()
		{
			var list = await this.viewModel.Object.GetSeasonsList();
			list.Count.Should().Be(this.seasonsList.Count);
		}

		[TestMethod]
		public async Task get_referees_list_count()
		{
			var list = await this.viewModel.Object.GetRefereeList();
			list.Count.Should().Be(this.refereesList.Count);
		}

		[TestMethod]
		public async Task get_empty_list()
		{
			this.refereesList.Clear();
			var list = await this.viewModel.Object.GetRefereeList();
			list.Count.Should().Be(0);
		}

		#endregion

		#region Add Items

		[TestMethod]
		public async Task add_new_season()
		{
			await this.viewModel.Object.AddSeason(new Season() { Name = "TestSeason" });
			this.context.Verify(x => x.Seasons, Times.Once);
			this.context.Verify(x => x.SaveChangesAsync(), Times.Once);
			this.seasonsList.Count.Should().Be(2);
		}

		[TestMethod]
		public async Task add_new_team()
		{
			await this.viewModel.Object.AddTeam(new Team());
			this.context.Verify(x => x.Teams, Times.Once);
			this.context.Verify(x => x.SaveChangesAsync(), Times.Once);
			this.teamsList.Count.Should().Be(2);
		}

		[TestMethod]
		public async Task add_new_referee()
		{
			await this.viewModel.Object.AddReferee(new Referee());
			this.context.Verify(x => x.Referees, Times.Once);
			this.context.Verify(x => x.SaveChangesAsync(), Times.Once);
			this.authenticationService.Verify(x => x.CreateRefereeAccount(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			this.refereesList.Count.Should().Be(2);
		}

		[TestMethod]
		public async Task add_new_coach()
		{
			await this.viewModel.Object.AddCoach(new Coach());
			this.context.Verify(x => x.Coaches, Times.Once);
			this.context.Verify(x => x.SaveChangesAsync(), Times.Once);
			this.coachesList.Count.Should().Be(2);
		}

		[TestMethod]
		public async Task add_new_match()
		{
			await this.viewModel.Object.AddMatch(new Domain.Entity.Models.Match());
			this.context.Verify(x => x.Matches, Times.Once);
			this.context.Verify(x => x.SaveChangesAsync(), Times.Once);
			this.matchesList.Count.Should().Be(2);
		}

		[TestMethod]
		public async Task add_new_goal()
		{
			await this.viewModel.Object.AddGoal(new Goal());
			this.context.Verify(x => x.Goals, Times.Once);
			this.context.Verify(x => x.SaveChangesAsync(), Times.Once);
			this.goalsList.Count.Should().Be(2);
		}

		[TestMethod]
		public async Task add_new_address()
		{
			await this.viewModel.Object.AddAddress(new Address());
			this.context.Verify(x => x.Addresses, Times.Once);
			this.context.Verify(x => x.SaveChangesAsync(), Times.Once);
			this.addressList.Count.Should().Be(1);
		}

		[TestMethod]
		public async Task add_new_stadium()
		{
			await this.viewModel.Object.AddStadium(new Stadium());
			this.context.Verify(x => x.Stadiums, Times.Once);
			this.context.Verify(x => x.SaveChangesAsync(), Times.Once);
			this.stadiumsList.Count.Should().Be(2);
		}

		[TestMethod]
		public async Task add_player_in_budget()
		{
			var result = await this.viewModel.Object.AddFootballer(new Footballer() { FirstName = "Adam", Surname = "Chrzanowski", BirthDate = new DateTime(1999, 3, 31), TeamId = 1, Salary = 1 });
			result.Should().Be(true);
			this.footballersList.Count.Should().Be(2);
		}

		[TestMethod]
		public async Task add_player_over_budget()
		{
			var result = await this.viewModel.Object.AddFootballer(new Footballer() { FirstName = "Adam", Surname = "Chrzanowski", BirthDate = new DateTime(1999, 3, 31), TeamId = 1, Salary = 200 });
			result.Should().Be(false);
			this.footballersList.Count.Should().Be(1);
		}

		#endregion

		#region Get single item

		[TestMethod]
		public async Task get_footballer()
		{
			var result = await this.viewModel.Object.GetFootballer(1);
			result.Should().NotBeNull();
			result.Should().Be(this.footballersList.First());
		}

		[TestMethod]
		public async Task get_referee()
		{
			var result = await this.viewModel.Object.GetReferee(1);
			result.Should().NotBeNull();
			result.Should().Be(this.refereesList.First());
		}

		[TestMethod]
		public async Task get_coach()
		{
			var result = await this.viewModel.Object.GetCoach(1);
			result.Should().NotBeNull();
			result.Should().Be(this.coachesList.First());
		}

		[TestMethod]
		public async Task get_not_existing_object()
		{
			try
			{
				var result = (await this.viewModel.Object.GetCoach(-1));
			}
			catch (Exception ex)
			{
				ex.Should().NotBeNull();
			}
		}

		#endregion

		[TestMethod]
		public void remove_player_from_team()
		{
			this.viewModel.Object.RemovePlayerFromTeam(this.footballersList.First());
			this.footballersList.First().TeamId.Should().Be(null);
		}

		[TestMethod]
		public void update_salaries()
		{
			this.teamsList.First().Footballers = new List<Footballer>();
			this.viewModel.Object.UpdateTeamSalaries(this.teamsList.First(),0);
			this.context.Verify(x => x.SaveChangesAsync(), Times.Never);
		}

		[TestMethod]
		public async Task generate_fixtures_should_fail()
		{
			var result = await this.viewModel.Object.GenerateFixtures(1, DateTime.Now);
			result.Should().Be(false);
		}

		[TestMethod]
		public async Task generate_fixtures_should_succeed()
		{
			this.matchesList.Clear();
			var result = await this.viewModel.Object.GenerateFixtures(1, DateTime.Now);
			result.Should().Be(true);
		}

		[TestMethod]
		public async Task update_match()
		{
			var match = new Domain.Entity.Models.Match() { Id = 1, Attendance = 100, HomeGoals = 0, AwayGoals = 0, HomeTeamId = 1, AwayTeamId = 2, SeasonId = 1, Goals = new List<Goal>() };
			this.tablesList.Add(new Table() { TeamId = 2, SeasonId = 1 });
			await this.viewModel.Object.UpdateMatch(match);
			this.matchesList.First().Attendance.Should().Be(100);
		}

		[TestMethod]
		public void get_next_round()
		{
			var matchNext = new Domain.Entity.Models.Match() { Id = 1, Attendance = 100, HomeGoals = 0, AwayGoals = 0, HomeTeamId = 1, AwayTeamId = 2, SeasonId = 1, Goals = new List<Goal>(), Round = 1, Date = DateTime.Now.AddDays(1) };
			var matchPrev = new Domain.Entity.Models.Match() { Id = 1, Attendance = 100, HomeGoals = 0, AwayGoals = 0, HomeTeamId = 1, AwayTeamId = 2, SeasonId = 1, Goals = new List<Goal>(), Round = 2, Date = DateTime.Now.AddDays(-1) };
			this.matchesList.Add(matchNext);
			this.matchesList.Add(matchPrev);
			var result = this.viewModel.Object.GetNextRound();
			result.Should().NotBeNull();
			result.Count.Should().Be(1);
			result.Contains(matchNext).Should().BeTrue();
		}

		[TestMethod]
		public void get_previous_round()
		{
			var matchNext = new Domain.Entity.Models.Match() { Id = 1, Attendance = 100, HomeGoals = 0, AwayGoals = 0, HomeTeamId = 1, AwayTeamId = 2, SeasonId = 1, Goals = new List<Goal>(), Round = 1, Date = DateTime.Now.AddDays(1) };
			var matchPrev = new Domain.Entity.Models.Match() { Id = 1, Attendance = 100, HomeGoals = 0, AwayGoals = 0, HomeTeamId = 1, AwayTeamId = 2, SeasonId = 1, Goals = new List<Goal>(), Round = 2, Date = DateTime.Now.AddDays(-1) };
			this.matchesList.Add(matchNext);
			this.matchesList.Add(matchPrev);
			var result = this.viewModel.Object.GetPreviousRound();
			result.Should().NotBeNull();
			result.Count.Should().Be(1);
			result.Contains(matchPrev).Should().BeTrue();
		}

		[TestMethod]
		public void get_next_round_when_empty()
		{
			this.matchesList = new List<Domain.Entity.Models.Match>();
			var result = this.viewModel.Object.GetNextRound();
			result.Count.Should().Be(0);
		}

		[TestMethod]
		public async Task get_scorers_table()
		{
			this.footballersList.Add(new Footballer() { Id = 2, FirstName = "Mateusz", Surname = "Bąk", BirthDate = new DateTime(1983, 2, 24), TeamId = 1, Salary = 1 });
			this.goalsList.Add(new Goal() { Id = 2, MatchId = 1, Type = Domain.Entity.Enums.GoalType.Own, FootballerId = 1, Match = new Domain.Entity.Models.Match() { SeasonId = 1 } });
			var result = await this.viewModel.Object.GetScorersTable(1, 20);
			result.Count.Should().Be(1);
			result.First().Value.Should().Be(1);
		}
		
		[TestMethod]
		public async Task save_comment()
		{
			await this.viewModel.Object.SaveComment(new Comment() { Id = 2 });
			this.commentsList.Count.Should().Be(2);
		}

		[TestMethod]
		public async Task get_user_notifications()
		{
			var result = await this.viewModel.Object.GetUserNotifications("1");
			result.Should().NotBeEmpty();
			result.Count.Should().Be(1);
			result.First().FacebookId.Should().Be("1");
		}

		[TestMethod]
		public async Task add_notification()
		{
			await this.viewModel.Object.AddNotification(new Notification() { Id = 2, FacebookId = "2", TeamId = 1 });
			this.notificationsList.Count.Should().Be(2);
		}

		private Mock<DbSet<T>> CreateDbSetMock<T>(List<T> elements) where T : class
		{
			var elementsAsQueryable = elements.AsQueryable();
			var dbSetMock = new Mock<DbSet<T>>();

			dbSetMock.As<IDbAsyncEnumerable<T>>()
	.		Setup(m => m.GetAsyncEnumerator())
			.Returns(new TestDbAsyncEnumerator<T>(elementsAsQueryable.GetEnumerator()));
			dbSetMock.As<IQueryable<T>>()
			.Setup(m => m.Provider)
			.Returns(new TestDbAsyncQueryProvider<T>(elementsAsQueryable.Provider));
			dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
			dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
			dbSetMock.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(elements.Add);
			return dbSetMock;
		}
	}
}
