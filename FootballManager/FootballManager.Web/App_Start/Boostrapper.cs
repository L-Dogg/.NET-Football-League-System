using FootballManager.Web.Infrastructure.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FootballManager.Web.App_Start
{
	public class Bootstrapper
	{
		public static void Run()
		{
			// Configure Autofac
			AutofacWebapiConfig.Initialize(GlobalConfiguration.Configuration);
			//Configure AutoMapper
			AutoMapperConfiguration.Configure();
		}
	}
}