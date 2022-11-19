using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace Neambc.Seiumb.Feature.Forms.GTM.Utilities
{
	public abstract class HtmlProcessor
	{
		/// <summary>
		/// Get the text from the HTML
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string GetTextHtml(string html) {
			HtmlDocument htmlDoc = new HtmlDocument();
			htmlDoc.LoadHtml(html);

			StringBuilder sanitizedString = new StringBuilder();

			foreach (var node in htmlDoc.DocumentNode.ChildNodes)
				sanitizedString.Append(node.InnerText);
			return HttpUtility.HtmlDecode(sanitizedString.ToString());
		}

		/// <summary>
		/// Remove characters ' and \n
		/// </summary>
		/// <param name="inputText">Input text</param>
		/// <returns></returns>
		public static string GetSuppresedText(string inputText) {
			return Regex.Replace(inputText, @"\'|\t|\n|\r", "");
		}
        
    }
}