using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Neambc.Seiumb.Foundation.Sitecore.Extensions {
	public class EmailCompareAttribute : ValidationAttribute {

		protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
			var success = ValidationResult.Success;
			if (value != null) {
                if(!value.ToString().Contains("..") &&
                    !value.ToString().Contains("._") &&
                    !value.ToString().Contains(".-") &&
                    !value.ToString().Contains("__") &&
                    !value.ToString().Contains("_.") &&
                    !value.ToString().Contains("_-") &&
                    !value.ToString().Contains("--") &&
                    !value.ToString().Contains("-.") &&
                    !value.ToString().Contains("-_"))
                { 
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