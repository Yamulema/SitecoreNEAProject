using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public abstract class PageCard
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Cta { get; set; }
        public Item Item { get; set; }
        public string OnClickEvent { get; set; }
    }
}