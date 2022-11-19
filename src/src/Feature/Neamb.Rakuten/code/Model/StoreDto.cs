using Neambc.Neamb.Foundation.Eligibility.Model;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Rakuten.Model
{
    public class StoreDto : IRenderingModel
    {
        public string StoresListLabel { get; set; }
        public string MoreDealsText { get; set; }
        public string NoFavoriteStoresContent { get; set; }
        public string ShopingLink { get; set; }
        public string MemberEmail { get; set; }
        public EligibilityResultEnum Eligibility { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public string ProductCode { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
        }
    }
}