using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using FootballManager.Domain.Entity.Contexts.LeagueContext;
using FootballManager.BusinessLayer;
using FootballManager.AuthenticationLayer;
using FootballManager.Domain.Entity.Contexts.AuthenticationContext;

namespace FootballManager.Web.App_Start
{
	public class AutofacWebapiConfig
	{
		public static IContainer Container;
		public static void Initialize(HttpConfiguration config)
		{
			Initialize(config, RegisterServices(new ContainerBuilder()));
		}

		public static void Initialize(HttpConfiguration config, IContainer container)
		{
			config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
		}

		private static IContainer RegisterServices(ContainerBuilder builder)
		{
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

			// EF Context
			builder.RegisterType<LeagueContext>()
				   .As<ILeagueContext>()
				   .InstancePerRequest();

			builder.RegisterType<AuthenticationContext>()
			.As<IAuthenticationContext>()
			.InstancePerRequest();
			
			// Services
			builder.RegisterType<LeagueService>()
				.As<ILeagueService>()
				.InstancePerRequest();

			builder.RegisterType<AuthenticationLayer.AuthenticationService>()
				.As<IAuthenticationService>()
				.InstancePerRequest();

			Container = builder.Build();
			NotificationConfig.StartNotifications();
			return Container;
		}
	}
}
