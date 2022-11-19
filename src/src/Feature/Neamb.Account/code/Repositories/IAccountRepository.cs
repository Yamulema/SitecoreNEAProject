using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.MBCData.Model.RegisterUser;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Feature.Account.Repositories
{
	public interface IAccountRepository
	{
        LoginResponse AuthenticateUser(AccountMembership account, string username, string password, string pathReset,
			bool rememberme, bool executeLogout=true);

        RegisterUserResponse RegisterUser(AccountMembership account, string password);

		bool SendExactTargetRegisterEmail(string userName, string mdsInvId, string firstname, string lastname,
			string emailOptOut, string newEnvInd, string membershipType, string complifeDate, bool memberflag, string newMemberFlag);

		string GetDuplicateRegistrationPageUrl();
		List<EmailDuplicate> GetDuplicateRegistrationEmails(string currentEmail);
		bool DeleteDuplicateRegistrationEmails(string emailSelected, List<string> emailsDeleted);

		bool SendExactTargetDuplicateRegistrationEmail(AccountMembership accountMembership, string emailsDeleted,
			string emailSelected);

		void SetErrorProfile(ProfileDTO model, ViewDataDictionary viewData);
		bool HasDateBirthCustomValidationErrors<T>(T model) where T : IDateBirthDTO;
		bool HasEmailCustomValidationErrors(ProfileBasicDTO model);
        bool HasEmailDomainValidationErrors(string email);


        bool HasPasswordCustomValidationErrors<T>(T model) where T : IPasswordDTO;

		void SendExactTargetChangeUserName(string firstName, string lastName,
			string individualId, string newUsername, string oldUsername, string newcellParam, string oldcellParam);

		void SetErrorUserBasicData<T>(T model, ViewDataDictionary viewData) where T : IUsernameBasicDTO;

		bool SendExactTargetAddFamilyMember(string firstName, string lastName,
			string mdsid, string username);

		string GetInviteFamilyPageUrl();

		string ExecuteRegistration(RegistrationDTO model, ViewDataDictionary viewData, bool isModelValid);
		void SetGtmActionRegistration(RegistrationDTO model);
        bool SaveFavoriteStore(string mdsId, string emailAddress, List<StoreInfo> stores);
        Profile RetrieveRakutenProfile(AccountMembership account);
    }
}