using System;
using System.Web;
using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Feature.Language.Managers {
	[Service(typeof(ICookieManager))]
	public class BrowserCookie : ICookieManager {

		#region Public Methods
		public HttpCookie GetCookie(string name) {
			var httpContext = new HttpContextWrapper(HttpContext.Current);
			var cookie = httpContext.Request.Cookies[name];
			return cookie;
		}
		public void CreateCookie(string name) {
			var httpContext = new HttpContextWrapper(HttpContext.Current);
			var cookie = new HttpCookie(name);
			httpContext.Response.Cookies.Add(cookie);
		}
		public void CreateCookie(string name, string value, TimeSpan expires) {
			var httpContext = new HttpContextWrapper(HttpContext.Current);
			if (httpContext.Request.Cookies[name] != null) {
				var cookie = httpContext.Request.Cookies[name];
				cookie.Expires = DateTime.Now.Add(expires);
				cookie.Value = value;
				httpContext.Response.Cookies.Set(cookie);
			} else {
				var cookie = new HttpCookie(name, value) {
					Expires = DateTime.Now.Add(expires)
				};
				httpContext.Response.Cookies.Add(cookie);
			}
		}
		public bool Exists(string name) {
			var httpContext = new HttpContextWrapper(HttpContext.Current);
			return httpContext.Request.Cookies[name] != null;
		}
		#endregion

	}
}