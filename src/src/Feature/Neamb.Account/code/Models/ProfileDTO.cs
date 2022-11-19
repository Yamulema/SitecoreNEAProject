using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Account.Models {
	public class ProfileDTO : ProfileBasicDTO, IRenderingModel, IUsernameBasicDTO {

		private const string REGEXALPHANUMERIC = @"^[-.,' a-zA-Z0-9]{2,}$";
		private const string REGEXADDRESS = @"^[-#.,' a-zA-Z0-9/\\]{2,}$";

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[RegularExpression(REGEXADDRESS, ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MaxLength(44, ErrorMessage = ConstantsNeamb.ValidationLength)]
		[MinLength(2, ErrorMessage = ConstantsNeamb.ValidationMinLength)]
		public string Address {
			get; set;
		}

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[RegularExpression(REGEXALPHANUMERIC, ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MaxLength(44, ErrorMessage = ConstantsNeamb.ValidationLength)]
		[MinLength(2, ErrorMessage = ConstantsNeamb.ValidationMinLength)]
		public string City {
			get; set;
		}

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		public string State {
			get; set;
		}

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[RegularExpression(@"[0-9]+", ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MinLength(5, ErrorMessage = ConstantsNeamb.ValidationMinLength)]
		[MaxLength(5, ErrorMessage = ConstantsNeamb.ValidationLength)]
		public string Zip {
			get; set;
		}

		[RegularExpression(@"[0-9\- )(]+", ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MaxLength(12, ErrorMessage = ConstantsNeamb.ValidationLength)]
		public string Phone {
			get; set;
		}

		public string Emailconfirmation {
			get; set;
		}
		public bool OptIn {
			get; set;
		}
		public string UpdateAvatarLink {
			get; set;
		}
		public string ImageAvatar {
			get; set;
		}

		public List<ErrorStatusEnum> ErrorsAddress {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsCity {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsState {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsZip {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsPhone {
			get; set;
		}
		public List<SelectListItem> StatesList {
			get; set;
		}
		public bool HasTooltipAddress {
			get; set;
		}
		public bool HasTooltipCity {
			get; set;
		}
		public bool HasTooltipState {
			get; set;
		}
		public bool HasTooltipZip {
			get; set;
		}
		public bool HasTooltipPhone {
			get; set;
		}
		public string SubmitProfileButton {
			get; set;
		}
		public string SubmitPasswordButton {
			get; set;
		}
		public string CurrentPassword {
			get; set;
		}
		public string NewPassword {
			get; set;
		}
		public string ConfirmPassword {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsCurrentPassword {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsNewPassword {
			get; set;
		}
		public bool HasErrorUsername {
			get; set;
		}
		public bool HasErrorPassword {
			get; set;
		}
		public string UserFullName {
			get; set;
		}
		public string UserMemberSince {
			get; set;
		}
		public string OldcellParam {
			get; set;
		}
		public string NewcellParam {
			get; set;
		}
		public string MsrNameParam {
			get; set;
		}
		public string EmailPermission {
			get; set;
		}
		public Item SiteSettings {
			get; set;
		}
		public string GtmAction
		{
			get; set;
		}

		public bool IsUpdatingPassword
		{
			get; set;
		}

		public ProfileDTO() {

		}

		public ProfileDTO(Item item) {
			Initialize(item);
		}

		public void Initialize(Item item) {
			Item = item;
			StatesList = new List<SelectListItem>();
			ErrorsAddress = new List<ErrorStatusEnum>();
			ErrorsBirthDate = new List<ErrorStatusEnum>();
			ErrorsCity = new List<ErrorStatusEnum>();
			ErrorsEmail = new List<ErrorStatusEnum>();
			ErrorsFirstName = new List<ErrorStatusEnum>();
			ErrorsLastName = new List<ErrorStatusEnum>();
			ErrorsPhone = new List<ErrorStatusEnum>();
			ErrorsState = new List<ErrorStatusEnum>();
			ErrorsZip = new List<ErrorStatusEnum>();
			StatesList = GetStates();
			HasTooltipAddress = false;
			HasTooltipBirthDate = false;
			HasTooltipCity = false;
			HasTooltipEmail = false;
			HasTooltipFirstName = false;
			HasTooltipLastName = false;
			HasTooltipPhone = false;
			HasTooltipZip = false;
			HasTooltipState = false;
			ErrorsCurrentPassword = new List<ErrorStatusEnum>();
			ErrorsNewPassword = new List<ErrorStatusEnum>();
			HasErrorUsername = false;
			HasErrorPassword = false;
			HasGeneralError = false;
			ProcessedSucessfully = false;
			HasDuplicateAccount = false;
		}
		public void Initialize(Rendering rendering) {
			Rendering = rendering;
			Initialize(rendering.Item);
		}

		public List<SelectListItem> GetStates() {
			var listItems = new List<SelectListItem>();

			var statesList = Sitecore.Context.Database.GetItem(Templates.StatesGlobal.ID).GetChildren();

			foreach (Item state in statesList) {
				if (state != null) {
					listItems.Add(new SelectListItem {
						Text = state.Name,
						Value = state[Templates.NameValueItem.Fields.ItemValue],
						Selected = !string.IsNullOrEmpty(State) && (state.Name == State)
					});
				}
			}

			return listItems;
		}

		public string TransformBirthDate(ProfileDTO model) { 
			return model.BirthDate = string.Format("{0}{1}{2}", model.Month.PadLeft(2, '0'), model.Day.PadLeft(2, '0'), model.Year);
		}
	}
}
