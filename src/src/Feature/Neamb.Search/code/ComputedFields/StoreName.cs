using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Sites;
using System;

namespace Neambc.Neamb.Feature.Search.ComputedFields
{
    public class StoreName : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        public object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");

            if (!(indexable is SitecoreIndexableItem scIndexable))
            {
                Log.Warn(this + " : unsupported IIndexable type : " + indexable.GetType(), this);
                return string.Empty;
            }

            var item = (Item)scIndexable;

            if (item == null)
            {
                Log.Warn(this + " : unsupported SitecoreIndexableItem type : " + scIndexable.GetType(), this);
                return string.Empty;
            }

            // optimization to reduce indexing time
            // by skipping this logic for items in the Core database
            if (string.Compare(item.Database.Name, "core", StringComparison.OrdinalIgnoreCase) == 0)
                return string.Empty;

            if (!item.Paths.IsContentItem) return string.Empty;
            var website = Sitecore.Configuration.Factory.GetSite("neamb");

            using (new SiteContextSwitcher(website))
            {
                var storeName = item.Fields[Templates.Store.Fields.Name]?.Value;
                return storeName?.ToLower(item.Language.CultureInfo).Replace(" ", "");
            }
        }
    }
}