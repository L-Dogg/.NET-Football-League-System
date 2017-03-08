using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class ChangePasswordViewModel
	{
		public int  Id { get; set; }
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }
	}
}