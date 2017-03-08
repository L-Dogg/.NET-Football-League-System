using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace FootballManager.Presentation.ValidationRules
{
	/// <summary>
	/// Validator class for zipcode strings.
	/// </summary>
	public class PolishZipcodeValidationRule : ValidationRule
	{
		/// <summary>
		/// Validates if value is valid zipcode in polish postal system.
		/// </summary>
		/// <returns>True if value is valid polish zipcode, false otherwise</returns>
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (value == null)
				return new ValidationResult(false, "Not a valid Zipcode");

			var regex = new Regex(@"^\d{2}-\d{3}$");
			var match = regex.Match(value.ToString());
			
			return new ValidationResult(match.Success, "Not a valid Zipcode");
		}
	}
}
