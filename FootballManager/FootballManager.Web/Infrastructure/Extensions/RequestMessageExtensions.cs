using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Dependencies;
using FootballManager.AuthenticationLayer;

namespace FootballManager.Web.Infrastructure.Extensions
{
	public static class RequestMessageExtensions
	{
		internal static IAuthenticationService GetMembershipService(this HttpRequestMessage request)
		{
			return request.GetService<IAuthenticationService>();
		}

		private static TService GetService<TService>(this HttpRequestMessage request)
		{
			IDependencyScope dependencyScope = request.GetDependencyScope();
			TService service = (TService)dependencyScope.GetService(typeof(TService));

			return service;
		}
	}
}