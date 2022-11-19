using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Banner.Models
{
    public class TwoColumnHeroDto : IRenderingModel
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public string ClassButton { get; set; }
        public string ClassBackground { get; set; }
        public bool IsDisplayHeroCtaLink { get; set; }
        public bool IsDisplayOverlayLink { get; set; }
        public bool IsVisible { get; set; }

        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            if (Item.IsDerived(Templates.TwoColumnHeroBanner.ID)) {
                IsVisible = true;
            } else {
                IsVisible = false;
            }
            
            PageItem = PageContext.Current.Item;
            var buttonColorReferenced = Item[Templates.TwoColumnHeroBanner.Fields.HeroCtaColor];
            if (!string.IsNullOrEmpty(buttonColorReferenced)) {
                var buttonColorItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(buttonColorReferenced));
                var buttonColorValue = buttonColorItem[Templates.CategoryItem.Fields.Value];
                ClassButton = $"btn btn-{buttonColorValue}";
            } else {
                ClassButton = $"btn btn";
            }
            var backgroundColorReferenced = Item[Templates.TwoColumnHeroBanner.Fields.BackgroundColor];
            if (!string.IsNullOrEmpty(backgroundColorReferenced))
            {
                var backgroundColorItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(backgroundColorReferenced));
                ClassBackground = backgroundColorItem[Templates.CategoryItem.Fields.Value];
            }
            
            if (!string.IsNullOrEmpty(Item[Templates.TwoColumnHeroBanner.Fields.HeroCtaLink])) {
                IsDisplayHeroCtaLink = true;
            } else {
                IsDisplayHeroCtaLink = false;
            }
            if (!string.IsNullOrEmpty(Item[Templates.TwoColumnHeroBanner.Fields.OverlayLink]))
            {
                IsDisplayOverlayLink = true;
            }
            else
            {
                IsDisplayOverlayLink = false;
            }

        }
    }
}