using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Enums;
using Neambc.Neamb.Foundation.Configuration.Extensions;

namespace Neambc.Neamb.Foundation.Config.Utility {
	public static class ValidationFieldHelper {
		public static List<ErrorStatusEnum> SetErrorsField(ModelState modelStateVal, bool checkRequired,
			bool checkInvalidChar, bool checkLength) {
			var errorList = new List<ErrorStatusEnum>();
			if (checkRequired &&
				modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals(ConstantsNeamb.ValidationRequired)) != null) {
				errorList.Add(ErrorStatusEnum.Required);
			}

			if (checkLength &&
				modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals(ConstantsNeamb.ValidationLength)) != null) {
				errorList.Add(ErrorStatusEnum.Length);
			}

			if (checkLength &&
				modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals(ConstantsNeamb.ValidationMinLength)) != null) {
				errorList.Add(ErrorStatusEnum.MinLength);
			}

			if (checkInvalidChar &&
				modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals(ConstantsNeamb.ValidationSpecialCharacters)) !=
				null) {
				errorList.Add(ErrorStatusEnum.InvalidCharacters);
			}

			if (checkInvalidChar &&
				modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals(ConstantsNeamb.ValidationEmailFormat)) !=
				null) {
				errorList.Add(ErrorStatusEnum.EmailFormat);
			}

			return errorList;
		}

		public static bool ValidateBirthDate(DateTime inputDate) {
			if (inputDate.Year < DateTime.Now.Year) {
				var yearDiff = DateTime.Now.Year - inputDate.Year;
				return yearDiff >= 16 && yearDiff <= 95;
			} else {
				return false;
			}
		}
        
        public static bool ValidatePassword(string password) {
			if (!string.IsNullOrEmpty(password) && password.Length >= 8 && Regex.Matches(password, @"[a-zA-Z]").Count > 0 && password.Any(c => char.IsDigit(c))) {
				return true;
			} else {
				return false;
			}
		}
		public static ErrorStatusEnum SetErrorStatus(ModelState modelStateVal, bool checkRequired,
			bool checkInvalidChar, bool checkLength) {
			var result = ErrorStatusEnum.None;

			if (modelStateVal != null) {
				if (checkRequired &&
					modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals(ConstantsNeamb.ValidationRequired)) != null) {
					result |= ErrorStatusEnum.Required;
				}

				if (checkLength &&
					modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals(ConstantsNeamb.ValidationLength)) != null) {
					result |= ErrorStatusEnum.Length;
				}

				if (checkLength &&
					modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals(ConstantsNeamb.ValidationMinLength)) != null) {
					result |= ErrorStatusEnum.MinLength;
				}

				if (checkInvalidChar &&
					modelStateVal.Errors.FirstOrDefault(i => i.ErrorMessage.Equals(ConstantsNeamb.ValidationSpecialCharacters)) !=
					null) {
					result |= ErrorStatusEnum.InvalidCharacters;
				}
			}

			return result;
		}

		public static ErrorStatusEnum GetErrorStatus(string fieldName, ViewDataDictionary viewData, bool checkRequired,
			bool checkInvalidChar, bool checkLength) {
			return SetErrorStatus(viewData.ModelState[fieldName], checkRequired, checkInvalidChar, checkLength);
		}

		public static ModelErrorType GetAttributeError(ModelState modelState) {
			var result = ModelErrorType.None;
			if (modelState.Errors.Any(x => x.ErrorMessage == nameof(ModelErrorType.Required))) {
				result |= ModelErrorType.Required;
			}
			if (modelState.Errors.Any(x => x.ErrorMessage == nameof(ModelErrorType.RegularExpression))) {
				result |= ModelErrorType.RegularExpression;
			}
			if (modelState.Errors.Any(x => x.ErrorMessage == nameof(ModelErrorType.MaxLength))) {
				result |= ModelErrorType.MaxLength;
			}
			return result;
		}

		public static Dictionary<string, ModelErrorType> GetAttributesError(ModelStateDictionary modelState) {
			return modelState.ToDictionary(x => x.Key, x => GetAttributeError(x.Value));
		}
	}
}