using System.Web.Mvc;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Controllers {
	public class ThreeColMultirowContentItemsController : BaseController {
		public ActionResult ThreeColMultirowContentItems() {
			return View("/Views/Neamb.Cards/ThreeColMultirowContentItems.cshtml", CreateModel());
		}

		private ThreeColMultirowContentItemsDTO CreateModel() {
			var threeMultirowContentItemsDTO = new ThreeColMultirowContentItemsDTO();
			threeMultirowContentItemsDTO.Initialize(RenderingContext.Current.Rendering);
			return threeMultirowContentItemsDTO;
		}
	}
}