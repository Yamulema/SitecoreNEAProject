using Neambc.Seiumb.Feature.Forms.Enums;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.Forms.Models
{
    public class ProfileFormModel : IRenderingModel {
		private const string REGEXALPHANUMERIC = @"^[-.,' a-zA-Z0-9]{2,}$";
		private const string REGEXADDRESS = @"^[-#.,' a-zA-Z0-9/\\]{2,}$";

		[Required]
		[RegularExpression(REGEXALPHANUMERIC, ErrorMessage = "Special characters not allowed")]
		[MaxLength(15, ErrorMessage = "Error Length")]
		public string FirstName {
			get; set;
		}
		[Required]
		[RegularExpression(REGEXALPHANUMERIC, ErrorMessage = "Special characters not allowed")]
		[MaxLength(30, ErrorMessage = "Error Length")]
		public string LastName {
			get; set;
		}
		[Required]
		[RegularExpression(REGEXADDRESS, ErrorMessage = "Special characters not allowed")]
		[MaxLength(44, ErrorMessage = "Error Length")]
		public string Address {
			get; set;
		}
		[Required]
		[RegularExpression(REGEXALPHANUMERIC, ErrorMessage = "Special characters not allowed")]
		[MaxLength(44, ErrorMessage = "Error Length")]
		public string City {
			get; set;
		}
		[Required]
		public string State {
			get; set;
		}
		[Required]
		[RegularExpression(@"[0-9]+", ErrorMessage = "Special characters not allowed")]
		[MaxLength(9, ErrorMessage = "Error Length")]
		public string ZipCode {
			get; set;
		}
		[Required]
		[BirthDateTimeCompareAttribute(ErrorMessage = "DateRangeError")]
		public string DateOfBirth {
			get; set;
		}
		[Required]
		[RegularExpression(@"[0-9\-)(]+", ErrorMessage = "Special characters not allowed")]
		public string Phone {
			get; set;
		}
		[Required]
		public bool SendInformation {
			get; set;
		}

		public List<SelectListItem> StatesList {
			get; set;
		}

		public List<ProfileErrors> Errors {
			get; set;
		}

		public Rendering Rendering {
			get; set;
		}
		public Item Item {
			get; set;
		}
		public Item PageItem {
			get; set;
		}
		public bool HasErrorFirstName {
			get; set;
		}
		public bool HasErrorFirstNameInvalidCharacters {
			get; set;
		}
		public bool HasErrorLastName {
			get; set;
		}
		public bool HasErrorLastNameInvalidCharacters {
			get; set;
		}
		public bool HasErrorAddress {
			get; set;
		}
		public bool HasErrorAddressInvalidCharacters {
			get; set;
		}
		public bool HasErrorCity {
			get; set;
		}
		public bool HasErrorCityInvalidCharacters {
			get; set;
		}
		public bool HasErrorDateOfBirthAge {
			get; set;
		}
		public bool HasErrorBirthDate {
			get; set;
		}
		public bool HasErrorZipcodeLength {
			get; set;
		}
		public bool HasErrorZipcode {
			get; set;
		}
		public bool HasErrorPhone {
			get; set;
		}
		public bool HasErrorFirstNameLength {
			get; set;
		}
		public bool HasErrorLastNameLength {
			get; set;
		}
		public bool HasErrorAddressLength {
			get; set;
		}
		public bool HasErrorCityLength {
			get; set;
		}
		public bool HasErrorPhoneLength {
			get; set;
		}
        public string OnClickEventContent { get; set; }

        public void Initialize(Rendering rendering) {
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
			StatesList = new List<SelectListItem>();
			StatesList.AddRange(GetStates());
		}

		private List<SelectListItem> GetStates() {
			var listItems = new List<SelectListItem>();
			var statesList = Sitecore.Context.Database.GetItem(Templates.StatesGlobal.ID).GetChildren();
			foreach (Item state in statesList) {
				if (state != null) {
					listItems.Add(new SelectListItem {
						Text = state["ItemName"],
						Value = state["Value"]
					});
				}
			}
			return listItems;
		}
    }
}