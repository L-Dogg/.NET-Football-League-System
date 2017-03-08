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
	public class FootballerFormViewModelTests
	{
		private FootballerFormViewModel viewModel;
		private Mock<ILeagueService> businessLayer;
		private Mock<IRegionManager> regionManager;
		private Mock<IInteractionService> interactionService;
		private NavigationContext navigationContext;
		private List<SelectItem> TeamsList = new List<SelectItem>();
		[TestInitialize]
		public void Initialize()
		{
			this.businessLayer = new Mock<ILeagueService>();
			this.regionManager = new Mock<IRegionManager>();
			this.interactionService = new Mock<IInteractionService>();
			this.businessLayer.Setup(x => x.GetTeamsList()).ReturnsAsync(this.TeamsList);
			this.viewModel = new FootballerFormViewModel(this.regionManager.Object, this.interactionService.Object, this.businessLayer.Object);
			this.navigationContext = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("http://www.test.com"));
			this.businessLayer.Setup(x => x.GetFootballer(It.IsAny<int>())).ReturnsAsync(new Footballer());
		}

		[TestMethod]
		public void navigate_to_footballer_with_params()
		{
			navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.Footballer.Should().NotBeNull();
			this.viewModel.OKCommand.Should().NotBeNull();
			this.viewModel.ClearTeamCommand.Should().NotBeNull();
			this.businessLayer.Verify(x => x.GetFootballer(It.IsAny<int>()), Times.Once);
		}

		[TestMethod]
		public void footballer_fire_okcommand_with_wrong_params()
		{
			navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.businessLayer.Setup(x => x.CanAddFootballer(It.IsAny<Footballer>())).ReturnsAsync(false);
			this.viewModel.OKCommand.Execute(null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
			this.businessLayer.Verify(x => x.SaveChangesAsync(), Times.Never);
			this.interactionService.Verify(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
		}

		[TestMethod]
		public void footballer_fire_okcommand_with_correct_params()
		{
			navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.SelectedTeam = new SelectItem() { Id = 0 };
			this.businessLayer.Setup(x => x.CanAddFootballer(It.IsAny<Footballer>())).ReturnsAsync(true);
			this.viewModel.OKCommand.Execute(null);
			this.businessLayer.Verify(x => x.SaveChangesAsync(), Times.Once);
			this.interactionService.Verify(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
		}

		[TestMethod]
		public void footballer_fire_clear_team()
		{
			this.viewModel.Footballer = new Footballer() { TeamId = 1 };
			this.viewModel.ClearTeamCommand.Execute(null);
			this.viewModel.Footballer.TeamId.Should().Be(null);
		}

		[TestMethod]
		public void navigate_to_coach_without_params()
		{
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.Footballer.Should().NotBeNull();
			this.viewModel.OKCommand.Should().NotBeNull();
			this.viewModel.ClearTeamCommand.Should().NotBeNull();
			this.businessLayer.Verify(x => x.GetFootballer(It.IsAny<int>()), Times.Never);
		}

		[TestMethod]
		public void fire_okcommand_without_params()
		{
			this.viewModel.OnNavigatedTo(navigationContext);
			this.businessLayer.Setup(x => x.AddFootballer(It.IsAny<Footballer>())).ReturnsAsync(true);
			this.viewModel.OKCommand.Execute(null);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			this.businessLayer.Verify(x => x.AddFootballer(It.IsAny<Footballer>()), Times.Once);
		}
	}
}
