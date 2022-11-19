using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.ComponentModel.DataAnnotations;

namespace Neambc.Seiumb.Feature.Forms.Models
{
    public class UnsubscribeModel : IRenderingModel
    {
        public Item Item { get; set; }
        public bool IsSucess { get; set; }
        public void Initialize(Rendering rendering)
        {
            Item = rendering.Item;
            IsSucess = false;
        }        
    }
}