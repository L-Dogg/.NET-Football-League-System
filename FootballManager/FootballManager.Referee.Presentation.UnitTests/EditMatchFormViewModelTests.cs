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
using Microsoft.Maps.MapControl.WPF;

namespace FootballManager.Referee.Presentation.UnitTests
{
	[TestClass]
	public class EditMatchFormViewModelTests
	{
		private EditMatchFormViewModel viewModel;
		private Mock<RefereeService.IRefereeService> refService;
		private Mock<IRegionManager> regionManager;
		private Mock<IInteractionService> interactionService;
		private Mock<IGeocodingService> geocodingService;
		private NavigationContext navigationContext;

		[TestInitialize]
		public void TestInitialize()
		{
			this.refService = new Mock<RefereeService.IRefereeService>();
			this.regionManager = new Mock<IRegionManager>();
			this.interactionService = new Mock<IInteractionService>();
			this.geocodingService = new Mock<IGeocodingService>();
			this.viewModel = new EditMatchFormViewModel(regionManager.Object, refService.Object, interactionService.Object, geocodingService.Object);
			this.navigationContext = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("http://www.test.com"));
			this.refService.Setup(x => x.GetMatchAsync(It.IsAny<int>())).ReturnsAsync(new MatchDTO()
			{
				HomeTeamGoals = new GoalDTO[1] { new GoalDTO() },
				AwayTeamGoals = new GoalDTO[1] { new GoalDTO() },
				HomeTeamPlayers = new PlayerListItem[1] { new PlayerListItem() },
				Stadium = new StadiumDTO() { Address = new AddressDTO() { Street = "Koszykowa", Number = "75", City = "Warszawa", Zipcode = "00-662"} }
			});
			this.viewModel.DirectionsMap = new Map();
		}

		[TestMethod]
		public void navigate_to_noneditable_match()
		{
			this.navigationContext.Parameters.Add("id", 1);
			this.navigationContext.Parameters.Add("isEditing", false);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.refService.Verify(x => x.GetMatchAsync(It.IsAny<int>()), Times.Once);
			this.viewModel.RemoveGoalCommand.Should().NotBeNull();
			this.viewModel.AddGoalCommand.Should().NotBeNull();
			this.viewModel.OkCommand.Should().NotBeNull();
			this.viewModel.IsEditing.Should().Be(false);
		}

		[TestMethod]
		public void navigate_to_editable_match()
		{
			this.navigationContext.Parameters.Add("id", 1);
			this.navigationContext.Parameters.Add("isEditing", true);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.refService.Verify(x => x.GetMatchAsync(It.IsAny<int>()), Times.Once);
			this.viewModel.RemoveGoalCommand.Should().NotBeNull();
			this.viewModel.AddGoalCommand.Should().NotBeNull();
			this.viewModel.OkCommand.Should().NotBeNull();
			this.viewModel.IsEditing.Should().Be(true);
		}

		[TestMethod]
		public void fire_nonediting_okcommand()
		{
			this.navigationContext.Parameters.Add("id", 1);
			this.navigationContext.Parameters.Add("isEditing", false);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.OkCommand.Execute(null);
			this.refService.Verify(x => x.SaveGoals(It.IsAny<MatchDTO>()), Times.Never);
		}

		[TestMethod]
		public void fire_editing_okcommand()
		{
			this.navigationContext.Parameters.Add("id", 1);
			this.navigationContext.Parameters.Add("isEditing", true);
			this.viewModel.OnNavigatedTo(navigationContext);
			this.viewModel.OkCommand.Execute(null);
			this.refService.Verify(x => x.SaveGoalsAsync(It.IsAny<MatchDTO>()), Times.Once);
		}
	}
}
