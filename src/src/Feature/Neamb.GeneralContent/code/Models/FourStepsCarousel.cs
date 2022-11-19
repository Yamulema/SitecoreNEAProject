using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class FourStepsCarousel : IRenderingModel
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public bool HasStep1 { get; set; }
        public bool HasStep2 { get; set; }
        public bool HasStep3 { get; set; }
        public bool HasStep4 { get; set; }
        public bool HasStep5 { get; set; }
        public bool HasStep6 { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
        }
    }
}