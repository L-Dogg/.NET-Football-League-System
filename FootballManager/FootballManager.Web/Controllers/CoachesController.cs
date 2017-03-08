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
	[RoutePrefix("api/coaches")]
	public class CoachesController : ApiControllerBase
	{
		private readonly ILeagueService leagueService;

		public CoachesController() { }

		public CoachesController(ILeagueService leagueService)
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
				var coaches = await this.leagueService.GetFilteredCoachesList(filter);
				response = request.CreateResponse<IEnumerable<SelectItem>>(HttpStatusCode.OK, coaches);

				return response;
			});
		}

		[Route("{id:int}")]
		public async Task<HttpResponseMessage> GetCoach(HttpRequestMessage request, int id)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;
				var coach = await this.leagueService.GetCoach(id);
				var coachVm = Mapper.Map<Coach, CoachesViewModel>(coach);

				var zipped = coach.Teams.Zip(coachVm.Teams, (m, vm) => new {Model = m, ViewModel = vm});
				foreach (var zip in zipped)
				{
					zip.ViewModel.Coach = new KeyValuePair<int, string>(zip.Model.Coach.Id, 
						$"{zip.Model.Coach.FirstName} {zip.Model.Coach.Surname}");
					zip.ViewModel.Stadium = new KeyValuePair<int, string>(zip.Model.Stadium.Id, zip.Model.Stadium.Name);
				}

				response = request.CreateResponse(HttpStatusCode.OK, coachVm);

				return response;
			});
		}
	}
}