using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Neambc.Seiumb.Foundation.Sitecore.Extensions {
	public class BirthDateTimeCompareAttribute : ValidationAttribute {

		protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
			var success = ValidationResult.Success;
			if (value != null) {
				var date = DateTime.ParseExact(value.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
				var dateless16 = DateTime.Now.AddYears(-16).Date;
				var dateless90 = DateTime.Now.AddYears(-90).Date;
				if (date <= dateless16 && date.Year >= dateless90.Year) {
					return success;
				}
				return new ValidationResult(FormatErrorMessage(ErrorMessage));
			}

			return new ValidationResult(FormatErrorMessage(ErrorMessage));
		}


		/// <summary>
		/// Gets whether or not <see cref="ValidationContext"/> is required.
		/// </summary>
		public override bool RequiresValidationContext => true;
	}
}