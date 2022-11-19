using System;
using System.Collections.Generic;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Feature.Account.UnitTest.Fakes {


	public class FakeSessionAuthenticationManager : ISessionAuthenticationManager {

		public AccountMembership AccountMembership {
			get; set;
		}
		public AccountMembership AccountMembershipDraft {
			get; set;
		}
		public DateTime MembershipDraftSavedAt {
			get; set;
		}
		public DateTime MembershipSavedAt {
			get; set;
		}

		#region ISessionAuthenticationManager
		public AccountMembership GetAccountMembership() {
			return AccountMembership;
		}

		public AccountMembership GetAccountMembershipDraft() {
			return AccountMembershipDraft;
		}

		public int GetAttemptZipCodeValidation() {
			throw new NotImplementedException();
		}

		public string GetCampaignCode() {
			throw new NotImplementedException();
		}

		public string GetCellCode() {
			throw new System.NotImplementedException();
		}

		public List<LoginRegistration> GetDuplicateRegistration() {
			throw new System.NotImplementedException();
		}

		public string GetDuplicateRegistrationEmail() {
			throw new System.NotImplementedException();
		}

		public string GetDuplicateRegistrationPassword() {
			throw new System.NotImplementedException();
		}

		public string GetRequestedPageLogin() {
			throw new System.NotImplementedException();
		}

		public string GetRequestedPageLoginAbsolutePath() {
			throw new System.NotImplementedException();
		}

		public string GetRequestedPageRegister() {
			throw new System.NotImplementedException();
		}

		public bool GetZipCodeValidationSuccess() {
			throw new System.NotImplementedException();
		}

		public void RemoveAccountMembership() {
			throw new System.NotImplementedException();
		}

		public void RemoveAccountMembershipDraft() {
			throw new System.NotImplementedException();
		}

		public void RemoveAttemptZipCodeValidation() {
			throw new System.NotImplementedException();
		}

		public void RemoveCampaignCode() {
			throw new System.NotImplementedException();
		}

		public void RemoveCellCode() {
			throw new System.NotImplementedException();
		}

		public void RemoveDuplicateRegistration() {
			throw new System.NotImplementedException();
		}

		public void RemoveDuplicateRegistrationEmail() {
			throw new System.NotImplementedException();
		}

		public void RemoveDuplicateRegistrationPassword() {
			throw new System.NotImplementedException();
		}

		public void RemoveRequestedPageLogin() {
			throw new System.NotImplementedException();
		}

		public void RemoveRequestedPageRegister() {
			throw new System.NotImplementedException();
		}

		public void RemoveZipCodeValidationSuccess() {
			throw new System.NotImplementedException();
		}

		public void SaveAccountMembership(AccountMembership accountMembership) {
			MembershipSavedAt = DateTime.UtcNow;
		}

		public void SaveAccountMembershipDraft(AccountMembership accountMembership) {
			MembershipDraftSavedAt = DateTime.UtcNow;
		}

		public void SaveAttemptZipCodeValidation(int valueSession) {
			throw new System.NotImplementedException();
		}

		public void SaveCampaignCode(string valueSession) {
			throw new System.NotImplementedException();
		}

		public void SaveCellCode(string valueSession) {
			throw new System.NotImplementedException();
		}

		public void SaveDuplicateRegistration(List<LoginRegistration> valueSession) {
			throw new System.NotImplementedException();
		}

		public void SaveDuplicateRegistrationEmail(string valueSession) {
			throw new System.NotImplementedException();
		}

		public void SaveDuplicateRegistrationPassword(string valueSession) {
			throw new System.NotImplementedException();
		}

		public void SaveRequestedPageLogin(string valueSession) {
			throw new System.NotImplementedException();
		}

		public void SaveRequestedPageLoginAbsolutePath(string valueSession) {
			throw new System.NotImplementedException();
		}

		public void SaveRequestedPageRegister(string valueSession) {
			throw new System.NotImplementedException();
		}

		public void SaveZipCodeValidationSuccess() {
			throw new System.NotImplementedException();
		}

		public void SaveMedium(string valueSession)
		{
			throw new System.NotImplementedException();
		}

		public string GetMedium()
		{
			throw new System.NotImplementedException();
		}

		public void RemoveMedium()
		{
			throw new System.NotImplementedException();
		}
        public void SaveUtmTerm(string valueSession)
        {
            throw new System.NotImplementedException();
        }

        public string GetUtmTerm()
        {
            throw new System.NotImplementedException();
        }
        public void RemoveUtmTerm()
        {
            throw new System.NotImplementedException();
        }
        public void SaveGclid(string valueSession)
        {
            throw new System.NotImplementedException();
        }
        public string GetGclid()
        {
            throw new System.NotImplementedException();
        }
        public void RemoveGclid()
        {
            throw new System.NotImplementedException();
        }
        public void SaveSob(string valueSession)
        {
            throw new System.NotImplementedException();
        }
        public string GetSob()
        {
            throw new System.NotImplementedException();
        }
        public void RemoveSob()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
