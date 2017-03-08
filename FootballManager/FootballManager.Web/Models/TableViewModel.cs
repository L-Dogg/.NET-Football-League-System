using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class TableViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int GoalsScored { get; set; }
		public int GoalsConceded { get; set; }
		public int Points { get; set; }
	}
}