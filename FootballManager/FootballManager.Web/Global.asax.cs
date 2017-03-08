using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Optimization;
using FootballManager.Web.App_Start;

namespace FootballManager.Web
{
	public class Global : HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			var config = GlobalConfiguration.Configuration;

			AreaRegistration.RegisterAllAreas();
			WebApiConfig.Register(config);
			Bootstrapper.Run();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			GlobalConfiguration.Configuration.EnsureInitialized();
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}