using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class FootballerViewModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string Surname { get; set; }
		public string PictureUrl { get; set; }
		public DateTime BirthDate { get; set; }
		public KeyValuePair<int,string> Team { get; set; }
		public ICollection<GoalViewModel> Goals { get; set; }
	}
}