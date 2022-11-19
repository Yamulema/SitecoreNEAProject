using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.Banner.Models
{
    public class HeadlineHero : IRenderingModel
    {
        public string PageTitle { get; set; }
        public string Subheadline { get; set; }
        public string HeroImage { get; set; }

        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
        }
    }
}