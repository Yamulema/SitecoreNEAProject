using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Feature.Account.UnitTest.Fakes {
	public class FakeAuthenticationAccountManager : IAuthenticationAccountManager {

		#region IAuthenticationAccountManager
		public LoginResponse AuthenticateAccount(string username, string password, AccountMembership account, string cellcode) {
			throw new System.NotImplementedException();
		}
		public void LogoutUser(bool removeTracking = true) {
			throw new System.NotImplementedException();
		}
        public void RetrieveAccount(AccountMembership account, string mdsid)
        {
            throw new System.NotImplementedException();
        }
        public void SaveCustomFacets(AccountMembership account) {
			throw new System.NotImplementedException();
		}
		public void LoginSitecoreContext(string userName) {
			throw new System.NotImplementedException();
		}

		public void ProcessErrorAuthentication(LoginResponse response, AccountMembership account, string username, string pathReset) {
			throw new System.NotImplementedException();
		}

		public ResultSendResetEmail SendExactTargetResetEmail(ExactTargetResetEmail model) {
			throw new System.NotImplementedException();
		}

        public void RemoveSessionOffersMenu(string mdsid) {
            throw new System.NotImplementedException();
        }

		public void InitializeAccountMemberData(AccountMembership accountMembership)
		{
			throw new System.NotImplementedException();
		}

        public void IceTravelDollarCookie(string mdsid)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveIceTravellDollarCookie()
        {
            throw new System.NotImplementedException();
        }

		public bool IsValidRedirection(string currentpath)
        {
			throw new System.NotImplementedException();
		}
		#endregion
	}
}
