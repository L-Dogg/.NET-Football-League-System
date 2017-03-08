using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class TeamFootballerViewModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }
	}
}