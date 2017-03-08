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
using FootballManager.BusinessLayer.Models;

namespace FootballManager.Presentation.UnitTests
{
	[TestClass]
	public class MatchFormViewModelTests
	{
		private MatchFormViewModel viewModel;
		private Mock<ILeagueService> businessLayer;
		private Mock<IRegionManager> regionManager;
		private NavigationContext navigationContext;

		[TestInitialize]
		public void Initialize()
		{
			this.businessLayer = new Mock<ILeagueService>();
			this.regionManager = new Mock<IRegionManager>();
			this.viewModel = new MatchFormViewModel(this.regionManager.Object, this.businessLayer.Object);
			this.navigationContext = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("http://www.test.com"));
		}

		[TestMethod]
		public void home_team_double_click()
		{
			this.viewModel.HomeTeamMouseClick(null, null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NavigationParameters>()), Times.Once);
		}


		[TestMethod]
		public void away_team_double_click()
		{
			this.viewModel.AwayTeamMouseClick(null, null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NavigationParameters>()), Times.Once);
		}
	}
}
