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
	[RoutePrefix("api/match")]
	public class MatchController : ApiControllerBase
	{
		private readonly ILeagueService leagueService;


		public MatchController(ILeagueService leagueService)
		{
			this.leagueService = leagueService;
		}

		[Route("{id:int}")]
		public async Task<HttpResponseMessage> GetMatch(HttpRequestMessage request, int id)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var match = await this.leagueService.GetMatch(id);
				var matchVm = Mapper.Map<MatchViewModel>(match);
				response = request.CreateResponse<MatchViewModel>(HttpStatusCode.OK, matchVm);

				return response;
			});
		}

		[Route("saveComment")]
		[HttpPost]
		public async Task<HttpResponseMessage> SaveComment(HttpRequestMessage request, Comment comment)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				await this.leagueService.SaveComment(comment);
				response = request.CreateResponse<Comment>(HttpStatusCode.OK, comment);

				return response;
			});
		}

	}
}