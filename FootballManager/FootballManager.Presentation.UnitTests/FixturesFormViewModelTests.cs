using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FootballManager.Presentation.ViewModels;
using Moq;
using FootballManager.BusinessLayer;
using Prism.Regions;
using FootballManager.Presentation.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using FootballManager.BusinessLayer.Models;

namespace FootballManager.Presentation.UnitTests
{
	[TestClass]
	public class FixturesFormViewModelTests
	{
		private FixturesFormViewModel viewModel;
		private Mock<ILeagueService> businessLayer;
		private Mock<IRegionManager> regionManager;
		private NavigationContext navigationContext;
		private Mock<IInteractionService> interactionService;
		private List<SelectItem> SeasonsList = new List<SelectItem>();

		[TestInitialize]
		public void Initialize()
		{
			this.businessLayer = new Mock<ILeagueService>();
			this.regionManager = new Mock<IRegionManager>();
			this.interactionService = new Mock<IInteractionService>();
			this.businessLayer.Setup(x => x.GetSeasonsList()).ReturnsAsync(this.SeasonsList);
			this.SeasonsList.Add(new SelectItem() { Id = 0, Name = "name" });
			this.viewModel = new FixturesFormViewModel(this.regionManager.Object, this.interactionService.Object,  this.businessLayer.Object);
			this.viewModel.SelectedItem = SeasonsList.First();
			this.navigationContext = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("http://www.test.com"));
			this.businessLayer.Setup(x => x.GetCoach(It.IsAny<int>())).ReturnsAsync(new Domain.Entity.Models.Coach());
		}

		[TestMethod]
		public void generate_valid_fixture()
		{
			this.businessLayer.Setup(x => x.GenerateFixtures(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(true);
			this.viewModel.OKCommand.Execute(null);
			this.businessLayer.Verify(x => x.GenerateFixtures(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
		}

		[TestMethod]
		public void generate_invalid_fixture_and_show_confirmation()
		{
			this.businessLayer.Setup(x => x.GenerateFixtures(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(false);
			this.viewModel.OKCommand.Execute(null);
			this.businessLayer.Verify(x => x.GenerateFixtures(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
			this.interactionService.Verify(x => x.ShowConfirmationMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action>()), Times.Once);
		}
	}
}
