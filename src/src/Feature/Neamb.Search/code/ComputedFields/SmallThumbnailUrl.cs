using Sitecore.ContentSearch;
using Sitecore.Diagnostics;
using Sitecore.Sites;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Data.Fields;
using Sitecore.Links.UrlBuilders;

namespace Neambc.Neamb.Feature.Search.ComputedFields
{
    public class SmallThumbnailUrl : Sitecore.ContentSearch.ComputedFields.IComputedIndexField
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
            var website = Sitecore.Configuration.Factory.GetSite("neamb");
            using (new SiteContextSwitcher(website))
            {
                ImageField imageField = item.Fields[Templates.PageInfo.Fields.SmallThumbnail];
                var image = new MediaItem(imageField.MediaItem);

                var smallThumbnailUrl = MediaManager.GetMediaUrl(image, new MediaUrlBuilderOptions
                {
                    AlwaysIncludeServerUrl = false,
                    AbsolutePath = true
                });
                return string.IsNullOrEmpty(smallThumbnailUrl) ? string.Empty : smallThumbnailUrl;
            }
        }
    }
}