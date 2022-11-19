using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Web;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class ChatDTO
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public HtmlString Chat { get; set; }
        public HtmlString DefaultChat { get; set; }
        public bool IsDefaultChat { get; set; }
    }
}