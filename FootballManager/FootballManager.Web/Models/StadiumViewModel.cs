using FootballManager.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class StadiumViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Capacity { get; set; }
		public string PictureUrl { get; set; }
		public AddressViewModel Address { get; set; }
		public ICollection<SelectItem> Teams { get; set; }
	}
}