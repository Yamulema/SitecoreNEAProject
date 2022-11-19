using Neambc.Neamb.Foundation.Configuration.Pipelines;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class PageBodyDTO : IRenderingModel
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public string BodyBackgroundColorClass { get; set; }
        public string BodyHeightLimit { get; set; }
        public bool HasBodyHeight { get; set; }
        public IStringProcessor StringProcessor { get; set; }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
        }
    }
}