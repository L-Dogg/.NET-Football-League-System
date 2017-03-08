using AutoMapper;
using FootballManager.BusinessLayer;
using FootballManager.BusinessLayer.Models;
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
	[RoutePrefix("api/stadium")]
	public class StadiumController : ApiControllerBase
	{
		private readonly ILeagueService leagueService;


		public StadiumController(ILeagueService leagueService)
		{
			this.leagueService = leagueService;
		}

		[AllowAnonymous]
		[Route("list/{filter}")]
		public async Task<HttpResponseMessage> GetFilteredList(HttpRequestMessage request, string filter)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var teams = await this.leagueService.GetFilteredStadiumsList(filter);
				response = request.CreateResponse<IEnumerable<SelectItem>>(HttpStatusCode.OK, teams);

				return response;
			});
		}

		[Route("{id:int}")]
		public async Task<HttpResponseMessage> GetStadium(HttpRequestMessage request, int id)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var stadium = await this.leagueService.GetStadium(id);
				var stadiumVm = Mapper.Map<StadiumViewModel>(stadium);
				response = request.CreateResponse<StadiumViewModel>(HttpStatusCode.OK, stadiumVm);

				return response;
			});
		}

	}
}