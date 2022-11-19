using Neambc.Neamb.Foundation.Configuration.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Fields;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public class GridItemDTO : HeaderDTO, IRenderingModel
    {
        public string BackgroundColorClass { get; private set; }
		public string GtmAction { get; set; }
		public string UrlCta { get; set; }
		public string UrlText { get; set; }
		public string TargetCta { get; set; }
		public new void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            BackgroundColorClass = GetClass(Item.Fields[Templates.GridItem.Fields.BackgroundColor].Value);
			LinkField ctaField = rendering.Item.Fields[Templates.GridItem.Fields.CTA];
			UrlText = ctaField.Text;
			
			UrlCta = rendering.Item.LinkFieldUrl(Templates.GridItem.Fields.CTA);

			if (!string.IsNullOrEmpty(ctaField.Target)) {
				TargetCta = "_blank";
			}
			HasSubheadline = HasValueField(Templates.GridItem.Fields.Subhead);
        }

        private string GetClass(string value)
        {
            var colorClass = ConstantsNeamb.GrayBackgroundColorClass;
            if (value == ConstantsNeamb.GreenBackgroundColor)
            {
                colorClass = ConstantsNeamb.GreenBackgroundColorClass;
            }
            return colorClass;
        }
    }
}