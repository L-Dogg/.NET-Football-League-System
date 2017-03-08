using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FootballManager.AuthenticationLayer;
using FootballManager.AuthenticationViewModel;
using FootballManager.Domain.Entity.Models.Authentication;
using FootballManager.Web.Infrastructure.core;
using FootballManager.Web.Models;

namespace FootballManager.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	[RoutePrefix("api/Account")]
	public class AccountController : ApiControllerBase
	{
		private readonly IAuthenticationService _authenticationService;

		public AccountController(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}

		[AllowAnonymous]
		[Route("authenticate")]
		[HttpPost]
		public HttpResponseMessage Login(HttpRequestMessage request, LoginViewModel user)
		{
			return CreateHttpResponse(request, () =>
			{
				HttpResponseMessage response = null;

				if (ModelState.IsValid)
				{
					MembershipContext _userContext = _authenticationService.ValidateUser(user.Username, user.Password);

					if (_userContext.User != null)
					{
						response = request.CreateResponse(HttpStatusCode.OK, new
						{ success = true, id = _userContext.User.Id, isFacebookUser = _userContext.User.IsFacebookUser });
					}
					else
					{
						response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
					}
				}
				else
					response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

				return response;
			});
		}

		[AllowAnonymous]
		[Route("register")]
		[HttpPost]
		public async Task <HttpResponseMessage> Register(HttpRequestMessage request, RegistrationViewModel user)
		{
			return await CreateHttpResponse(request, async () =>
			{
				HttpResponseMessage response = null;

				if (!ModelState.IsValid)
				{
					response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
				}
				else
				{
					User _user = await _authenticationService.CreateUser(user.Username, user.Password, user.IsFacebookUser);

					if (_user != null)
					{
						response = request.CreateResponse(HttpStatusCode.OK, new { success = true, id = _user.Id });
					}
					else
					{
						response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
					}
				}

				return response;
			});
		}

		[AllowAnonymous]
		[Route("change")]
		[HttpPost]
		public async Task<HttpResponseMessage> ResetPassword(HttpRequestMessage request, ChangePasswordViewModel user)
		{
			return  CreateHttpResponse(request,  () =>
			{
				HttpResponseMessage response = null;

				if (!ModelState.IsValid)
				{
					response = request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
				}
				else
				{
					bool success =  _authenticationService.ChangePassword(user.Id, user.OldPassword, user.NewPassword);

					if (success)
					{
						response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
					}
					else
					{
						response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
					}
				}

				return response;
			});
		}
	}
}