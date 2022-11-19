using Neambc.Neamb.Foundation.Eligibility.Model;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Rakuten.Model
{
    public class StoreEligibilityDto : IRenderingModel
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public bool HasEligibility { get; set; }
        public bool HasToShowModal { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            HasEligibility = false;
            HasToShowModal = false;
        }
    }
}