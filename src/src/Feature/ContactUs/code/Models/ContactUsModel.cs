using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.ContactUs.Models {
	public class ContactUsModel : IRenderingModel {

		private const string REGEXALPHANUMERIC = @"^[-.,' a-zA-Z0-9]{2,}$";

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
		[MaxLength(100, ErrorMessage = "Error Length")]
		[EmailAddress(ErrorMessage = "Email Format")]
		public string Email {
			get; set;
		}
		[Required]
		[RegularExpression(@"[0-9\-)(]+", ErrorMessage = "Special characters not allowed")]
		public string Phone {
			get; set;
		}
		[DisallowHTML]
		public string LocalUnion {
			get; set;
		}
		[Required]
		public string State {
			get; set;
		}
		[Required]
		public string Topic {
			get; set;
		}
		[Required]
		[DisallowHTMLComment]
		public string Message {
			get; set;
		}

		public List<SelectListItem> StatesList {
			get; set;
		}
		public List<SelectListItem> TopicsList {
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
		public bool HasErrorFirstNameLength {
			get; set;
		}
		public bool HasErrorLastName {
			get; set;
		}
		public bool HasErrorLastNameInvalidCharacters {
			get; set;
		}
		public bool HasErrorLastNameLength {
			get; set;
		}
		public bool HasErrorEmail {
			get; set;
		}
		public bool HasErrorEmailLength {
			get; set;
		}
		public bool HasErrorEmailFormat {
			get; set;
		}
		public bool HasErrorPhone {
			get; set;
		}
		public bool HasErrorPhoneLength {
			get; set;
		}
		public bool HasErrorState {
			get; set;
		}
		public bool HasErrorTopic {
			get; set;
		}
		public bool HasErrorMessage {
			get; set;
		}
		public bool HasErrorLocalUnion {
			get; set;
		}
		public bool HasErrorMessageInvalidCharacters {
			get; set;
		}
		public bool HasCaptchaError
		{
			get; set;
		}
		public string CaptchaKey
		{
			get; set;
		}
		public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering) {
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
			StatesList = new List<SelectListItem>();
			StatesList.AddRange(GetStates());
			TopicsList = new List<SelectListItem>();
			TopicsList.AddRange(GetTopics());
		}

		public List<SelectListItem> GetStates() {
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

		public List<SelectListItem> GetTopics() {
			var listItems = new List<SelectListItem>();

			var topicsList = Sitecore.Context.Database.GetItem(Templates.TopicsGlobal.ID).GetChildren();

			foreach (Item state in topicsList) {
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