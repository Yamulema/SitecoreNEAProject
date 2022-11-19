
using System.Collections.Generic;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
	public interface ISessionAuthenticationManager
	{
		AccountMembership GetAccountMembership();
		void SaveAccountMembership(AccountMembership accountMembership);
		void RemoveAccountMembership();
		void SaveRequestedPageLogin(string valueSession);
		string GetRequestedPageLogin();
		void SaveRequestedPageLoginAbsolutePath(string valueSession);
		string GetRequestedPageLoginAbsolutePath();
		void RemoveRequestedPageLogin();
        void SaveMedium(string valueSession);
        string GetMedium();
        void RemoveMedium();
        void SaveCellCode(string valueSession);
		string GetCellCode();
		void RemoveCellCode();
		void SaveCampaignCode(string valueSession);
		string GetCampaignCode();
		void RemoveCampaignCode(); 
        void SaveUtmTerm(string valueSession);
        string GetUtmTerm();
        void RemoveUtmTerm();
        void SaveGclid(string valueSession);
        string GetGclid();
        void RemoveGclid();
        void SaveSob(string valueSession);
        string GetSob();
        void RemoveSob();
        void SaveRequestedPageRegister(string valueSession);
		string GetRequestedPageRegister();
		void RemoveRequestedPageRegister();
		void SaveAttemptZipCodeValidation(int valueSession);
		int GetAttemptZipCodeValidation();
		void RemoveAttemptZipCodeValidation();
		void SaveDuplicateRegistration(List<LoginRegistration> valueSession);
        List<LoginRegistration> GetDuplicateRegistration();
		void RemoveDuplicateRegistration();
		void SaveDuplicateRegistrationEmail(string valueSession);
		string GetDuplicateRegistrationEmail();
		void RemoveDuplicateRegistrationEmail();
		void SaveDuplicateRegistrationPassword(string valueSession);
		string GetDuplicateRegistrationPassword();
		void RemoveDuplicateRegistrationPassword();
	    AccountMembership GetAccountMembershipDraft();
	    void SaveAccountMembershipDraft(AccountMembership accountMembership);
	    void RemoveAccountMembershipDraft();
		void SaveZipCodeValidationSuccess();
		bool GetZipCodeValidationSuccess();
		void RemoveZipCodeValidationSuccess();
	}
}