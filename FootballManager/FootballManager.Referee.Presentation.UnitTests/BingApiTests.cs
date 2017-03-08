using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FootballManager.Referee.Presentation.BingServices;
using FootballManager.Referee.Presentation.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Location = Microsoft.Maps.MapControl.WPF.Location;

namespace FootballManager.Referee.Presentation.UnitTests
{
	[TestClass]
	public class BingApiTests
	{
		private Dictionary<string, Tuple<double, double>> locationDictionary = new Dictionary<string, Tuple<double, double>>();
		private readonly IGeocodingService geocodingService = new BingGeocodingService();
		private readonly double epsilon = 0.01;

		[TestInitialize]
		public void TestInitalize()
		{
			locationDictionary.Add("Warsaw, Poland", new Tuple<double, double>(52.232222, 21.008333));
			locationDictionary.Add("Krakow, Poland", new Tuple<double, double>(50.061389, 19.938333));
		}

		[TestMethod]
		public void TestCoordinates()
		{
			foreach (var location in locationDictionary)
			{
				CheckCoords(geocodingService.GetLocationFromStringQuery(location.Key), location.Value)
					.Should()
					.BeTrue("service should return proper geographical data.");

			}
		}

		private bool CheckCoords(Location apiCoords, Tuple<double, double> wikiCoords)
		{
			var dx = Math.Abs(wikiCoords.Item1 - apiCoords.Latitude);
			var dy = Math.Abs(wikiCoords.Item2 - apiCoords.Longitude);

			return dx/wikiCoords.Item1 <= epsilon && dy/wikiCoords.Item2 <= epsilon;
		}
	}
}
