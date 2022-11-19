using System;
using System.Web;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Product.Interfaces;

namespace Neambc.Neamb.Foundation.Product.Manager {
	[Service(typeof(IDatapassActionTypeManager))]
	public class QueryStringManager {
		/// <summary>
		/// Append to the url to be opened in cta link the query parameters
		/// </summary>
		/// <param name="urlInput">Input url</param>
		/// <param name="sessionManager">Session manager</param>
		/// <returns>Inpur url with the query parameters obtained from session</returns>
		protected string AppendQueryStringParameter(string urlInput, ISessionManager sessionManager) {
			var urlOutput = urlInput;
			var sessionQueryString = sessionManager.RetrieveFromSession<string>(ConstantsNeamb.QueryParameter);
			if (!string.IsNullOrEmpty(sessionQueryString)) {
				var myUri = new Uri(urlInput);
				urlOutput = HttpUtility.ParseQueryString(myUri.Query).Count == 0
					? $"{urlInput}?{sessionQueryString}"
					: $"{urlInput}&{sessionQueryString}";
			}

			return urlOutput;
		}
	}
}