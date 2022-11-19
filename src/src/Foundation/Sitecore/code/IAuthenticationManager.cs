using System;
using Sitecore.Security.Accounts;
using SC = Sitecore.Security.Authentication;

namespace Neambc.Seiumb.Foundation.Sitecore {
	public interface IAuthenticationManager {
		User BuildVirtualUser(string userName, bool isAuthenticated);
		bool CheckLegacyPassword(User user, string password);
		User GetActiveUser();
		bool LoginVirtualUser(User user);
		bool Login(User user);
		bool Login(string userName);
		bool Login(string userName, string password);
		bool Login(string userName, bool persistent);
		bool Login(string userName, string password, bool persistent);
		bool Login(string userName, bool persistent, bool allowLoginToShell);
		bool Login(string userName, string password, bool persistent, bool allowLoginToShell);
		void Logout();
		void SetActiveUser(string userName);
		void SetActiveUser(User user);
		string GetAuthenticationData(string key);
		void SetAuthenticationData(string key, string authenticationData);
		bool IsAuthenticationTicketExpired();
	}
}