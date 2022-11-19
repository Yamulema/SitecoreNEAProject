using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Security.Accounts;
using SC = Sitecore.Security.Authentication;

namespace Neambc.Seiumb.Foundation.Sitecore {
	[Service(typeof(IAuthenticationManager))]
	public class AuthenticationManager : IAuthenticationManager {

		public User BuildVirtualUser(string userName, bool isAuthenticated) {
			return SC.AuthenticationManager.BuildVirtualUser(userName, isAuthenticated);
		}

		public bool CheckLegacyPassword(User user, string password) {
			return SC.AuthenticationManager.CheckLegacyPassword(user, password);
		}

		public User GetActiveUser() {
			return SC.AuthenticationManager.GetActiveUser();
		}

		public bool LoginVirtualUser(User user) {
			return SC.AuthenticationManager.LoginVirtualUser(user);
		}

		public bool Login(User user) {
			return SC.AuthenticationManager.Login(user);
		}

		public bool Login(string userName) {
			return SC.AuthenticationManager.Login(userName);
		}

		public bool Login(string userName, string password) {
			return SC.AuthenticationManager.Login(userName, password);
		}

		public bool Login(string userName, bool persistent) {
			return SC.AuthenticationManager.Login(userName, persistent);
		}

		public bool Login(string userName, string password, bool persistent) {
			return SC.AuthenticationManager.Login(userName, password, persistent);
		}

		public bool Login(string userName, bool persistent, bool allowLoginToShell) {
			return SC.AuthenticationManager.Login(userName, persistent, allowLoginToShell);
		}

		public bool Login(string userName, string password, bool persistent, bool allowLoginToShell) {
			return SC.AuthenticationManager.Login(userName, password, persistent, allowLoginToShell);
		}

		public void Logout() {
			SC.AuthenticationManager.Logout();
		}

		public void SetActiveUser(string userName) {
			SC.AuthenticationManager.SetActiveUser(userName);
		}

		public void SetActiveUser(User user) {
			SC.AuthenticationManager.SetActiveUser(user);
		}

		public string GetAuthenticationData(string key) {
			return SC.AuthenticationManager.GetAuthenticationData(key);
		}

		public void SetAuthenticationData(string key, string authenticationData) {
			SC.AuthenticationManager.SetAuthenticationData(key, authenticationData);
		}

		public bool IsAuthenticationTicketExpired() {
			return SC.AuthenticationManager.IsAuthenticationTicketExpired();
		}
	}

}