using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using FootballManager.Web.Models;

namespace FootballManager.Web.Infrastructure.Validators
{
	public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>
	{
		public RegistrationViewModelValidator()
		{
			RuleFor(r => r.Username).NotEmpty()
				.WithMessage("Invalid username");

			RuleFor(r => r.Password).NotEmpty()
				.WithMessage("Invalid password");
		}
	}

	public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
	{
		public LoginViewModelValidator()
		{
			RuleFor(r => r.Username).NotEmpty()
				.WithMessage("Invalid username");

			RuleFor(r => r.Password).NotEmpty()
				.WithMessage("Invalid password");
		}
	}
}