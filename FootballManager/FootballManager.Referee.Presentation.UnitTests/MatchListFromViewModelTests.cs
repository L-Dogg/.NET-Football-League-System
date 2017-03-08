using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FootballManager.Referee.Presentation.ViewModels;
using RefereeServiceLibrary;
using Moq;
using FootballManager.Referee.Presentation.Infrastructure;
using Prism.Regions;
using FootballManager.BusinessLayer;
using FootballManager.AuthenticationLayer;
using FluentAssertions;
using System.Windows.Input;
using FootballManager.Referee.Presentation.RefereeService;

namespace FootballManager.Referee.Presentation.UnitTests
{
	[TestClass]
	public class MatchListFromViewModelTests
	{
		private MatchListFormViewModel viewModel;
		private Mock<RefereeService.IRefereeService> refService;
		private Mock<IRegionManager> regionManager;
		private Mock<IViewToDataService> viewToDataService;
		private NavigationContext navigationContext;

		[TestInitialize]
		public void TestInitialize()
		{
			this.refService = new Mock<RefereeService.IRefereeService>();
			this.regionManager = new Mock<IRegionManager>();
			this.viewToDataService = new Mock<IViewToDataService>();
			this.viewModel = new MatchListFormViewModel(regionManager.Object, refService.Object, viewToDataService.Object);
			this.navigationContext = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("http://www.test.com"));
		}

		[TestMethod]
		public void navigate_to_matchList()
		{
			this.viewModel.OnNavigatedTo(navigationContext);
			this.refService.Verify(x => x.GetMatchesListAsync(It.IsAny<int>()), Times.Once);
			this.viewModel.Matches.Should().NotBeNull();
		}

		[TestMethod]
		public void matchesList_match_double_click()
		{
			this.viewToDataService.Setup(x => x.DataGridToObject(It.IsAny<object>(), It.IsAny<MouseEventArgs>())).Returns(new MatchListItem() { HomeTeamGoals = 0 });
			this.viewModel.MatchMouseDoubleClick(null, null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NavigationParameters>()), Times.Once);
		}
	}
}
