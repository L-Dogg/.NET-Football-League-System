using AutoMapper;
using FootballManager.BusinessLayer;
using FootballManager.BusinessLayer.Models;
using FootballManager.Domain.Entity.Models;
using FootballManager.Web.Infrastructure.core;
using FootballManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FootballManager.Web.Controllers
{
	[Authorize(Roles = "User")]
	[RoutePrefix("api/notification")]
	public class NotificationController : ApiControllerBase
	{
		private readonly ILeagueService leagueService;


		public NotificationController(ILeagueService leagueService)
		{
			this.leagueService = leagueService;
		}

		[Route("list/{id}")]
		public async Task<HttpResponseMessage> GetFilteredList(HttpRequestMessage request, string id)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var notifications = await this.leagueService.GetUserNotifications(id);
				var notificationsVm = Mapper.Map<List<NotificationViewModel>>(notifications);
				response = request.CreateResponse<List<NotificationViewModel>>(HttpStatusCode.OK, notificationsVm);

				return response;
			});
		}

		[HttpPost]
		[Route("addNotification")]
		public async Task<HttpResponseMessage> AddNotification(HttpRequestMessage request, NotificationViewModel notificationVm)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var notification = new Notification()
				{
					FacebookId = notificationVm.FacebookId,
					TeamId = notificationVm.Team.Id
				};
				await this.leagueService.AddNotification(notification);
				notificationVm.Id = notification.Id;
				response = request.CreateResponse<NotificationViewModel>(HttpStatusCode.OK, notificationVm);

				return response;
			});
		}


		[HttpPost]
		[Route("removeNotification")]
		public async Task<HttpResponseMessage> RemoveNotification(HttpRequestMessage request, NotificationViewModel notificationVm)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var notification = new Notification()
				{
					Id = notificationVm.Id,
					FacebookId = notificationVm.FacebookId,
					TeamId = notificationVm.Team.Id
				};
				await this.leagueService.RemoveNotification(notification);
				response = request.CreateResponse<NotificationViewModel>(HttpStatusCode.OK, notificationVm);

				return response;
			});
		}

	}
}