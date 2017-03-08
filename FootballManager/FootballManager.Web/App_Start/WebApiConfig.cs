using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FootballManager.Web.Infrastructure.MessageHandlers;

namespace FootballManager.Web
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			config.MessageHandlers.Add(new AuthHandler());

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			//config.Routes.MapHttpRoute(
			//	name: "DefaultListApi",
			//	routeTemplate: "api/{controller}/list/filter={filter}",
			//	defaults: new { filter = RouteParameter.Optional }
			//);

			//config.Routes.MapHttpRoute(
			//	name: "DefaultGetApi",
			//	routeTemplate: "api/{controller}/get/{id}",
			//	defaults: new { id = RouteParameter.Optional }
			//);
		}
	}
}
