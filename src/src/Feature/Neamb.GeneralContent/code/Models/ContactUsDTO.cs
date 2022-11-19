using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Linq;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class ContactUsDTO : IRenderingModel {
		public ICacheManager _cacheManager {
			get; set;
		}
		private const string SELECT_TEXT_SPANISH = "Seleccione";
		private const string SELECT_TEXT_ENGLISH = "Select";
		private const string REGEXALPHANUMERIC = @"^[-.,' a-zA-Z0-9]{2,}$";

		private readonly ISessionAuthenticationManager _sessionManager;
		public Rendering Rendering {
			get; set;
		}
		public Item Item {
			get; set;
		}

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[RegularExpression(REGEXALPHANUMERIC, ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MaxLength(15, ErrorMessage = ConstantsNeamb.ValidationLength)]
		public string FirstName {
			get; set;
		}
		public bool HasTooltipFirstName {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsFirstName {
			get; set;
		}

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[RegularExpression(REGEXALPHANUMERIC, ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MaxLength(30, ErrorMessage = ConstantsNeamb.ValidationLength)]
		public string LastName {
			get; set;
		}
		public bool HasTooltipLastName {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsLastName {
			get; set;
		}

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[MaxLength(100, ErrorMessage = ConstantsNeamb.ValidationLength)]
        [EmailCompare(ErrorMessage = "Email Format")]
		[EmailAddress(ErrorMessage = "Email Format")]
		public string Email {
			get; set;
		}
		public string Emailconfirmation {
			get; set;
		}
		public bool HasTooltipEmail {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsEmail {
			get; set;
		}

		public string StateAffiliate {
			get; set;
		}
		public bool HasTooltipStateAffiliate {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsStateAffiliate {
			get; set;
		}
		public List<SelectListItem> StateAffiliatesList {
			get; set;
		}

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		public string Topic {
			get; set;
		}
		public bool HasTooltipTopic {
			get; set;
		}
		public List<ErrorStatusEnum> ErrorsTopic {
			get; set;
		}
		public List<SelectListItem> TopicList {
			get; set;
		}

		[AllowHtml]
		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		[RegularExpression(@"^[a-zñáéíóúA-ZÑÁÉÍÓÚ0-9ü\¡\!string.Empty\?\¿#%@,_./ '\-:;()*]*$\n", ErrorMessage = ConstantsNeamb.ValidationSpecialCharacters)]
		[MaxLength(1000, ErrorMessage = ConstantsNeamb.ValidationLength)]
		public string Message {
			get; set;
		}
		public ErrorStatusEnum ErrorsMessage {
			get; set;
		}
		public bool IsModelValid {
			get; set;
		}
		public bool HasGeneralError {
			get; set;
		}
		public bool WasProcessedSuccessfully {
			get; set;
		}
		public string SelectText {
			get; private set;
		}
		public bool HasCaptchaError {
			get; set;
		}
		public string CaptchaKey {
			get; private set;
		}
		public string GtmAction
		{
			get; set;
		}
		public bool HasSelectedTopic
		{
			get; set;
		}
		public bool HasSelectedState
		{
			get; set;
		}
		public ContactUsDTO(ISessionAuthenticationManager sessionAuthenticationManager, ICacheManager cacheManager) {
			_cacheManager = cacheManager;
			_sessionManager = sessionAuthenticationManager;
		}

		public ContactUsDTO() {

		}

		public void Initialize(Rendering rendering) {
			Rendering = rendering;
			Item = rendering.Item;
			ErrorsFirstName = new List<ErrorStatusEnum>();
			ErrorsLastName = new List<ErrorStatusEnum>();
			ErrorsEmail = new List<ErrorStatusEnum>();
			ErrorsMessage = ErrorStatusEnum.None;
			var language = Item[Templates.LanguageToggle.Fields.Default];
			StateAffiliatesList = GetSelectList(Items.StateAffiliate, StateAffiliate);
			if (language == Items.SpanishLanguage.ToString()) {
				TopicList = GetSelectList(Items.ContactUsTopicEs, Topic);
				SelectText = SELECT_TEXT_SPANISH;
			} else {
				TopicList = GetSelectList(Items.ContactUsTopic, Topic);
				SelectText = SELECT_TEXT_ENGLISH;
			}
			var selectedState = StateAffiliatesList.Count(item => item.Selected == true);
			HasSelectedState = selectedState>0;
			var selectedTopic = TopicList.Count(item => item.Selected == true);
			HasSelectedTopic = selectedTopic > 0;
			WasProcessedSuccessfully = false;
			HasCaptchaError = false;
			CaptchaKey = Configuration.CaptchaKey;
		}
		private List<SelectListItem> GetSelectList(ID itemID, string field) {
			List<SelectListItem> listItems;
			listItems = GetListFromSitecore(itemID, field);
			return listItems;
		}

		private List<SelectListItem> GetListFromSitecore(ID itemID, string field) {
			var listItems = new List<SelectListItem>();
			var list = Sitecore.Context.Database.GetItem(itemID).GetChildren();
			foreach (Item item in list) {
				if (item != null) {
					listItems.Add(new SelectListItem {
						Text = item.Name,
						Value = item[Templates._CategoryItem.Fields.Value],
						Selected = !string.IsNullOrEmpty(field) && (item[Templates._CategoryItem.Fields.Value] == field)
					});
				}
			}
			return listItems;
		}

		public void LoadFields() {
			var accountMembership = _sessionManager.GetAccountMembership();
			if (accountMembership.Status == StatusEnum.Hot || accountMembership.Status == StatusEnum.WarmHot || accountMembership.Status == StatusEnum.WarmCold) {
				FirstName = accountMembership.Profile.FirstName;
				LastName = accountMembership.Profile.LastName;
			}
			if (accountMembership.Status == StatusEnum.Hot || accountMembership.Status == StatusEnum.WarmHot) {
				Email = accountMembership.Username;
			}
			HasTooltipEmail = !string.IsNullOrEmpty(Item[Templates.Email.Fields.Tooltip]);
			HasTooltipFirstName = !string.IsNullOrEmpty(Item[Templates.Name.Fields.FirstNameTooltip]);
			HasTooltipLastName = !string.IsNullOrEmpty(Item[Templates.Name.Fields.LastNameTooltip]);
		}
	}
}