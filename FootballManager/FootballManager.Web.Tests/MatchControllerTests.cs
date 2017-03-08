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
	public class MatchControllerTests
	{
		private MatchController controller;
		private Mock<ILeagueService> service;
		private HttpRequestMessage request;
		[TestInitialize]
		public void Initialize()
		{
			this.service = new Mock<ILeagueService>();
			this.controller = new MatchController(service.Object);
			this.request = new HttpRequestMessage();
			request.SetConfiguration(new System.Web.Http.HttpConfiguration());
			Mapper.Initialize(x =>
			{
				x.AddProfile<DomainToViewModelMappingProfile>();
			});
		}

		[TestMethod]
		public async Task get_match()
		{
			this.service.Setup(x => x.GetMatch(It.IsAny<int>())).ReturnsAsync(new Domain.Entity.Models.Match() { Id = 1});
			var result = await this.controller.GetMatch(request, 1);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			MatchViewModel viewModel;
			result.TryGetContentValue<MatchViewModel>(out viewModel);
			viewModel.Should().NotBeNull();
			viewModel.Id.Should().Be(1);
		}

		[TestMethod]
		public async Task get_nonexisting_match()
		{
			this.service.Setup(x => x.GetMatch(It.IsAny<int>())).ThrowsAsync(new Exception());
			var result = await this.controller.GetMatch(request, 1);
			result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
		}

		[TestMethod]
		public async Task save_comment()
		{
			var result = await this.controller.SaveComment(request, new Comment() { Id = 1 });
			this.service.Verify(x => x.SaveComment(It.IsAny<Comment>()), Times.Once);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
		}
	}
}
