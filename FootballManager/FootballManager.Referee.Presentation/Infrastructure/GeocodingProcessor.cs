using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FootballManager.Referee.Presentation.Properties;
using Microsoft.Maps.MapControl.WPF;

namespace FootballManager.Referee.Presentation.Infrastructure
{
	public interface IGeocodingService
	{
		Location GetLocationFromStringQuery(string addressQuery);
	}

	public class BingGeocodingService : IGeocodingService
	{
		private XmlDocument Geocode(string addressQuery)
		{
			var geocodeRequest =
				"http://dev.virtualearth.net/REST/v1/Locations/" + addressQuery + "?o=xml&maxResults=1&key=" + Resources.BingApiKey;
			return GetXmlResponse(geocodeRequest);
		}

		private XmlDocument GetXmlResponse(string requestUrl)
		{
			System.Diagnostics.Trace.WriteLine("Request URL (XML): " + requestUrl);
			var request = WebRequest.Create(requestUrl) as HttpWebRequest;
			using (var response = request.GetResponse() as HttpWebResponse)
			{
				if (response.StatusCode != HttpStatusCode.OK)
					throw new Exception($"Server error (HTTP {response.StatusCode}: {response.StatusDescription}).");
				var xmlDoc = new XmlDocument();
				xmlDoc.Load(response.GetResponseStream());
				return xmlDoc;
			}
		}

		private Location XmlToLocation(XmlDocument xmlDoc)
		{
			var t = xmlDoc["Response"]["ResourceSets"]["ResourceSet"]["Resources"]["Location"]["Point"];
			var p = new Location()
			{
				Latitude = double.Parse(t["Latitude"].InnerText, CultureInfo.InvariantCulture),
				Longitude = double.Parse(t["Longitude"].InnerText, CultureInfo.InvariantCulture)
			};

			return p;
		}

		public Location GetLocationFromStringQuery(string addressQuery)
		{
			return XmlToLocation(Geocode(addressQuery));
		}
	}
}
