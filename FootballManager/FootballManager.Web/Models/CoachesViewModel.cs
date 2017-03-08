using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class CoachesViewModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }
		public string PictureUrl { get; set; }
		public ICollection<TeamViewModel> Teams { get; set; }
	}
}