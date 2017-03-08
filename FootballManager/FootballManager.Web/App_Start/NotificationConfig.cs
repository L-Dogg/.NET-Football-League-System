using Autofac;
using FootballManager.AuthenticationLayer;
using FootballManager.BusinessLayer;
using FootballManager.Domain.Entity.Contexts.AuthenticationContext;
using FootballManager.Domain.Entity.Contexts.LeagueContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace FootballManager.Web.App_Start
{
	public static class NotificationConfig
	{
		public static void StartNotifications()
		{
			Task.Run(async () =>
			{
				string appToken = "247733828993169|flLA5gjH3CwUumwl5wzvJF8Dv2E";
				while (true)
				{
					var service = new LeagueService(new LeagueContext(), new AuthenticationService(new AuthenticationContext()));
					var notifications = await service.GetNotificationsToSend();
					foreach (var notification in notifications)
					{
						HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, $"https://graph.facebook.com//{notification.FacebookId}/notifications?access_token={appToken}&template={notification.Team.Name} is playing on {DateTime.Today.AddDays(1)}");
						HttpClient client = new HttpClient();
						await client.SendAsync(req);
						Thread.Sleep(new TimeSpan(0, 1, 0));
					}
					service.Dispose();
					Thread.Sleep(new TimeSpan(24, 0, 0));
				}
			});
		}
	}
}