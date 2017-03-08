﻿using FootballManager.Domain.Entity.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.AuthenticationViewModel
{
	public class MembershipContext
	{
		public IPrincipal Principal { get; set; }
		public User User { get; set; }
		public bool IsValid()
		{
			return Principal != null;
		}
	}
}
