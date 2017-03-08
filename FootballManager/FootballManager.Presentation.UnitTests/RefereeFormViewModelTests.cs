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
	public class RefereeFormViewModelTests
	{
		private RefereeFormViewModel viewModel;
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
			this.viewModel = new RefereeFormViewModel(this.regionManager.Object, this.viewToDataService.Object, this.businessLayer.Object);
			this.navigationContext = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("http://www.test.com"));
			this.businessLayer.Setup(x => x.GetReferee(It.IsAny<int>())).ReturnsAsync(new Referee());
		}

		[TestMethod]
		public void navigate_to_referee_with_params()
		{
			navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.Referee.Should().NotBeNull();
			this.viewModel.OKCommand.Should().NotBeNull();
			this.businessLayer.Verify(x => x.GetReferee(It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		public void referee_fire_okcommand_with_params()
		{
			navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.OKCommand.Execute(null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			this.businessLayer.Verify(x => x.SaveChangesAsync(), Times.Once);
		}


		[TestMethod]
		public void navigate_to_referee_without_params()
		{
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.Referee.Should().NotBeNull();
			this.viewModel.OKCommand.Should().NotBeNull();
			this.businessLayer.Verify(x => x.GetReferee(It.IsAny<int>()), Times.Never);
		}

		[TestMethod]
		public void referee_fire_okcommand_without_params()
		{
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.OKCommand.Execute(null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			this.businessLayer.Verify(x => x.AddReferee(It.IsAny<Referee>()), Times.Once);
		}

		[TestMethod]
		public void referee_match_double_click()
		{
			this.viewToDataService.Setup(x => x.DataGridToObject(It.IsAny<object>(), It.IsAny<MouseEventArgs>())).Returns(new Domain.Entity.Models.Match());
			this.viewModel.MatchesMouseDoubleClick(null, null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NavigationParameters>()), Times.Once);
		}
	}
}
