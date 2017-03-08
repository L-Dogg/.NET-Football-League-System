using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefereeServiceLibrary;
using FootballManager.BusinessLayer;
using Moq;
using FootballManager.AuthenticationLayer;
using FootballManager.Domain.Entity.Models.Authentication.Enums;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballManager.RefereeService.UnitTests
{
	[TestClass]
	public class RefereeServiceTests
	{
		private IRefereeService refService;
		private Mock<IRefereeLeagueService> leagueService;
		private Mock<IAuthenticationService> authenticationService;
		[TestInitialize]
		public void TestInitialize()
		{
			this.leagueService = new Mock<IRefereeLeagueService>();
			this.authenticationService = new Mock<IAuthenticationService>();
			this.refService = new RefereeServiceLibrary.RefereeService(leagueService.Object, authenticationService.Object);
		}

		[TestMethod]
		public void authenticate_referee_success()
		{
			this.authenticationService.Setup(x => x.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserType>())).Returns(1);
			this.refService.AuthenticateReferee("test", "test", UserType.Admin).Should().Be(1);
			this.authenticationService.Verify(x => x.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserType>()), Times.Once);			
		}

		[TestMethod]
		public void authenticate_referee_failed()
		{
			this.authenticationService.Setup(x => x.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserType>())).Returns(-1);
			this.refService.AuthenticateReferee("test", "test", UserType.Admin).Should().Be(-1);
			this.authenticationService.Verify(x => x.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserType>()), Times.Once);
		}

		[TestMethod]
		public void change_password_success()
		{
			this.authenticationService.Setup(x => x.ChangePassword(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
			this.refService.ChangePassword(1, "test", "test").Should().Be(true);
			this.authenticationService.Verify(x => x.ChangePassword(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
		}

		[TestMethod]
		public void change_password_failed()
		{
			this.authenticationService.Setup(x => x.ChangePassword(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
			this.refService.ChangePassword(1, "test", "test").Should().Be(false);
			this.authenticationService.Verify(x => x.ChangePassword(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
		}

		[TestMethod]
		public async Task get_referee_matches()
		{
			this.leagueService.Setup(x => x.GetRefereeByUserId(It.IsAny<int>())).ReturnsAsync(new Domain.Entity.Models.Referee() { Matches = new List<Domain.Entity.Models.Match>() });
			var result = await this.refService.GetMatchesList(1);
			result.Should().NotBeNull();
			this.leagueService.Verify(x => x.GetRefereeByUserId(It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		public async Task get_referee_match()
		{
			this.leagueService.Setup(x => x.GetMatch(It.IsAny<int>())).ReturnsAsync(new Domain.Entity.Models.Match()
			{
				AwayGoals = 0,
				HomeGoals = 0,
				HomeTeamId = 1,
				AwayTeamId = 2,
				HomeTeam = new Domain.Entity.Models.Team() { Footballers = new List<Domain.Entity.Models.Footballer>() },
				AwayTeam = new Domain.Entity.Models.Team() { Footballers = new List<Domain.Entity.Models.Footballer>() },
				Referee = new Domain.Entity.Models.Referee(),
				Stadium = new Domain.Entity.Models.Stadium() { Address = new Domain.Entity.Models.Address()},
				Goals = new List<Domain.Entity.Models.Goal>()

			});
			var result = await this.refService.GetMatch(1);
			result.Should().NotBeNull();
			this.leagueService.Verify(x => x.GetMatch(It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		public async Task update_match()
		{
			await this.refService.SaveGoals(new RefereeServiceLibrary.DTOs.MatchDTO() { HomeTeamGoals = new List<RefereeServiceLibrary.DTOs.GoalDTO>(), AwayTeamGoals = new List<RefereeServiceLibrary.DTOs.GoalDTO>(), Referee = new RefereeServiceLibrary.DTOs.RefereeDTO() });
			this.leagueService.Verify(x => x.UpdateMatch(It.IsAny<FootballManager.Domain.Entity.Models.Match>()), Times.Once);
		}
	}
}
