using Neambc.Neamb.Feature.Cards.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Utility;

namespace Neambc.Neamb.Feature.Cards.Controllers
{
    public class GridItemController : BaseController
    {
		private readonly IGtmService _gtmService;

		public GridItemController(IGtmService gtmService) {
			_gtmService = gtmService;
		}
		public ActionResult GridItem()
        {
            return View("/Views/Neamb.Cards/GridItem.cshtml", CreateModel());
        }

        private GridItemDTO CreateModel()
        {
            var gridItemDTO = new GridItemDTO();
            gridItemDTO.Initialize(RenderingContext.Current.Rendering);
			gridItemDTO.GtmAction = _gtmService.GetGtmEvent(new ContentArticle() {
				Event = "content",
				ContentTitle = gridItemDTO.Item[Templates.GridItem.Fields.Header],
				ContentLocation = "tools and guides"
			});

			return gridItemDTO;
        }
    }
}