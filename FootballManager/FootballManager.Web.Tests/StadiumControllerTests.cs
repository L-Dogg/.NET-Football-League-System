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
	public class StadiumControllerTests
	{
		private StadiumController controller;
		private Mock<ILeagueService> service;
		private HttpRequestMessage request;
		[TestInitialize]
		public void Initialize()
		{
			this.service = new Mock<ILeagueService>();
			this.controller = new StadiumController(service.Object);
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
			this.service.Setup(x => x.GetStadium(It.IsAny<int>())).ReturnsAsync(new Stadium() { Id = 1 });
			var result = await this.controller.GetStadium(request, 1);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			StadiumViewModel viewModel;
			result.TryGetContentValue(out viewModel);
			viewModel.Should().NotBeNull();
			viewModel.Id.Should().Be(1);
		}

		[TestMethod]
		public async Task GetNonexistingSeason()
		{
			this.service.Setup(x => x.GetStadium(It.IsAny<int>())).ThrowsAsync(new Exception());
			var result = await this.controller.GetStadium(request, 1);
			result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
		}

		[TestMethod]
		public async Task GetFilteredList()
		{
			this.service.Setup(x => x.GetFilteredStadiumsList(It.IsAny<string>())).
				ReturnsAsync(new List<SelectItem>(5) { new SelectItem() { Id = 1, Name = "Stadion" } });
			var result = await this.controller.GetFilteredList(request, "filter");
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			IEnumerable<SelectItem> list;
			result.TryGetContentValue(out list);
			list.Count().Should().Be(1);
		}
	}
}
