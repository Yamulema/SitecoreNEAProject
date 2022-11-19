using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
	public interface IAuthenticationAccountManager
	{
        LoginResponse AuthenticateAccount(string username, string password, AccountMembership account, string cellcode);
		void LogoutUser(bool removeTracking = true);
        void RetrieveAccount(AccountMembership account, string mdsid);
        void SaveCustomFacets(AccountMembership account);
		void LoginSitecoreContext(string userName);

		void ProcessErrorAuthentication(LoginResponse response, AccountMembership account, string username,
			string pathReset);

		ResultSendResetEmail SendExactTargetResetEmail(ExactTargetResetEmail model);
        void RemoveSessionOffersMenu(string mdsid);
		void InitializeAccountMemberData(AccountMembership accountMembership);

		void IceTravelDollarCookie(string mdsid);

		void RemoveIceTravellDollarCookie();
		bool IsValidRedirection(string currentpath);
	}
}