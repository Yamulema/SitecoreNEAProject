using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Pipelines.ProcessSimulatorResponse;
using Sitecore.Sites;
using Sitecore.Syndication;

namespace Neambc.Neamb.Feature.Search.ComputedFields
{
    public class ShortDescriptionHtml: Sitecore.ContentSearch.ComputedFields.IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        public object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");

            if (!(indexable is SitecoreIndexableItem scIndexable))
            {
                Log.Warn(
                    this + " : unsupported IIndexable type : " + indexable.GetType(), this);
                return string.Empty;
            }

            var item = (Item)scIndexable;

            if (item == null)
            {
                Log.Warn(
                    this + " : unsupported SitecoreIndexableItem type : " + scIndexable.GetType(), this);
                return string.Empty;
            }

            // optimization to reduce indexing time
            // by skipping this logic for items in the Core database
            if (string.Compare(
                    item.Database.Name,
                    "core",
                    System.StringComparison.OrdinalIgnoreCase) == 0)
                return string.Empty;

            if (item.Paths.IsMediaItem)
                if (item.TemplateID != TemplateIDs.MediaFolder
                    && item.ID != ItemIDs.MediaLibraryRoot)
                    return LinkManager.GetItemUrl(item);

            if (!item.Paths.IsContentItem)
                return string.Empty;
            var website = Sitecore.Configuration.Factory.GetSite("neamb");

            using (new SiteContextSwitcher(website)) {
                var shortDescription = ((HtmlField)item.Fields[Templates.PageInfo.Fields.ShortDescription]);
                if (string.IsNullOrEmpty(shortDescription?.Value))
                    return string.Empty;

                var tags = new[] {
                    "sup"
                };
                var htmlField = EncodeHtmlTags(shortDescription.Value,
                    tags);
                var result = GetPlainText(htmlField);
                result = DecodeHtmlTags(result, tags);
                return result;
            }
        }
        private string EncodeHtmlTags(string input, string[] tags) {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            foreach (var tag in tags) {
                var supNodes = htmlDoc.DocumentNode.Descendants(tag);
                foreach (var supNode in supNodes.ToList())
                {
                    supNode.ParentNode.ReplaceChild(HtmlNode.CreateNode($"${tag}${supNode.InnerHtml}${tag}$"), supNode);
                }
            }
            return htmlDoc.DocumentNode.OuterHtml;
        }
        private string DecodeHtmlTags(string input, string[] tags) {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            var result = input;
            foreach (var tag in tags) {
                result = result.Replace($"${tag}$", $"<{tag}>");
            }
            return result;
        }
        public string GetPlainText(string input)
        {
            if (input == null)
                return (string)null;
            return HttpUtility.HtmlDecode(Regex.Replace(input, "<[^>]*>", string.Empty));
        }
    }
}