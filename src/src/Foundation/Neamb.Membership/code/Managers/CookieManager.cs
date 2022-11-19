using System;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Foundation.Membership.Managers {
	[Service(typeof(ICookieManager))]
	public class CookieManager : ICookieManager {
		/// <summary>
		/// Save a cookie 
		/// </summary>
		/// <param name="key">Cookie key</param>
		/// <param name="value">Cookie value</param>
		/// <param name="expires">Cookie expiration</param>
		/// <param name="reinitialize">Flag to override the cookie and create a new one</param>
		public void SaveCookie(string key, string value, TimeSpan? expires=null, bool? reinitialize = null)
		{
			SetCookie(key, value,expires,reinitialize);
		}

		/// <summary>
		/// Verify the existence of a cookie
		/// </summary>
		/// <param name="key">Cookie key</param>
		/// <returns>Result of the existence of a cookie</returns>
		public bool HasCookie(string key)
		{
			return !string.IsNullOrEmpty(GetCookie(key));
		}

		/// <summary>
		/// Save cookie with name "neambwarmuser"
		/// </summary>
		/// <param name="value">cookie value</param>
		/// <param name="expires">expiration time</param>
		public void SaveWarmUser(string value, TimeSpan expires) {
			SetCookie(ConstantsNeamb.CookieWarm, value, expires);
		}

		/// <summary>
		/// Save cookie with name "neambrememberme"
		/// </summary>
		/// <param name="value">cookie value</param>
		/// <param name="expires">expiration time</param>
		public void SaveRememberMe(string value, TimeSpan expires) {
			SetCookie(ConstantsNeamb.CookieRememberMe, value, expires);
		}

		/// <summary>
		/// Get the cookie with name "neambwarmuser"
		/// </summary>
		/// <returns>cookie value</returns>
		public string GetWarmUser() {
			return GetCookie(ConstantsNeamb.CookieWarm);
		}

		/// <summary>
		/// Remove the cookie with name "neambwarmuser"
		/// </summary>
		public void RemoveWarmUser() {
			RemoveCookie(ConstantsNeamb.CookieWarm);
		}

		/// <summary>
		/// Get the cookie with name "neambrememberme"
		/// </summary>
		/// <returns>cookie value</returns>
		public string GetRememberMe() {
			return GetCookie(ConstantsNeamb.CookieRememberMe);
		}

		/// <summary>
		/// Remove the cookie with name "neambrememberme"
		/// </summary>
		public void RemoveRememberMe() {
			RemoveCookie(ConstantsNeamb.CookieRememberMe);
		}

		/// <summary>
		/// Sets a cookie with an arbitrary value.
		/// </summary>
		/// <param name="key">Cookie key</param>
		/// <param name="value">Cookie value</param>
		/// <param name="expires">TimeSpan expiration</param>
		private void SetCookie(string key, string value, TimeSpan? expires, bool? reinitialize=null) {

            HttpCookie cookie = HttpContext.Current.Response.Cookies.AllKeys.Contains(key)
                ? HttpContext.Current.Response.Cookies[key]
                : HttpContext.Current.Request.Cookies[key];
            if (cookie == null || (reinitialize.HasValue && reinitialize.Value)) cookie = new HttpCookie(key);
            cookie.Value = value;
			if (expires.HasValue)
			{
				cookie.Expires = DateTime.Now.Add(expires.Value);
			}
            HttpContext.Current.Response.Cookies.Set(cookie);
		}

		/// <summary>
		/// Get the value of a cookie
		/// </summary>
		/// <param name="key">Key of the cookie</param>
		/// <returns></returns>
		public string GetCookie(string key) {

            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                string val = cookie.Value;
                if (!String.IsNullOrEmpty(val)) return Uri.UnescapeDataString(val);
            }
            return null;

   
		}

		/// <summary>
		/// Remove the cookie from the browser setting a expiration day in the past
		/// </summary>
		/// <param name="key">Cookie key</param>
		public void RemoveCookie(string key) {

            var _httpContext = new HttpContextWrapper(HttpContext.Current);
            if (_httpContext.Response.Cookies[key] != null)
            {
                var cookie = _httpContext.Response.Cookies[key];
                cookie.Expires = DateTime.Now.AddDays(-1d);
                cookie.Value = "";
                _httpContext.Response.Cookies.Set(cookie);
                HttpContext.Current.Request.Cookies.Remove(key);
            }
            else
            {
                var cookie = new HttpCookie(key)
                {
                    Expires = DateTime.Now.AddDays(-1d)
                };
                _httpContext.Response.Cookies.Add(cookie);
                HttpContext.Current.Request.Cookies.Remove(key);
            }

        }
	}
}