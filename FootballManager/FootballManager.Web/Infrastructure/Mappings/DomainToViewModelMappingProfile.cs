using AutoMapper;
using FootballManager.BusinessLayer.Models;
using FootballManager.Domain.Entity.Models;
using FootballManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Infrastructure.Mappings
{
	public class DomainToViewModelMappingProfile : Profile
	{
		public override string ProfileName
		{
			get { return "DomainToViewModelMappings"; }
		}

		public DomainToViewModelMappingProfile()
		{
			this.CreateMap<Referee, RefereesViewModel>()
				.ForMember(vm => vm.Id, map => map.MapFrom(f => f.Id))
				.ForMember(vm => vm.FirstName, map => map.MapFrom(f => f.FirstName))
				.ForMember(vm => vm.PictureUrl, map => map.MapFrom(f => f.PictureUrl))
				.ForMember(vm => vm.Surname, map => map.MapFrom(f => f.Surname))
				.ForMember(vm => vm.BirthDate, map => map.MapFrom(f => f.BirthDate))
				.ForMember(vm => vm.Matches, map => map.MapFrom(f => f.Matches));

			this.CreateMap<Coach, CoachesViewModel>()
				.ForMember(vm => vm.Id, map => map.MapFrom(f => f.Id))
				.ForMember(vm => vm.FirstName, map => map.MapFrom(f => f.FirstName))
				.ForMember(vm => vm.Surname, map => map.MapFrom(f => f.Surname))
				.ForMember(vm => vm.BirthDate, map => map.MapFrom(f => f.BirthDate))
				.ForMember(vm => vm.PictureUrl, map => map.MapFrom(f => f.PictureUrl))
				.ForMember(vm => vm.Teams, map => map.MapFrom(f => f.Teams));

			this.CreateMap<Address, AddressViewModel>()
				.ForMember(vm => vm.Id, map => map.MapFrom(f => f.Id))
				.ForMember(vm => vm.City, map => map.MapFrom(f => f.City))
				.ForMember(vm => vm.Street, map => map.MapFrom(f => f.Street))
				.ForMember(vm => vm.Number, map => map.MapFrom(f => f.Number))
				.ForMember(vm => vm.Zipcode, map => map.MapFrom(f => f.Zipcode));

			this.CreateMap<Team, SelectItem>()
				.ForMember(vm => vm.Id, map => map.MapFrom(s => s.Id))
				.ForMember(vm => vm.Name, map => map.MapFrom(s => s.Name));

			this.CreateMap<Stadium, StadiumViewModel>()
				.ForMember(vm => vm.Id, map => map.MapFrom(f => f.Id))
				.ForMember(vm => vm.Address, map => map.MapFrom(f => f.Address))
				.ForMember(vm => vm.Name, map => map.MapFrom(f => f.Name))
				.ForMember(vm => vm.Capacity, map => map.MapFrom(f => f.Capacity))
				.ForMember(vm => vm.Teams, map => map.MapFrom(f => f.Teams));

			//this.CreateMap<Team, TeamViewModel>()
			//	.ForMember(vm => vm.Id, map => map.MapFrom(f => f.Id))
			//	.ForMember(vm => vm.Address, map => map.MapFrom(f => f.Address))
			//	.ForMember(vm => vm.Coach, map => map.MapFrom(f => f.Coach))
			//	.ForMember(vm => vm.Footballers, map => map.MapFrom(f => f.Footballers))
			//	.ForMember(vm => vm.Name, map => map.MapFrom(f => f.Name))
			//	.ForMember(vm => vm.Matches, map => map.MapFrom(f => f.Matches))
			//	.ForMember(vm => vm.Stadium, map => map.MapFrom(f => f.Stadium))
			//	.ForMember(vm => vm.Founded, map => map.MapFrom(f => f.Founded));

			this.CreateMap<Season, KeyValuePair<int, string>>()
				.ForMember(vm => vm.Key, map => map.MapFrom(s => s.Id))
				.ForMember(vm => vm.Value, map => map.MapFrom(s => s.Name));

			this.CreateMap<Table, TableViewModel>()
				.ForMember(vm => vm.Id, map => map.MapFrom(t => t.TeamId))
				.ForMember(vm => vm.Name, map => map.MapFrom(t => t.Team.Name))
				.ForMember(vm => vm.GoalsScored, map => map.MapFrom(t => t.GoalsScored))
				.ForMember(vm => vm.GoalsConceded, map => map.MapFrom(t => t.GoalsConceded))
				.ForMember(vm => vm.Points, map => map.MapFrom(t => t.Points));

			this.CreateMap<Season, SeasonViewModel>()
				.ForMember(vm => vm.Id, map => map.MapFrom(s => s.Id))
				.ForMember(vm => vm.Tables, map => map.MapFrom(s =>(s.Table)));

			this.CreateMap<Footballer, ScorersViewModel>()
				.ForMember(vm => vm.Id, map => map.MapFrom(f => f.Id))
				.ForMember(vm => vm.Name, map => map.MapFrom(f => f.FirstName + " " + f.Surname))
				.ForMember(vm => vm.Team, map => map.MapFrom(f => f.Team.Name))
				.ForMember(vm => vm.PictureUrl, map => map.MapFrom(f => f.PictureUrl));


			this.CreateMap<KeyValuePair<Footballer, int>, KeyValuePair<ScorersViewModel, int>>()
				.ForMember(vm => vm.Key, map => map.MapFrom(x => Mapper.Map<ScorersViewModel>(x.Key)))
				.ForMember(vm => vm.Value, map => map.MapFrom(x => x.Value));

			this.CreateMap<Footballer, TeamFootballerViewModel>()
				.ForMember(vm => vm.Id, map => map.MapFrom(x => x.Id))
				.ForMember(vm => vm.FirstName, map => map.MapFrom(x => x.FirstName))
				.ForMember(vm => vm.Surname, map => map.MapFrom(x => x.Surname))
				.ForMember(vm => vm.BirthDate, map => map.MapFrom(x => x.BirthDate));

			//this.CreateMap<Table, KeyValuePair<int, string>>()
			//	.ForMember(vm => vm.Key, map => map.MapFrom(x => x.Season.Id))
			//	.ForMember(vm => vm.Value, map => map.MapFrom(x => x.Season.Name));
			this.CreateMap<Stadium, KeyValuePair<int, string>>()
				.ForMember(vm => vm.Key, map => map.MapFrom(s => s.Id))
				.ForMember(vm => vm.Value, map => map.MapFrom(s => s.Name));

			this.CreateMap<Coach, KeyValuePair<int, string>>()
				.ForMember(vm => vm.Key, map => map.MapFrom(s => s.Id))
				.ForMember(vm => vm.Value, map => map.MapFrom(s => s.FirstName + " " + s.Surname));

			this.CreateMap<Team, TeamViewModel>()
				.ForMember(vm => vm.Id, map => map.MapFrom(x => x.Id))
				.ForMember(vm => vm.Players, map => map.MapFrom(x => x.Footballers))
				.ForMember(vm => vm.Stadium, map => map.MapFrom(x => x.Stadium));

			this.CreateMap<Goal, GoalViewModel>()
				.ForMember(vm => vm.Id, map => map.MapFrom(x => x.Id))
				.ForMember(vm => vm.HomeTeam, map => map.MapFrom(x => x.Match.HomeTeam.Name))
				.ForMember(vm => vm.AwayTeam, map => map.MapFrom(x => x.Match.AwayTeam.Name))
				.ForMember(vm => vm.Type, map => map.MapFrom(x => x.Type.ToString()))
				.ForMember(vm => vm.Footballer, map => map.MapFrom(x => x.Footballer))
				.ForMember(vm => vm.Time, map => map.MapFrom(x => x.Time));

			this.CreateMap<Team, KeyValuePair<int, string>>()
				.ForMember(vm => vm.Key, map => map.MapFrom(x => x.Id))
				.ForMember(vm => vm.Value, map => map.MapFrom(x => x.Name));

			this.CreateMap<Footballer, FootballerViewModel>()
				.ForMember(vm => vm.Id, map => map.MapFrom(x => x.Id))
				.ForMember(vm => vm.FirstName, map => map.MapFrom(x => x.FirstName))
				.ForMember(vm => vm.Surname, map => map.MapFrom(x => x.Surname))
				.ForMember(vm => vm.PictureUrl, map => map.MapFrom(x => x.PictureUrl))
				.ForMember(vm => vm.BirthDate, map => map.MapFrom(x => x.BirthDate))
				.ForMember(vm => vm.Goals, map => map.MapFrom(x => x.Goals));

			this.CreateMap<Match, MatchViewModel>()
				.ForMember(vm => vm.Id, map => map.MapFrom(m => m.Id))
				.ForMember(vm => vm.HomeTeamId, map => map.MapFrom(m => m.HomeTeamId))
				.ForMember(vm => vm.AwayTeamId, map => map.MapFrom(m => m.AwayTeamId))
				.ForMember(vm => vm.HomeTeamName, map => map.MapFrom(m => m.HomeTeam.Name))
				.ForMember(vm => vm.AwayTeamName, map => map.MapFrom(m => m.AwayTeam.Name))
				.ForMember(vm => vm.HomeTeamLogoUrl, map => map.MapFrom(m => m.HomeTeam.LogoUrl))
				.ForMember(vm => vm.AwayTeamLogoUrl, map => map.MapFrom(m => m.AwayTeam.LogoUrl))
				.ForMember(vm => vm.HomeGoalsCount, map => map.MapFrom(m => m.HomeGoals))
				.ForMember(vm => vm.AwayGoalsCount, map => map.MapFrom(m => m.AwayGoals))
				.ForMember(vm => vm.Referee, map => map.MapFrom(m => m.Referee.FirstName + " " + m.Referee.Surname))
				.ForMember(vm => vm.RefereeId, map => map.MapFrom(m => m.RefereeId))
				.ForMember(vm => vm.Date, map => map.MapFrom(m => m.Date))
				.ForMember(vm => vm.Stadium, map => map.MapFrom(m => m.Stadium))
				.ForMember(vm => vm.HomeGoals, map => map.MapFrom(m => m.Goals.Where(x => x.TeamID == m.HomeTeamId)))
				.ForMember(vm => vm.AwayGoals, map => map.MapFrom(m => m.Goals.Where(x => x.TeamID == m.AwayTeamId)))
				.ForMember(vm => vm.AverageRating, map => map.MapFrom(m =>(int) Math.Round(m.Comments != null && m.Comments.Count != 0 ? m.Comments.Average(x => x.Rating) : 0)));

			this.CreateMap<Notification, NotificationViewModel>();
		}
	}
}