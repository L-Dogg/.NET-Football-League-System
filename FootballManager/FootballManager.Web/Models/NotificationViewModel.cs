using FootballManager.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class NotificationViewModel
	{
		public int Id { get; set; }
		public string FacebookId { get; set; }
		public SelectItem Team { get; set; }
	}
}