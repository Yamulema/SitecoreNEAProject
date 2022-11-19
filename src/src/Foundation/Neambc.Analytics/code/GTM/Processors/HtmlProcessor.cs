using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using Neambc.Seiumb.Foundation.Analytics.GTM.Processors.Interfaces;

namespace Neambc.Seiumb.Foundation.Analytics.GTM.Processors
{
	public class HtmlProcessor: IHtmlProcessor
    {
		/// <summary>
		/// Get the text from the HTML
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public string GetTextHtml(string html) {
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
		public string GetSuppresedText(string inputText) {
			return Regex.Replace(inputText, @"\'|\t|\n|\r", "");
		}
        
    }
}