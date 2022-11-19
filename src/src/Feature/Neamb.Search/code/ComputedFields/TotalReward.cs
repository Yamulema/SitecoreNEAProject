using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Sites;
using System;
using System.Globalization;
using System.Linq;
using static System.Decimal;

namespace Neambc.Neamb.Feature.Search.ComputedFields
{
    [Obsolete]
    public class TotalReward : IComputedIndexField
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
            var website = Sitecore.Configuration.Factory.GetSite("neamb"); //check

            using (new SiteContextSwitcher(website))
            {
                var reward = item.Children.FirstOrDefault(x => x.Name.Equals(Templates.Store.Child.TotalChild));
                var itemAmount = reward?.Fields[Templates.StoreReward.Fields.Amount];
                TryParse(itemAmount?.Value, out var amount);
                Log.Debug($"Found value {amount} for item {reward?.ID} in Base Reward", this);

                var itemSign = reward?.Fields[Templates.StoreReward.Fields.Display];

                //Determine sign to be used on return reward value
                switch (itemSign?.Value) {
                    case "Percentage":
                        return $"{amount}%";
                    case "Fixed":
                        return $"${amount}";
                    default:
                        Log.Warn($"Unexpected reward type with value: {itemSign?.Value} on conversion for reward {reward?.ID}", this);
                        return amount.ToString(CultureInfo.InvariantCulture);
                }
            }
        }
    }
}