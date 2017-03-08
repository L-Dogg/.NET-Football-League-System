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
	public class SeasonControllerTests
	{
		private SeasonController controller;
		private Mock<ILeagueService> service;
		private HttpRequestMessage request;

		[TestInitialize]
		public void Initialize()
		{
			this.service = new Mock<ILeagueService>();
			this.controller = new SeasonController(service.Object);
			this.request = new HttpRequestMessage();
			request.SetConfiguration(new System.Web.Http.HttpConfiguration());
			Mapper.Initialize(x =>
			{
				x.AddProfile<DomainToViewModelMappingProfile>();
			});
		}

		[TestMethod]
		public async Task GetExistingSeason()
		{
			this.service.Setup(x => x.GetSeason(It.IsAny<int>())).ReturnsAsync(new Season()
			{
				Id = 1, Table = new Table[1] {new Table() {Points = 3} }, Name = "Sezon" ,
				Matches = new Match[1] {new Match() }
			});
			this.service.Setup(x => x.GetScorersTable(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(
				new List<KeyValuePair<Footballer, int>>(0));
			var result = await this.controller.GetSeason(request, 1);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			SeasonViewModel viewModel;
			result.TryGetContentValue(out viewModel);
			viewModel.Should().NotBeNull();
			viewModel.Id.Should().Be(1);
			viewModel.Tables.Count.Should().Be(1);
		}

		[TestMethod]
		public async Task GetNonexistingSeason()
		{
			this.service.Setup(x => x.GetSeason(It.IsAny<int>())).ThrowsAsync(new Exception());
			var result = await this.controller.GetSeason(request, 1);
			result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
		}

		[TestMethod]
		public async Task GetFilteredList()
		{
			this.service.Setup(x => x.GetFilteredSeasonsList(It.IsAny<string>())).
				ReturnsAsync(new List<SelectItem>(5) {new SelectItem() {Id = 1, Name= "Sezon"} });
			var result = await this.controller.GetList(request, "filter");
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			IEnumerable<SelectItem> list;
			result.TryGetContentValue(out list);
			list.Count().Should().Be(1);
		}
	}
}
