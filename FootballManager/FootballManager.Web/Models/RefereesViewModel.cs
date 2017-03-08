using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class RefereesViewModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }
		public string PictureUrl { get; set; }
		public List<MatchViewModel> Matches { get; set; }
	}
}