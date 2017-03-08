using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FootballManager.AuthenticationLayer;
using FootballManager.AuthenticationViewModel;
using FootballManager.BusinessLayer;
using FootballManager.Domain.Entity.Models;
using FootballManager.Domain.Entity.Models.Authentication;
using FootballManager.Web.Controllers;
using FootballManager.Web.Infrastructure.Mappings;
using FootballManager.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FootballManager.Web.Tests
{
	[TestClass]
	public class AccountControllerTests
	{
		private AccountController controller;
		private Mock<IAuthenticationService> service;
		private HttpRequestMessage request;

		[TestInitialize]
		public void Initialize()
		{
			this.service = new Mock<IAuthenticationService>();
			this.controller = new AccountController(service.Object);
			this.request = new HttpRequestMessage();
			request.SetConfiguration(new System.Web.Http.HttpConfiguration());
			Mapper.Initialize(x =>
			{
				x.AddProfile<DomainToViewModelMappingProfile>();
			});
		}

		[TestMethod]
		public void SuccessfullLogin()
		{
			this.service.Setup(x => x.ValidateUser(It.IsAny<string>(), It.IsAny<string>())).
				Returns(new MembershipContext() { User = new User() { Id = 1, Username = "admin", IsFacebookUser = true } });
			var result = this.controller.Login(request, new LoginViewModel() { Id = 1, Username = "admin", Password = "password"});
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			object content;
			result.TryGetContentValue(out content);
			var success = (bool)content.GetType().GetProperty("success").GetValue(content);
			success.Should().BeTrue();
		}

		[TestMethod]
		public void UnsuccessfullLogin()
		{
			this.service.Setup(x => x.ValidateUser(It.IsAny<string>(), It.IsAny<string>())).
				Returns(new MembershipContext());
			var result = this.controller.Login(request, new LoginViewModel() { Id = 21, Username = "piotr", Password = "zyla" });
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			object content;
			result.TryGetContentValue(out content);
			var success = (bool)content.GetType().GetProperty("success").GetValue(content);
			success.Should().BeFalse();
		}

		[TestMethod]
		public async Task SuccessfullRegister()
		{
			this.service.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).
				ReturnsAsync(new User() {Id = 1, Username = "admin"});
			var result = await this.controller.Register(request, new RegistrationViewModel() {Password = "admin", Username = "admin"});
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			object content;
			result.TryGetContentValue(out content);
			var success = (bool)content.GetType().GetProperty("success").GetValue(content);
			var id = (int)content.GetType().GetProperty("id").GetValue(content);
			success.Should().BeTrue();
			id.Should().Be(1);
		}

		[TestMethod]
		public async Task UnsuccessfullRegister()
		{
			this.service.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).
				ThrowsAsync(new Exception());
			var result = await this.controller.Register(request, new RegistrationViewModel()
				{ Password = "piotr", Username = "zyla", IsFacebookUser = false});
			result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
		}

		[TestMethod]
		public async Task UnsuccessfullFacebookRegister()
		{
			this.service.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).
				ReturnsAsync(null);
			var result = await this.controller.Register(request, new RegistrationViewModel() { Password = "piotr", Username = "zyla", IsFacebookUser = false });
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			object content;
			result.TryGetContentValue(out content);
			var success = (bool)content.GetType().GetProperty("success").GetValue(content);
			success.Should().BeFalse();
		}

		[TestMethod]
		public async Task SuccessfullPasswordChange()
		{
			this.service.Setup(x => x.ChangePassword(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).
				Returns(() => true);
			var result = await this.controller.ResetPassword(request, new ChangePasswordViewModel()
				{ OldPassword = "piotr", NewPassword = "zyla", Id = 1 });
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			object content;
			result.TryGetContentValue(out content);
			var success = (bool)content.GetType().GetProperty("success").GetValue(content);
			success.Should().BeTrue();
		}

		[TestMethod]
		public async Task UnsuccessfullPasswordChange()
		{
			this.service.Setup(x => x.ChangePassword(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).
				Returns(() => false);
			var result = await this.controller.ResetPassword(request, new ChangePasswordViewModel()
			{ OldPassword = "piotr", NewPassword = "zyla", Id = 1 });
			result.StatusCode.Should().Be(HttpStatusCode.OK);
			object content;
			result.TryGetContentValue(out content);
			var success = (bool)content.GetType().GetProperty("success").GetValue(content);
			success.Should().BeFalse();
		}
	}
}