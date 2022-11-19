using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class SavingCalculatorDto
    {
        public List<List<SavingCalculatorItem>> SavingCalculatorItems { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            SavingCalculatorItems = new List<List<SavingCalculatorItem>>();
        }
    }
}