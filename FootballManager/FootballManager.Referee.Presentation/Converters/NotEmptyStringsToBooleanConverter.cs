using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FootballManager.Referee.Presentation.Converters
{
	public class NotEmptyStringsToBooleanConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			foreach (var text in values)
			{
				var s = text as string;

				if (string.IsNullOrEmpty(s))
					return false;
			}

			return values.Length == 3 && values[1] as string == values[2] as string;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
