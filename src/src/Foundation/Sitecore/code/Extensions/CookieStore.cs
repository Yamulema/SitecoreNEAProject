using System;
using System.Web;

namespace Neambc.Seiumb.Foundation.Sitecore.Extensions {
	public static class CookieStore {
		/// <summary>
		/// Sets a cookie with an arbitrary value.
		/// </summary>
		/// <param name="key">Cookie key</param>
		/// <param name="value">Cookie value</param>
		/// <param name="expires">TimeSpan expiration</param>
		public static void SetCookie(string key, string value, TimeSpan expires) {
			var _httpContext = new HttpContextWrapper(HttpContext.Current);

			if (_httpContext.Request.Cookies[key] != null) {
				var cookie = _httpContext.Request.Cookies[key];
				cookie.Expires = DateTime.Now.Add(expires);
				cookie.Domain = _httpContext.Request.Url.Host;
				cookie.Value = value;

				_httpContext.Response.Cookies.Set(cookie);
			} else {
				var cookie = new HttpCookie(key, value) {
					Domain = _httpContext.Request.Url.Host,
					Expires = DateTime.Now.Add(expires)
				};

				_httpContext.Response.Cookies.Add(cookie);
			}
		}

		/// <summary>
		/// Get the value of a cookie
		/// </summary>
		/// <param name="key">Key of the cookie</param>
		/// <returns></returns>
		public static string GetCookie(string key) {
			var httpContext = new HttpContextWrapper(HttpContext.Current);
			//Get the cookie
			var value = string.Empty;
			var cookie = httpContext.Request.Cookies[key];

			if (cookie != null) {
				value = cookie.Value;
			}
			return value;
		}
	}
}