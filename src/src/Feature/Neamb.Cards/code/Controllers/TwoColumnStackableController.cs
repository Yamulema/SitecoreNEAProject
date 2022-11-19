using System.Web.Mvc;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Controllers {
	public class TwoColumnStackableController : BaseController {

		public ActionResult TwoColumnStackable() {
			return View("/Views/Neamb.Cards/TwoColumnStackable.cshtml", CreateModel());
		}

		private TwoColumnStackableDTO CreateModel() {
			var twoColumnStackableDto = new TwoColumnStackableDTO();
			twoColumnStackableDto.Initialize(RenderingContext.Current.Rendering);
			return twoColumnStackableDto;
		}
	}
}