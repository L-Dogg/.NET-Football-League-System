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
	[RoutePrefix("api/team")]
	public class TeamController : ApiControllerBase
	{
		private readonly ILeagueService leagueService;


		public TeamController(ILeagueService leagueService)
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
				var teams = await this.leagueService.GetFilteredTeamsList(filter);
				response = request.CreateResponse<IEnumerable<SelectItem>>(HttpStatusCode.OK, teams);

				return response;
			});
		}

		[Route("{id:int}")]
		public async Task<HttpResponseMessage> GetTeam(HttpRequestMessage request, int id)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var team = await this.leagueService.GetTeam(id);
				var matches = await this.leagueService.GetTeamsMatches(id);
				var teamVm = Mapper.Map<TeamViewModel>(team);
				teamVm.Seasons = new List<KeyValuePair<int, string>>();
				teamVm.Seasons.AddRange(team.Table.Select(x => new KeyValuePair<int, string>(x.Season.Id, x.Season.Name)));
				teamVm.Coach = new KeyValuePair<int, string>(team.CoachId, team.Coach.FirstName + " " + team.Coach.Surname);
				teamVm.Stadium = new KeyValuePair<int, string>(team.Stadium.Id, team.Stadium.Name);
				teamVm.Matches = Mapper.Map<ICollection<MatchViewModel>>(matches);
				response = request.CreateResponse<TeamViewModel>(HttpStatusCode.OK, teamVm);

				return response;
			});
		}

	}
}