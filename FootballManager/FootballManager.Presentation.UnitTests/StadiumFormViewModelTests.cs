using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FootballManager.Presentation.ViewModels;
using FootballManager.BusinessLayer;
using Moq;
using Prism.Regions;
using FluentAssertions;
using System.Collections.Generic;
using FootballManager.Domain.Entity.Models;


namespace FootballManager.Presentation.UnitTests
{
	[TestClass]
	public class StadiumFormViewModelTests
	{
		private StadiumFormViewModel viewModel;
		private Mock<ILeagueService> businessLayer;
		private Mock<IRegionManager> regionManager;
		private NavigationContext navigationContext;
		[TestInitialize]
		public void Initialize()
		{
			this.businessLayer = new Mock<ILeagueService>();
			this.regionManager = new Mock<IRegionManager>();
			this.viewModel = new StadiumFormViewModel(this.regionManager.Object, this.businessLayer.Object);
			this.navigationContext = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("http://www.test.com"));
			this.businessLayer.Setup(x => x.GetStadium(It.IsAny<int>())).ReturnsAsync(new Stadium());
		}

		[TestMethod]
		public void navigate_to_coach_with_params()
		{
			navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.Stadium.Should().NotBeNull();
			this.viewModel.OKCommand.Should().NotBeNull();
			this.businessLayer.Verify(x => x.GetStadium(It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		public void fire_okcommand_with_params()
		{
			navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.OKCommand.Execute(null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			this.businessLayer.Verify(x => x.SaveChangesAsync(), Times.Once);
		}


		[TestMethod]
		public void navigate_to_coach_without_params()
		{
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.Stadium.Should().NotBeNull();
			this.viewModel.OKCommand.Should().NotBeNull();
			this.businessLayer.Verify(x => x.GetStadium(It.IsAny<int>()), Times.Never);
		}

		[TestMethod]
		public void fire_okcommand_without_params()
		{
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.OKCommand.Execute(null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			this.businessLayer.Verify(x => x.AddStadium(It.IsAny<Stadium>()), Times.Once);
		}
	}
}
