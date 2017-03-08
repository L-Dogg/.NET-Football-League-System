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
	public class NotificationControllerTest
	{
		private NotificationController controller;
		private Mock<ILeagueService> service;
		private HttpRequestMessage request;

		[TestInitialize]
		public void Initialize()
		{
			this.service = new Mock<ILeagueService>();
			this.controller = new NotificationController(service.Object);
			this.request = new HttpRequestMessage();
			request.SetConfiguration(new System.Web.Http.HttpConfiguration());
			Mapper.Initialize(x =>
			{
				x.AddProfile<DomainToViewModelMappingProfile>();
			});
		}

		[TestMethod]
		public async Task get_user_notifications()
		{
			this.service.Setup(x => x.GetUserNotifications(It.IsAny<string>())).ReturnsAsync(new List<Notification>());
			var result = await this.controller.GetFilteredList(request, "1");
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			this.service.Verify(x => x.GetUserNotifications(It.IsAny<string>()), Times.Once);
		}

		[TestMethod]
		public async Task add_notification()
		{
			this.request.Method = HttpMethod.Post;
			var result = await this.controller.AddNotification(request, new NotificationViewModel() {Id = 1, FacebookId = "1", Team = new BusinessLayer.Models.SelectItem() { Id = 1 } });
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			this.service.Verify(x => x.AddNotification(It.IsAny<Notification>()), Times.Once);
		}

		[TestMethod]
		public async Task remove_notification()
		{
			this.request.Method = HttpMethod.Post;
			var result = await this.controller.RemoveNotification(request, new NotificationViewModel() { Id = 1, FacebookId = "1", Team = new BusinessLayer.Models.SelectItem() { Id = 1 } });
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			this.service.Verify(x => x.RemoveNotification(It.IsAny<Notification>()), Times.Once);
		}
	}
}
