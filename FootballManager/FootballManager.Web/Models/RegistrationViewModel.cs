﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FootballManager.Web.Infrastructure.Validators;

namespace FootballManager.Web.Models
{
	public class RegistrationViewModel : IValidatableObject
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public bool IsFacebookUser { get; set; } = false;

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var validator = new RegistrationViewModelValidator();
			var result = validator.Validate(this);
			return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
		}
	}
}