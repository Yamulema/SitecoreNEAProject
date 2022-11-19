using System;
using System.Linq;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Sites;
using Sitecore.Syndication;

namespace Neambc.Neamb.Feature.Search.ComputedFields
{
    public class FriendlyUrl : Sitecore.ContentSearch.ComputedFields.IComputedIndexField
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
            {
                return string.Empty;
            }

            if (item.Paths.IsMediaItem)
            {
                if (item.TemplateID != TemplateIDs.MediaFolder
                    && item.ID != ItemIDs.MediaLibraryRoot)
                {
                    return LinkManager.GetItemUrl(item);
                }
            }

            if (!item.Paths.IsContentItem)
            {
                return string.Empty;
            }
            var website = Sitecore.Configuration.Factory.GetSite("neamb");

            using (new SiteContextSwitcher(website))
            {
                var options = new ItemUrlBuilderOptions
                {
                    AlwaysIncludeServerUrl = false,
                    UseDisplayName = false,
                    LanguageEmbedding = LanguageEmbedding.Never
                };
                var url = LinkManager.GetItemUrl(item, options);

                return item.Database.Resources.Devices.GetAll()
                    .Where(x => x.ID != FeedUtil.FeedDeviceId || !FeedUtil.IsFeed(item))
                    .Any(x => item.Visualization.GetLayout(x) != null)
                    ? url.Replace(" ", "-")
                    : string.Empty;
            }
        }
    }
}