using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballManager.Web.Models
{
	public class AddressViewModel
	{
		public int Id { get; set; }
		public string City { get; set; }
		public string Zipcode { get; set; }
		public string Street { get; set; }
		public string Number { get; set; }
	}
}