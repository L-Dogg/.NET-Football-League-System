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
using FootballManager.BusinessLayer.Models;

namespace FootballManager.Web.Tests
{
	[TestClass]
	public class CoachesControllerTests
	{
		private CoachesController controller;
		private Mock<ILeagueService> service;
		private HttpRequestMessage request;
		[TestInitialize]
		public void Initialize()
		{
			this.service = new Mock<ILeagueService>();
			this.controller = new CoachesController(service.Object);
			this.request = new HttpRequestMessage();
			request.SetConfiguration(new System.Web.Http.HttpConfiguration());
			Mapper.Initialize(x =>
			{
				x.AddProfile<DomainToViewModelMappingProfile>();
			});
		}

		[TestMethod]
		public async Task GetExistingCoach()
		{
			this.service.Setup(x => x.GetCoach(It.IsAny<int>())).ReturnsAsync(new Coach()
			{
				Id = 1,
				BirthDate = DateTime.Today,
				FirstName = "Adam",
				Surname = "Nowak",
				PictureUrl = "123",
				Teams = new List<Team>()
			});
			var result = await this.controller.GetCoach(request, 1);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			CoachesViewModel viewModel;
			result.TryGetContentValue(out viewModel);
			viewModel.Should().NotBeNull();
			viewModel.Id.Should().Be(1);
		}

		[TestMethod]
		public async Task GetNonexistingCoach()
		{
			this.service.Setup(x => x.GetCoach(It.IsAny<int>())).ThrowsAsync(new Exception());
			var result = await this.controller.GetCoach(request, 1);
			result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
		}

		[TestMethod]
		public async Task GetFilteredList()
		{
			this.service.Setup(x => x.GetFilteredCoachesList(It.IsAny<string>())).
				ReturnsAsync(new List<SelectItem>(5));
			var result = await this.controller.GetFiltered(request, "filter");
			result.StatusCode.Should().Be(HttpStatusCode.OK);
		}
	}
}
