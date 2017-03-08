using FootballManager.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FootballManager.Fans.Presentation.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILeagueService leagueService;

		public HomeController(ILeagueService leagueService) : base()
		{
			this.leagueService = leagueService;
		}

		public ActionResult Index()
		{
			ViewBag.Title = "Home Page";

			return View();
		}
	}
}
