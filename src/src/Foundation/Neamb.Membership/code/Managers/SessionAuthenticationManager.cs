using System;
using System.Collections.Generic;
using System.Web;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Diagnostics;
using Sitecore.Links;

namespace Neambc.Neamb.Foundation.Membership.Managers {
	[Service(typeof(ISessionAuthenticationManager))]
	public class SessionAuthenticationManager : ISessionAuthenticationManager {
		/// <summary>
		/// Get the AccountMembership object from Session
		/// </summary>
		/// <returns>AccountMembership of the user logged in the application</returns>
		public AccountMembership GetAccountMembership() {
			var accountMembership = new AccountMembership();
			var account = HttpContext.Current.Session[ConstantsNeamb.SessionAccountMembership];
			if (account != null) {
				accountMembership = (AccountMembership)account;
			}
			return accountMembership;
		}

		/// <summary>
		/// Save the AccountMembership object in Session
		/// </summary>
		/// <param name="accountMembership">AccountMembership of the user logged in the application</param>
		public void SaveAccountMembership(AccountMembership accountMembership) {
			HttpContext.Current.Session[ConstantsNeamb.SessionAccountMembership] = accountMembership;
		}

		/// <summary>
		/// Remove the AccountMembership object from Session
		/// </summary>
		public void RemoveAccountMembership() {
			HttpContext.Current.Session.Remove(ConstantsNeamb.SessionAccountMembership);
		}

		/// <summary>
		/// Save the requested page that call the login in Session
		/// </summary>
		/// <param name="valueSession">Path of the page that call the login</param>
		public void SaveRequestedPageLogin(string valueSession) {
            //The following condition avoid to insert in session the login page url. That is one case when there are a popup windows that reloads in the same login page.
            var pathLoginPage = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID));
            if (!valueSession.Contains(pathLoginPage)) {
                HttpContext.Current.Session[ConstantsNeamb.SessionRequestedPageLogin] = valueSession;
            }
        }

		/// <summary>
		/// Get the value saved in session of the url of the page that call the login
		/// </summary>
		/// <returns></returns>
		public string GetRequestedPageLogin() {
			var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionRequestedPageLogin];
			return valueSession != null ? valueSession.ToString() : string.Empty;
		}

		/// <summary>
		/// Save the requested page that call the login in Session
		/// </summary>
		/// <param name="valueSession">Path of the page that call the login</param>
		public void SaveRequestedPageLoginAbsolutePath(string valueSession) {
			HttpContext.Current.Session[ConstantsNeamb.SessionRequestedPageLoginAbsolutePath] = valueSession;
		}

		/// <summary>
		/// Get the value saved in session of the url of the page that call the login
		/// </summary>
		/// <returns></returns>
		public string GetRequestedPageLoginAbsolutePath() {
			var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionRequestedPageLoginAbsolutePath];
			return valueSession != null ? valueSession.ToString() : string.Empty;
		}

		/// <summary>
		/// Remove the RequestedPageLogin object from Session
		/// </summary>
		public void RemoveRequestedPageLogin() {
			HttpContext.Current.Session.Remove(ConstantsNeamb.SessionRequestedPageLogin);
		}
        /// <summary>
        /// Save parameter utm_source query string value in session
        /// </summary>
        /// <param name="valueSession">Query string value</param>
        public void SaveMedium(string valueSession)
        {
            HttpContext.Current.Session[ConstantsNeamb.SessionMedium] = valueSession;
            Log.Info($"Stored MediumCode with the value of{valueSession}", this);
        }

        /// <summary>
        /// Get parameter utm_source query string value in session
        /// </summary>
        /// <returns>Session value</returns>
        public string GetMedium()
        {
            var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionMedium];
            var result = valueSession != null ? valueSession.ToString() : string.Empty;
            Log.Info($"Fetched MediumCode with the value of{result}", this);
            return result;
        }
        /// <summary>
        /// Remove the Medium object from Session
        /// </summary>
        public void RemoveMedium()
        {
            HttpContext.Current.Session.Remove(ConstantsNeamb.SessionMedium);
            Log.Info($"Removed Medium.", this);
        }
        /// <summary>
        /// Save parameter utm_source query string value in session
        /// </summary>
        /// <param name="valueSession">Query string value</param>
        public void SaveCellCode(string valueSession) {
			HttpContext.Current.Session[ConstantsNeamb.SessionCellcode] = valueSession;
			Log.Info($"Stored CellCode with the value of{valueSession}", this);
		}

		/// <summary>
		/// Get parameter utm_source query string value in session
		/// </summary>
		/// <returns>Session value</returns>
		public string GetCellCode() {
			var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionCellcode];
			var result = valueSession != null ? valueSession.ToString() : string.Empty;
			Log.Info($"Fetched CellCode with the value of{result}", this);
			return result;
		}

		/// <summary>
		/// Remove the Cellcode object from Session
		/// </summary>
		public void RemoveCellCode() {
			HttpContext.Current.Session.Remove(ConstantsNeamb.SessionCellcode);
			Log.Info($"Removed CellCode.", this);
		}

		/// <summary>
		/// Save parameter utm_campaign query string value in session
		/// </summary>
		/// <param name="valueSession">Query string value</param>
		public void SaveCampaignCode(string valueSession) {
			HttpContext.Current.Session[ConstantsNeamb.SessionCampaigncode] = valueSession;
		}

		/// <summary>
		/// Get parameter utm_campaign query string value in session
		/// </summary>
		/// <returns>Session value</returns>
		public string GetCampaignCode() {
			var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionCampaigncode];
			return valueSession != null ? valueSession.ToString() : string.Empty;
		}

		/// <summary>
		/// Remove the Campaigncode object from Session
		/// </summary>
		public void RemoveCampaignCode() {
			HttpContext.Current.Session.Remove(ConstantsNeamb.SessionCampaigncode);
		}

        /// <summary>
        /// Save parameter utm_term query string value in session
        /// </summary>
        /// <param name="valueSession">Query string value</param>
        public void SaveUtmTerm(string valueSession)
        {
            HttpContext.Current.Session[ConstantsNeamb.SessionUtmTerm] = valueSession;
        }

        /// <summary>
        /// Get parameter utm_term query string value in session
        /// </summary>
        /// <returns>Session value</returns>
        public string GetUtmTerm()
        {
            var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionUtmTerm];
            return valueSession != null ? valueSession.ToString() : string.Empty;
        }

        /// <summary>
        /// Remove the utm_term object from Session
        /// </summary>
        public void RemoveUtmTerm()
        {
            HttpContext.Current.Session.Remove(ConstantsNeamb.SessionUtmTerm);
        }

        /// <summary>
        /// Save parameter gclid query string value in session
        /// </summary>
        /// <param name="valueSession">Query string value</param>
        public void SaveGclid(string valueSession)
        {
            HttpContext.Current.Session[ConstantsNeamb.SessionGclid] = valueSession;
        }

        /// <summary>
        /// Get parameter gclid query string value in session
        /// </summary>
        /// <returns>Session value</returns>
        public string GetGclid()
        {
            var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionGclid];
            return valueSession != null ? valueSession.ToString() : string.Empty;
        }

        /// <summary>
        /// Remove the gclid object from Session
        /// </summary>
        public void RemoveGclid()
        {
            HttpContext.Current.Session.Remove(ConstantsNeamb.SessionGclid);
        }

        /// <summary>
        /// Save parameter sob query string value in session
        /// </summary>
        /// <param name="valueSession">Query string value</param>
        public void SaveSob(string valueSession)
        {
            HttpContext.Current.Session[ConstantsNeamb.SessionSob] = valueSession;
        }

        /// <summary>
        /// Get parameter sob query string value in session
        /// </summary>
        /// <returns>Session value</returns>
        public string GetSob()
        {
            var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionSob];
            return valueSession != null ? valueSession.ToString() : string.Empty;
        }

        /// <summary>
        /// Remove the sob object from Session
        /// </summary>
        public void RemoveSob()
        {
            HttpContext.Current.Session.Remove(ConstantsNeamb.SessionSob);
        }

        /// <summary>
        /// Save the requested page that call the register in Session
        /// </summary>
        /// <param name="valueSession">Path of the page that call the login</param>
        public void SaveRequestedPageRegister(string valueSession) {
			HttpContext.Current.Session[ConstantsNeamb.SessionRequestedPageRegister] = valueSession;
		}

		/// <summary>
		/// Get the value saved in session of the url of the page that call the register
		/// </summary>
		/// <returns></returns>
		public string GetRequestedPageRegister() {
			var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionRequestedPageRegister];
			return valueSession != null ? valueSession.ToString() : string.Empty;
		}

		/// <summary>
		/// Remove the RequestedPageRegister object from Session
		/// </summary>
		public void RemoveRequestedPageRegister() {
			HttpContext.Current.Session.Remove(ConstantsNeamb.SessionRequestedPageRegister);
		}

		/// <summary>
		/// Save the attempt number in session
		/// </summary>
		/// <param name="valueSession">attempt value</param>
		public void SaveAttemptZipCodeValidation(int valueSession) {
			HttpContext.Current.Session[ConstantsNeamb.SessionAttemptZipcodeVal] = valueSession;
		}

		/// <summary>
		/// Get the attempt number in session
		/// </summary>
		/// <returns>Session value</returns>
		public int GetAttemptZipCodeValidation() {
			var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionAttemptZipcodeVal];
			return valueSession != null ? Convert.ToInt32(valueSession) : 0;
		}

		/// <summary>
		/// Remove the AttemptZipCodeValidation object from Session
		/// </summary>
		public void RemoveAttemptZipCodeValidation() {
			HttpContext.Current.Session.Remove(ConstantsNeamb.SessionAttemptZipcodeVal);
		}

		/// <summary>
		/// Save the duplicate registration in session
		/// </summary>
		/// <param name="valueSession">attempt value</param>
		public void SaveDuplicateRegistration(List<LoginRegistration> valueSession) {
			HttpContext.Current.Session[ConstantsNeamb.SessionDuplicateRegistration] = valueSession;
		}

		/// <summary>
		/// Get the duplicate registration in session
		/// </summary>
		/// <returns>Session value</returns>
		public List<LoginRegistration> GetDuplicateRegistration() {
			var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionDuplicateRegistration];
			return valueSession != null ? (List<LoginRegistration>)valueSession : null;
		}

		/// <summary>
		/// Remove the duplicate registration object from Session
		/// </summary>
		public void RemoveDuplicateRegistration() {
			HttpContext.Current.Session.Remove(ConstantsNeamb.SessionDuplicateRegistration);
		}

		/// <summary>
		/// Save the duplicate registration email in session
		/// </summary>
		/// <param name="valueSession">duplicate registration email</param>
		public void SaveDuplicateRegistrationEmail(string valueSession) {
			HttpContext.Current.Session[ConstantsNeamb.SessionDuplicateRegistrationEmail] = valueSession;
		}

		/// <summary>
		/// Get the duplicate registration email in session
		/// </summary>
		/// <returns>Session value</returns>
		public string GetDuplicateRegistrationEmail() {
			var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionDuplicateRegistrationEmail];
			return valueSession != null ? valueSession.ToString() : string.Empty;
		}

		/// <summary>
		/// Remove the duplicate registration email from Session
		/// </summary>
		public void RemoveDuplicateRegistrationEmail() {
			HttpContext.Current.Session.Remove(ConstantsNeamb.SessionDuplicateRegistrationEmail);
		}

		/// <summary>
		/// Save the duplicate registration password in session
		/// </summary>
		/// <param name="valueSession">duplicate registration password</param>
		public void SaveDuplicateRegistrationPassword(string valueSession) {
			HttpContext.Current.Session[ConstantsNeamb.SessionDuplicateRegistrationPassword] = valueSession;
		}

		/// <summary>
		/// Get the duplicate registration password in session
		/// </summary>
		/// <returns>Session value</returns>
		public string GetDuplicateRegistrationPassword() {
			var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionDuplicateRegistrationPassword];
			return valueSession != null ? valueSession.ToString() : string.Empty;
		}

		/// <summary>
		/// Remove the duplicate registration password from Session
		/// </summary>
		public void RemoveDuplicateRegistrationPassword() {
			HttpContext.Current.Session.Remove(ConstantsNeamb.SessionDuplicateRegistrationPassword);
		}
		/// <summary>
		/// Get the AccountMembershipDraft object from Session
		/// </summary>
		/// <returns>AccountMembershipDraft of the user logged in the application</returns>
		public AccountMembership GetAccountMembershipDraft() {
			var accountMembership = new AccountMembership();
			var account = HttpContext.Current.Session[ConstantsNeamb.SessionAccountMembershipDraft];
			if (account != null) {
				accountMembership = (AccountMembership)account;
			}
			return accountMembership;
		}

		/// <summary>
		/// Save the AccountMembershipDraft object in Session
		/// </summary>
		/// <param name="accountMembership">AccountMembershipDraft of the user logged in the application</param>
		public void SaveAccountMembershipDraft(AccountMembership accountMembership) {
			HttpContext.Current.Session[ConstantsNeamb.SessionAccountMembershipDraft] = accountMembership;
		}

		public void RemoveAccountMembershipDraft() {
			HttpContext.Current.Session.Remove(ConstantsNeamb.SessionAccountMembershipDraft);
		}

		/// <summary>
		/// Save the zipcode validation success flag
		/// </summary>
		public void SaveZipCodeValidationSuccess() {
			HttpContext.Current.Session[ConstantsNeamb.SessionZipcodeValSuccess] = true;
		}

		/// <summary>
		/// Get the zipcode validation success flag
		/// </summary>
		public bool GetZipCodeValidationSuccess() {
            var valueSession = HttpContext.Current.Session[ConstantsNeamb.SessionZipcodeValSuccess];
            return valueSession != null;
        }

		/// <summary>
		/// Remove the zipcode validation success flag
		/// </summary>
		public void RemoveZipCodeValidationSuccess() {
			HttpContext.Current.Session.Remove(ConstantsNeamb.SessionZipcodeValSuccess);
		}
	}
}

