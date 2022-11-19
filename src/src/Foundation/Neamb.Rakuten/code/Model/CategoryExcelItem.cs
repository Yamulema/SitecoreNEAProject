using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Rakuten.Model
{
    public class CategoryExcelItem
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public int? Level { get; set; }
        public Item ItemSitecore { get; set; }
        public Item ItemParentSitecore { get; set; }
    }
}