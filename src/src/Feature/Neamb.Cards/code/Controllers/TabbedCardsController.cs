using System.Web.Mvc;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Controllers {
	public class TabbedCardsController : BaseController {

        #region ActionResult Methods
        public ActionResult TabbedCards() {
            return View("/Views/Neamb.Cards/TabbedCards.cshtml", CreateModel());
		}
        #endregion

        #region Static Methods
        private TabbedCardsDTO CreateModel()
        {
            var tabbedCards = new TabbedCardsDTO();
            tabbedCards.Initialize(RenderingContext.Current.Rendering);
            return tabbedCards;
        }
        #endregion
    }
}