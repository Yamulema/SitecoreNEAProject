using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Config.Utility;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Feature.GeneralContent.Repositories {
	[Service(typeof(IContactUsRepository))]
	public class ContactUsRepository : IContactUsRepository {

		#region Fields
		private readonly ISessionAuthenticationManager _sessionManager;
		private readonly IExactTargetClient _exactTargetClient;
		private const string TIME_ZONE = "Central Standard Time";
		private const string NULL_INDIVIDUAL_ID = "000000000";
		private const string SUCCESSFULLY_SEND_EXACT_TARGET_SERVICE = "OK";
		#endregion

		#region Constructors
		public ContactUsRepository(IExactTargetClient exactTargetClient, ISessionAuthenticationManager sessionManager) {
			_exactTargetClient = exactTargetClient;
			_sessionManager = sessionManager;
		}
		#endregion

		#region Public Methods
		public void SubmitContactUs(ref ContactUsDTO model, ViewDataDictionary viewData) {
			var isModelValid = ValidateModel(ref model, viewData);
			if (isModelValid) {
				SendExactTargetContactUsMail(model);
			} else {
				model.HasGeneralError = false;
			}
		}
		public void SendExactTargetContactUsMail(ContactUsDTO model) {
			var accountMembership = _sessionManager.GetAccountMembership();

			var parameters = new List<KeyValuePair<string, string>> {
				new KeyValuePair<string, string>("FIRST_NAME", model.FirstName),
				new KeyValuePair<string, string>("LAST_NAME", model.LastName),
				new KeyValuePair<string, string>("CELL_CODE", Configuration.ContactUsUserEmailCellCode),
				new KeyValuePair<string, string>("CAMPAIGN_CD", Configuration.ContactUsUserEmailCampaignCd),
				new KeyValuePair<string, string>("INDIVIDUAL_ID", GetIndividualID(accountMembership)),
				//new KeyValuePair<string, string>("DATE_TIME_ACTION", GetDateTimeAction()),
				new KeyValuePair<string, string>("SEA_NAME", model.StateAffiliate ?? string.Empty),
				new KeyValuePair<string, string>("MBR_STATUS", accountMembership.Status.ToString()),
				new KeyValuePair<string, string>("MSG_TOPIC", model.Topic),
				new KeyValuePair<string, string>("MBR_EMAIL", model.Email?.ToLower()),
				new KeyValuePair<string, string>("MESSAGE", HttpContext.Current.Server.HtmlEncode(model.Message))
			};
            model.Email = model.Email.ToLower();
			var userEmailResponse = _exactTargetClient.SendExactTargetService(Configuration.ContactUsUserEmailCustomerKey, model.Email?.ToLower(), parameters);
			parameters.RemoveAll(item => item.Key == "CELL_CODE");
			parameters.Add(new KeyValuePair<string, string>("CELL_CODE", Configuration.ContactUsAdminEmailCellCode));
			var adminEmailResponse = _exactTargetClient.SendExactTargetService(Configuration.ContactUsUserEmailCustomerKey, Configuration.ContactUsAdminEmailAddress, parameters);
			if (userEmailResponse) {
				model.WasProcessedSuccessfully = true;
				model.HasGeneralError = false;
			} else {
				model.WasProcessedSuccessfully = false;
				model.HasGeneralError = true;
			}
		}
		#endregion

		#region Public Methods
		private bool ValidateModel(ref ContactUsDTO model, ViewDataDictionary viewData) {
			model.ErrorsMessage |= ValidationFieldHelper.GetErrorStatus(nameof(model.Message), viewData, true, false, true);
			if (model.ErrorsMessage != ErrorStatusEnum.None) {
				return model.IsModelValid = false;
			}

			return model.IsModelValid = true;
		}
		private string GetIndividualID(AccountMembership accountMembership) {
			var individualID = accountMembership.Mdsid;
			if (string.IsNullOrEmpty(individualID)) {
				individualID = NULL_INDIVIDUAL_ID;
			}
			return individualID;
		}
		private string GetDateTimeAction() {
			var targetZone = TimeZoneInfo.FindSystemTimeZoneById(TIME_ZONE);
			var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, targetZone);
			return $"{now.ToString("MM/dd/yyyy")} at {now.ToString("h:mm tt")} (CST)";
		}
		#endregion
	}
}