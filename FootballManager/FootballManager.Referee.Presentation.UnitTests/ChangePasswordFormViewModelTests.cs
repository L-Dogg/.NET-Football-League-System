using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManager.Referee.Presentation.Infrastructure;
using FootballManager.Referee.Presentation.RefereeService;
using FootballManager.Referee.Presentation.ViewModels;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prism.Regions;

namespace FootballManager.Referee.Presentation.UnitTests
{
	[TestClass]
	public class ChangePasswordFormViewModelTests
	{
		private ChangePasswordFormViewModel viewModel;
		private Mock<IRefereeService> refService;
		private Mock<IRegionManager> regionManager;
		private Mock<IInteractionService> interactionService;
		private NavigationContext navigationContext;

		[TestInitialize]
		public void TestInitialize()
		{
			this.refService = new Mock<IRefereeService>();
			this.regionManager = new Mock<IRegionManager>();
			this.interactionService = new Mock<IInteractionService>();
			this.viewModel = new ChangePasswordFormViewModel(regionManager.Object, interactionService.Object, refService.Object);
			this.navigationContext = new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("http://www.test.com"));
			this.refService.Setup(x => x.ChangePassword(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
		}

		[TestMethod]
		public void fire_ok_command()
		{
			this.viewModel.ChangePassword("admin", "admin");
			this.refService.Verify(x => x.ChangePasswordAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			this.viewModel.OnNavigatedFrom(navigationContext);
		}
	}
}
