using AutoMapper;
using FootballManager.BusinessLayer;
using FootballManager.Domain.Entity.Models;
using FootballManager.Web.Infrastructure.core;
using FootballManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FootballManager.Web.Controllers
{
	[Authorize(Roles = "User")]
	[RoutePrefix("api/matches")]
	public class MatchesController : ApiControllerBase
	{
		private readonly ILeagueService leagueService;

		public MatchesController() { }

		public MatchesController(ILeagueService leagueService)
		{
			this.leagueService = leagueService;
		}

		[AllowAnonymous]
		[Route("next")]
		public HttpResponseMessage GetNext(HttpRequestMessage request)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response = null;
				var matches = this.leagueService.GetNextRound();

				IEnumerable<MatchViewModel> matchesVM = Mapper.Map<IEnumerable<Match>, IEnumerable<MatchViewModel>>(matches);

				response = request.CreateResponse<IEnumerable<MatchViewModel>>(HttpStatusCode.OK, matchesVM);

				return response;
			});
		}

		[AllowAnonymous]
		[Route("previous")]
		public HttpResponseMessage GetPrevious(HttpRequestMessage request)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response = null;
				var matches = this.leagueService.GetPreviousRound();

				IEnumerable<MatchViewModel> matchesVM = Mapper.Map<IEnumerable<Match>, IEnumerable<MatchViewModel>>(matches);

				response = request.CreateResponse<IEnumerable<MatchViewModel>>(HttpStatusCode.OK, matchesVM);

				return response;
			});
		}
	}
}