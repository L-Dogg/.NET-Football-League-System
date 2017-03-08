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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace FootballManager.Web.Tests
{
	[TestClass]
	public class MatchesControllerTests
	{
		private MatchesController controller;
		private Mock<ILeagueService> service;
		private HttpRequestMessage request;

		[TestInitialize]
		public void Initialize()
		{
			this.service = new Mock<ILeagueService>();
			this.controller = new MatchesController(service.Object);
			this.request = new HttpRequestMessage();
			request.SetConfiguration(new System.Web.Http.HttpConfiguration());
			Mapper.Initialize(x =>
			{
				x.AddProfile<DomainToViewModelMappingProfile>();
			});
		}

		[TestMethod]
		public void get_previous_round()
		{
			this.service.Setup(x => x.GetPreviousRound()).Returns(new List<Domain.Entity.Models.Match>());
			var result = this.controller.GetPrevious(request);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			this.service.Verify(x => x.GetPreviousRound(), Times.Once);
		}

		[TestMethod]
		public void get_next_round()
		{
			this.service.Setup(x => x.GetNextRound()).Returns(new List<Domain.Entity.Models.Match>());
			var result = this.controller.GetNext(request);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			this.service.Verify(x => x.GetNextRound(), Times.Once);
		}
	}
}
