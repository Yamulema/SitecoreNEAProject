using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
	public interface ICookieManager
	{
		void SaveWarmUser(string value, TimeSpan expires);
		void SaveRememberMe(string value, TimeSpan expires);
		string GetWarmUser();
		void RemoveWarmUser();
		string GetRememberMe();
		void RemoveRememberMe();
		void SaveCookie(string key, string value, TimeSpan? expires=null, bool? reinitialize = null);
		bool HasCookie(string key);
		void RemoveCookie(string key);

		string GetCookie(string key);
	}
}