using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.GeneralContent.Models
{
    public class FourStepsCarousel
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public bool HasStep1 { get; set; }
        public bool HasStep2 { get; set; }
        public bool HasStep3 { get; set; }
        public bool HasStep4 { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
        }
    }
}