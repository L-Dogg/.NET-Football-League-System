﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FootballManager.Web.Infrastructure.Validators;

namespace FootballManager.Web.Models
{
	public class LoginViewModel : IValidatableObject
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var validator = new LoginViewModelValidator();
			var result = validator.Validate(this);
			return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
		}
	}
}