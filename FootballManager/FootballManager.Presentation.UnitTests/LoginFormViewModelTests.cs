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
using FootballManager.AuthenticationLayer;
using System.Threading.Tasks;
using FootballManager.Domain.Entity.Models.Authentication.Enums;

namespace FootballManager.Presentation.UnitTests
{
	[TestClass]
	public class LoginFormViewModelTests
	{
		private LoginFormViewModel viewModel;
		private Mock<IAuthenticationService> authentication;
		private Mock<IRegionManager> regionManager;
		private Mock<IInteractionService> interactionService;
		private List<SelectItem> TeamsList = new List<SelectItem>();

		[TestInitialize]
		public void Initialize()
		{
			this.authentication = new Mock<IAuthenticationService>();
			this.regionManager = new Mock<IRegionManager>();
			this.interactionService = new Mock<IInteractionService>();
			this.viewModel = new LoginFormViewModel(this.regionManager.Object, this.interactionService.Object, this.authentication.Object);
		}

		[TestMethod]
		public void successful_login()
		{
			this.authentication.Setup(x => x.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>(), UserType.Admin)).Returns(1);
			this.viewModel.PerformLogin("password");
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
		}

		[TestMethod]
		public void failed_login()
		{
			this.authentication.Setup(x => x.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>(), UserType.Admin)).Returns(-1);
			this.viewModel.PerformLogin("password");
			this.regionManager.Verify(x => x.RequestNavigate(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
			this.interactionService.Verify(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
		}
	}
}
