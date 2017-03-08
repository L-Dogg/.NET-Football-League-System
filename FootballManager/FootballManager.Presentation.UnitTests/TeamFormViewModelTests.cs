using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FootballManager.Presentation.ViewModels;
using FootballManager.BusinessLayer;
using Moq;
using Prism.Regions;
using FluentAssertions;
using System.Collections.Generic;
using FootballManager.Domain.Entity.Models;
using FootballManager.Presentation.Infrastructure;
using System.Windows.Input;
using FootballManager.BusinessLayer.Models;

namespace FootballManager.Presentation.UnitTests
{
	[TestClass]
	public class TeamFormViewModelTests
	{
		private TeamFormViewModel viewModel;
		private Mock<ILeagueService> businessLayer;
		private Mock<IRegionManager> regionManager;
		private Mock<IViewToDataService> viewToDataService;
		private NavigationContext navigationContext;
		private List<SelectItem> CoachList = new List<SelectItem>();
		private List<SelectItem> StadiumsList = new List<SelectItem>();
		[TestInitialize]
		public void Initialize()
		{
			this.businessLayer = new Mock<ILeagueService>();
			this.regionManager = new Mock<IRegionManager>();
			this.viewToDataService = new Mock<IViewToDataService>();
			this.businessLayer.Setup(x => x.GetCoachList()).ReturnsAsync(this.CoachList);
			this.businessLayer.Setup(x => x.GetStadiumsList()).ReturnsAsync(this.StadiumsList);
			this.viewModel = new TeamFormViewModel(this.regionManager.Object, this.viewToDataService.Object, this.businessLayer.Object, new Mock<IInteractionService>().Object);
			this.navigationContext = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("http://www.test.com"));
			this.businessLayer.Setup(x => x.GetTeam(It.IsAny<int>())).ReturnsAsync(new Team());
		}

		[TestMethod]
		public void navigate_to_team_with_params()
		{
			navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.Team.Should().NotBeNull();
			this.viewModel.OKCommand.Should().NotBeNull();
			this.businessLayer.Verify(x => x.GetTeam(It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		public void fire_okcommand_with_params()
		{
			navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.SelectedCoach = new SelectItem() { Id = 0, Name = "Name" };
			this.viewModel.SelectedStadium = new SelectItem() { Id = 0 };
			this.viewModel.OKCommand.Execute(null);
			this.businessLayer.Verify(x => x.SaveChangesAsync(), Times.Once);
		}


		[TestMethod]
		public void navigate_to_team_without_params()
		{
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.Team.Should().NotBeNull();
			this.viewModel.OKCommand.Should().NotBeNull();
			this.businessLayer.Verify(x => x.GetTeam(It.IsAny<int>()), Times.Never);
		}

		[TestMethod]
		public void fire_okcommand_without_params()
		{
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.OKCommand.Execute(null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			this.businessLayer.Verify(x => x.AddTeam(It.IsAny<Team>()), Times.Once);
		}

		[TestMethod]
		public void team_footballer_double_click()
		{
			this.viewToDataService.Setup(x => x.DataGridToObject(It.IsAny<object>(), It.IsAny<MouseEventArgs>())).Returns(new Footballer());
			this.viewModel.MouseDoubleClick(null, null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NavigationParameters>()), Times.Once);
		}

		[TestMethod]
		public void team_season_double_click()
		{
			this.viewToDataService.Setup(x => x.DataGridToObject(It.IsAny<object>(), It.IsAny<MouseEventArgs>())).Returns(new Table());
			this.viewModel.SeasonMouseDoubleClick(null, null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NavigationParameters>()), Times.Once);
		}
	}
}
