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
	[RoutePrefix("api/season")]
	public class SeasonController : ApiControllerBase
	{
		private readonly ILeagueService leagueService;


		public SeasonController(ILeagueService leagueService)
		{
			this.leagueService = leagueService;
		}


		[AllowAnonymous]
		[Route("list/{filter}")]
		public async Task<HttpResponseMessage> GetList(HttpRequestMessage request, string filter)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var seasons = await this.leagueService.GetFilteredSeasonsList(filter);
				response = request.CreateResponse<IEnumerable<SelectItem>>(HttpStatusCode.OK, seasons);

				return response;
			});
		}

		public async Task<HttpResponseMessage> GetSeason(HttpRequestMessage request, int id)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var season = await this.leagueService.GetSeason(id);
				var seasonVm = Mapper.Map<Season, SeasonViewModel>(season);
				seasonVm.Schedule = new List<List<MatchViewModel>>();
				var groupedSchedule = season.Matches.GroupBy(x => x.Round);
				foreach(var round in groupedSchedule.OrderBy(x => x.Key))
				{
					var list = round.ToList();
					seasonVm.Schedule.Add(Mapper.Map<List<Match>, List<MatchViewModel>>(list));
				}
				seasonVm.Tables = seasonVm.Tables.OrderByDescending(x => x.Points).ToList();
				var scorers = await this.leagueService.GetScorersTable(id, 20);
				seasonVm.ScorersTable = new List<KeyValuePair<ScorersViewModel, int>>();
				seasonVm.ScorersTable.AddRange(scorers.Select(x => new KeyValuePair<ScorersViewModel, int>(Mapper.Map<ScorersViewModel>(x.Key), x.Value)));
				response = request.CreateResponse<SeasonViewModel>(HttpStatusCode.OK, seasonVm);
				return response;
			});
		}
	}
}