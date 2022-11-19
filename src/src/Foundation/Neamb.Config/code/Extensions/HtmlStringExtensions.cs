using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Neambc.Neamb.Foundation.Configuration.Extensions {
	public static class HtmlStringExtensions {
		/// <summary>
		/// Replaces any token with that matches \{(.*?)\} regex pattern with the correspondig value defined in tokenDictionary. Eg. "Hello {token}". 
		/// </summary>
		/// <param name="htmlString"></param>
		/// <param name="tokenDictionary"></param>
		/// <returns>HtmlString Eg. "Hello World"</returns>
		public static HtmlString ReplaceTokens(this HtmlString htmlString, Dictionary<string, string> tokenDictionary) {
			if (tokenDictionary == null) {
				return htmlString;
			}
			if (!tokenDictionary.Any()) {
				return htmlString;
			}
			if (string.IsNullOrEmpty(htmlString.ToString())) {
				return htmlString;
			}

			var html = htmlString.ToString();

			foreach (var token in tokenDictionary.Where(x => x.Key != null && x.Value != null)) {
				var regex = new Regex(@"\{(.*?)\}", RegexOptions.IgnoreCase);
				var match = regex.Match(token.Key);

				//Checks if the provided token meets a token pattern.
				if (!match.Success) {
					continue;
				}

				html = html.Replace(token.Key, token.Value);
			}

			return new HtmlString(html);
		}
	}
}