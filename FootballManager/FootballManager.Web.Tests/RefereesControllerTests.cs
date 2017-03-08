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
	public class RefereesControllerTests
	{
		private RefereesController controller;
		private Mock<ILeagueService> service;
		private HttpRequestMessage request;

		[TestInitialize]
		public void Initialize()
		{
			this.service = new Mock<ILeagueService>();
			this.controller = new RefereesController(service.Object);
			this.request = new HttpRequestMessage();
			request.SetConfiguration(new System.Web.Http.HttpConfiguration());
			Mapper.Initialize(x =>
			{
				x.AddProfile<DomainToViewModelMappingProfile>();
			});
		}

		[TestMethod]
		public async Task get_filtered_referees_list()
		{
			this.service.Setup(x => x.GetFilteredRefereeList(It.IsAny<string>())).ReturnsAsync(new List<BusinessLayer.Models.SelectItem>());
			var result = await this.controller.GetFiltered(request, "filter");
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			this.service.Verify(x => x.GetFilteredRefereeList(It.IsAny<string>()), Times.Once);
		}

		[TestMethod]
		public async Task get_existing_referee()
		{
			this.service.Setup(x => x.GetReferee(It.IsAny<int>())).ReturnsAsync(new Referee() { Id = 1});
			var result = await this.controller.GetReferee(request, 1);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			this.service.Verify(x => x.GetReferee(It.IsAny<int>()), Times.Once);
			RefereesViewModel viewModel;
			result.TryGetContentValue<RefereesViewModel>(out viewModel);
			viewModel.Should().NotBeNull();
			viewModel.Id.Should().Be(1);
		}

		[TestMethod]
		public async Task get_nonexisting_referee()
		{
			this.service.Setup(x => x.GetReferee(It.IsAny<int>())).ThrowsAsync(new Exception());
			var result = await this.controller.GetReferee(request, 1);
			result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
			this.service.Verify(x => x.GetReferee(It.IsAny<int>()), Times.Once);

		}
	}
}
