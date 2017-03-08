using System;
using System.Linq;
using System.Windows.Data;

namespace FootballManager.Presentation.Converters
{
	/// <summary>
	/// Converter providing methods checking "logical and" for multiple values.
	/// </summary>
	public class BooleanAndConverter : IMultiValueConverter
	{
		/// <summary>
		/// Checks whether all provided values are valid. <para />
		/// Value cannot be null, false (when its type is bool) or 0 (when its type is int).
		/// </summary>
		/// <param name="values">Values to check.</param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns>True if all values are valid, false otherwise</returns>
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return values.All(value => value != null && (!(value is int) || (int) value != 0) && ((!(value is bool)) || !(bool) value));
		}
		/// <summary>
		/// Not supported for this converter.
		/// <exception cref="NotSupportedException"></exception>
		/// </summary>
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException("BooleanAndConverter is a OneWay converter.");
		}
	}
}
