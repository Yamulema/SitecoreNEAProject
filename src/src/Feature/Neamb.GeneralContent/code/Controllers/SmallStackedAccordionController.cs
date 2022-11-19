using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class SmallStackedAccordionController : BaseController {
        private readonly IGtmService _gtmService;
        public SmallStackedAccordionController(IGtmService gtmService) {
            _gtmService = gtmService;
        }
        public ActionResult Item() {
            var item = RenderingContext.Current.Rendering.Item;
            var page = Sitecore.Context.Item;
            var model = new SmallStackedItem() {
                Item = page.ID != item?.ID ? item : null,
                OnClickEvent = _gtmService.GetOnClickEvent(GetFaq(page, item))
            };
            return View("/Views/Neamb.GeneralContent/SmallStackedAccordion/Item.cshtml", model);
        }

        private Faq GetFaq(Item page, Item item) {
            return new Faq() {
                Event = "faqs",
                ProductName = page.Fields[Templates.Product.Fields.PageTitle]?.Value ?? string.Empty,
                FaqQuestion = item?[Templates.SmallAccordionItem.Fields.Header]
            };
        }
    }
}