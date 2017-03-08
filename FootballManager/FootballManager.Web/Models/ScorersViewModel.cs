using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class ScorersViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Team { get; set; }
		public string PictureUrl { get; set; }
	}
}