using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Carousel.Models
{
    public class CarouselModel : IRenderingModel
    {
        public string Time { get; set; }
        public List<SlideModel> Slides { get; set; }

        public HtmlString Headline { get; set; }
        public HtmlString Subheadline { get; set; }
        public LinkField ReadMore { get; set; }
        public ImageField Image { get; set; }

        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            Time = FieldRenderer.Render(Item, "Time") + "000";

            var slidesList = new List<SlideModel>();

            var slideItemList = rendering.Item[Templates.Carousel.Fields.Slides];
            if (!string.IsNullOrEmpty(slideItemList))
            {
                var slidesListValues = slideItemList.Split('|').Take(5);

                foreach (var id in slidesListValues)
                {
                    var itemBdd = Sitecore.Context.Database.GetItem(new ID(id));
                   
                    var model = new SlideModel
                    {
                        Slide = itemBdd
                    };
                    slidesList.Add(model);
                }
            }
            Slides = slidesList;
        }
    }
}