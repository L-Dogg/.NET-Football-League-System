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
	[RoutePrefix("api/referees")]
	public class RefereesController : ApiControllerBase
	{
		private readonly ILeagueService leagueService;

		public RefereesController() { }

		public RefereesController(ILeagueService leagueService)
		{
			this.leagueService = leagueService;
		}

		[AllowAnonymous]
		[Route("list/{filter}")]
		public async Task<HttpResponseMessage> GetFiltered(HttpRequestMessage request, string filter)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var referees = await this.leagueService.GetFilteredRefereeList(filter);
				response = request.CreateResponse<IEnumerable<SelectItem>>(HttpStatusCode.OK, referees);

				return response;
			});
		}

		[Route("{id:int}")]
		public async Task<HttpResponseMessage> GetReferee(HttpRequestMessage request, int id)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var referee = await this.leagueService.GetReferee(id);
				var refereeVm = Mapper.Map<Referee, RefereesViewModel>(referee);
				response = request.CreateResponse<RefereesViewModel>(HttpStatusCode.OK, refereeVm);

				return response;
			});
		}
	}
}