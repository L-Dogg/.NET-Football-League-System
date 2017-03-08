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
	public class TeamToSeasonViewModelTests
	{
		private TeamToSeasonFormViewModel viewModel;
		private Mock<ILeagueService> businessLayer;
		private Mock<IRegionManager> regionManager;
		private NavigationContext navigationContext;
		private Mock<IInteractionService> interactionService;
		private List<SelectItem> TeamsList = new List<SelectItem>();

		[TestInitialize]
		public void Initialize()
		{
			this.businessLayer = new Mock<ILeagueService>();
			this.regionManager = new Mock<IRegionManager>();
			this.interactionService = new Mock<IInteractionService>();
			this.businessLayer.Setup(x => x.GetTeamsList()).ReturnsAsync(this.TeamsList);
			this.TeamsList.Add(new SelectItem() { Id = 0, Name = "name" });
			this.viewModel = new TeamToSeasonFormViewModel(this.regionManager.Object, this.interactionService.Object, this.businessLayer.Object);
			this.viewModel.SelectedItem = TeamsList.First();
			this.navigationContext = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("http://www.test.com"));
			this.navigationContext.Parameters.Add("id", 0);
			this.viewModel.OnNavigatedTo(this.navigationContext);
		}

		[TestMethod]
		public void assign_team_to_season()
		{
			this.businessLayer.Setup(x => x.AssignTeamToSeason(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
			this.viewModel.OKCommand.Execute(null);
			this.businessLayer.Verify(x => x.AssignTeamToSeason(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
		}
	}
}
