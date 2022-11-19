using Neambc.Neamb.Feature.GeneralContent.Models;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class TopicListingController : BaseController {

        public ActionResult TopicListing()
        {
            return View("/Views/Neamb.GeneralContent/Renderings/TopListing.cshtml", CreateModel());
        }

        private TopicListing CreateModel()
        {
            var renderingItem = RenderingContext.Current.Rendering.Item;
            var model = new TopicListing();

            model.Initialize(RenderingContext.Current.Rendering);
            model.Headline = renderingItem[Templates.TopListing.Fields.HeadlineText];
            model.ExpandText = renderingItem[Templates.TopListing.Fields.ExpandText];
            model.CollapseText = renderingItem[Templates.TopListing.Fields.CollapseText];

            var items = ((Sitecore.Data.Fields.MultilistField) renderingItem.Fields[
                Templates.TopListing.Fields.Topics]).GetItems();

            foreach (var topic in items)
                model.Topics.Add(new Topic {
                    Name = topic.Fields[Templates.PageInfo.Fields.PageTitle].Value,
                    Url = LinkManager.GetItemUrl(topic)
                });

            return model;
        }
    }
}