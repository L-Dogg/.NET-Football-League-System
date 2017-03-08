using FootballManager.AuthenticationLayer;
using FootballManager.BusinessLayer;
using FootballManager.Domain.Entity.Contexts.AuthenticationContext;
using FootballManager.Domain.Entity.Contexts.LeagueContext;
using FootballManager.Fans.Presentation.Controllers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Unity.WebApi;

namespace FootballManager.Fans.Presentation
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

			// register all your components with the container here
			// it is NOT necessary to register your controllers

			// e.g. container.RegisterType<ITestService, TestService>();
			container.RegisterType<ILeagueContext, LeagueContext>(new HierarchicalLifetimeManager());
			container.RegisterType<IAuthenticationContext, AuthenticationContext>(new HierarchicalLifetimeManager());
			container.RegisterType<ILeagueService, LeagueService>(new HierarchicalLifetimeManager());
			container.RegisterType<IAuthenticationService, AuthenticationService>(new HierarchicalLifetimeManager());
			var objectTypeInList = new List<Type>() { typeof(object) };
			container.RegisterTypes(AllClasses.FromAssemblies(Assembly.GetExecutingAssembly()),
			type => objectTypeInList, WithName.TypeName);
			//container.RegisterType<HomeController, HomeController>();
			container.RegisterTypes(AllClasses.FromAssemblies(typeof(UnityConfig).Assembly),
			WithMappings.FromMatchingInterface,
			WithName.Default);

			GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
			DependencyResolver.SetResolver(new Microsoft.Practices.Unity.Mvc.UnityDependencyResolver(container)); 
		}
    }
}