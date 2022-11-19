using System.Web.Mvc;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Controllers {
	public class CarouselPlanCardsController : BaseController {
		public ActionResult CarouselPlanCards() {
			return View("/Views/Neamb.Cards/CarouselPlanCards.cshtml", CreateModel());
		}

		private CarouselPlanCardsDTO CreateModel() {
			var carouselPlanCards = new CarouselPlanCardsDTO();
			carouselPlanCards.Initialize(RenderingContext.Current.Rendering);
			return carouselPlanCards;
		}
	}
}