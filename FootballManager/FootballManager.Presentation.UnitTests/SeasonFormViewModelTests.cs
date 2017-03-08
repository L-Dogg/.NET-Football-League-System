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

namespace FootballManager.Presentation.UnitTests
{
	[TestClass]
	public class SeasonFormViewModelTests
	{
		private SeasonFormViewModel viewModel;
		private Mock<ILeagueService> businessLayer;
		private Mock<IRegionManager> regionManager;
		private Mock<IViewToDataService> viewToDataService;
		private NavigationContext navigationContext;
		[TestInitialize]
		public void Initialize()
		{
			this.businessLayer = new Mock<ILeagueService>();
			this.regionManager = new Mock<IRegionManager>();
			this.viewToDataService = new Mock<IViewToDataService>();
			this.viewModel = new SeasonFormViewModel(this.regionManager.Object, this.viewToDataService.Object, this.businessLayer.Object);
			this.navigationContext = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("http://www.test.com"));
			this.businessLayer.Setup(x => x.GetSeason(It.IsAny<int>())).ReturnsAsync(new Season());
		}

		[TestMethod]
		public void navigate_to_season_with_params()
		{
			navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.Season.Should().NotBeNull();
			this.viewModel.OKCommand.Should().NotBeNull();
			this.viewModel.AddTeamCommand.Should().NotBeNull();
			this.businessLayer.Verify(x => x.GetSeason(It.IsAny<int>()), Times.Once);
			this.viewModel.AddTeamEnabled.Should().BeTrue();
		}

		[TestMethod]
		public void fire_okcommand_with_params()
		{
			navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.OKCommand.Execute(null);
			this.businessLayer.Verify(x => x.SaveChangesAsync(), Times.Once);
		}


		[TestMethod]
		public void navigate_to_season_without_params()
		{
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.Season.Should().NotBeNull();
			this.viewModel.OKCommand.Should().NotBeNull();
			this.viewModel.AddTeamCommand.Should().BeNull();
			this.businessLayer.Verify(x => x.GetSeason(It.IsAny<int>()), Times.Never);
		}

		[TestMethod]
		public void fire_okcommand_without_params()
		{
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.OKCommand.Execute(null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			this.businessLayer.Verify(x => x.AddSeason(It.IsAny<Season>()), Times.Once);
			this.viewModel.AddTeamEnabled.Should().BeFalse();
		}

		[TestMethod]
		public void fire_addteamcommand()
		{
			navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.AddTeamCommand.Execute(null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NavigationParameters>()), Times.Once);
		}

		[TestMethod]
		public void season_team_double_click()
		{
			this.viewToDataService.Setup(x => x.DataGridToObject(It.IsAny<object>(), It.IsAny<MouseEventArgs>())).Returns(new Domain.Entity.Models.Table());
			this.viewModel.MouseDoubleClick(null, null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NavigationParameters>()), Times.Once);
		}

		[TestMethod]
		public void season_match_double_click()
		{
			this.viewToDataService.Setup(x => x.DataGridToObject(It.IsAny<object>(), It.IsAny<MouseEventArgs>())).Returns(new Domain.Entity.Models.Match());
			this.viewModel.MatchMouseDoubleClick(null, null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NavigationParameters>()), Times.Once);
		}
	}
}
