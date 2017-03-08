using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FootballManager.Referee.Presentation.Converters
{
	public class RouteResultToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var route = value as BingServices.RouteResult;
			return route != null && route.Legs.Length > 0 && route.Legs[0].Itinerary != null &&
			       route.Legs[0].Itinerary.Length > 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
