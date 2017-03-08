using AutoMapper;
using FluentAssertions;
using FootballManager.BusinessLayer;
using FootballManager.Domain.Entity.Models;
using FootballManager.Web.Controllers;
using FootballManager.Web.Infrastructure.Mappings;
using FootballManager.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FootballManager.BusinessLayer.Models;
using Match = FootballManager.Domain.Entity.Models.Match;

namespace FootballManager.Web.Tests
{
	[TestClass]
	public class TeamControllerTests
	{
		private TeamController controller;
		private Mock<ILeagueService> service;
		private HttpRequestMessage request;

		[TestInitialize]
		public void Initialize()
		{
			this.service = new Mock<ILeagueService>();
			this.controller = new TeamController(service.Object);
			this.request = new HttpRequestMessage();
			request.SetConfiguration(new System.Web.Http.HttpConfiguration());
			Mapper.Initialize(x =>
			{
				x.AddProfile<DomainToViewModelMappingProfile>();
			});
		}

		[TestMethod]
		public async Task GetExistingTeam()
		{
			this.service.Setup(x => x.GetTeam(It.IsAny<int>())).ReturnsAsync(new Team()
			{
				Id = 1,
				Coach = new Coach() { Id = 1, FirstName = "a", Surname = "b"},
				Name = "Team",
				Table = new List<Table>(),
				Stadium = new Stadium() { Id = 1, Name = "Stadium"}
			});
			this.service.Setup(x => x.GetTeamsMatches(It.IsAny<int>())).ReturnsAsync(
				new List<Match>(1) {new Match()});
			var result = await this.controller.GetTeam(request, 1);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			TeamViewModel viewModel;
			result.TryGetContentValue(out viewModel);
			viewModel.Should().NotBeNull();
			viewModel.Id.Should().Be(1);
			viewModel.Coach.Value.Should().Be("a b");
		}

		[TestMethod]
		public async Task GetNonexistingTeam()
		{
			this.service.Setup(x => x.GetSeason(It.IsAny<int>())).ThrowsAsync(new Exception());
			var result = await this.controller.GetTeam(request, 1);
			result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
		}

		[TestMethod]
		public async Task GetFilteredList()
		{
			this.service.Setup(x => x.GetFilteredTeamsList(It.IsAny<string>())).
				ReturnsAsync(new List<SelectItem>(1) { new SelectItem() { Id = 1, Name = "Team" } });
			var result = await this.controller.GetFilteredList(request, "filter");
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			IEnumerable<SelectItem> list;
			result.TryGetContentValue(out list);
			list.Count().Should().Be(1);
		}
	}
}
