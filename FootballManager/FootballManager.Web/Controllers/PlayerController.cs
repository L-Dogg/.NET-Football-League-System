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
	[RoutePrefix("api/player")]
	public class PlayerController : ApiControllerBase
	{
		private readonly ILeagueService leagueService;


		public PlayerController(ILeagueService leagueService)
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
				var players = await this.leagueService.GetFilteredFootballersList(filter);
				response = request.CreateResponse<IEnumerable<SelectItem>>(HttpStatusCode.OK, players);

				return response;
			});
		}

		[Route("{id:int}")]
		public async Task<HttpResponseMessage> GetPlayer(HttpRequestMessage request, int id)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var player = await this.leagueService.GetFootballer(id);
				var playerVm = Mapper.Map<FootballerViewModel>(player);
				playerVm.Team = new KeyValuePair<int, string>(player.Team.Id, player.Team.Name);
				response = request.CreateResponse<FootballerViewModel>(HttpStatusCode.OK, playerVm);

				return response;
			});
		}

	}
}