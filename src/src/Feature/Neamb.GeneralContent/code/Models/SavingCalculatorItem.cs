using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class SavingCalculatorItem
    {
        public Item SavingCalculatorProductItem { get; set; }
        public string Icon { get; set; }
        public void Initialize(Rendering rendering)
        {
            SavingCalculatorProductItem = rendering.Item;
        }
    }
}