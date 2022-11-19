using System.Web.Mvc;
using Neambc.Neamb.Feature.Banner.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Banner.Controllers {
	public class TwoColumnHeroController : BaseController {
        #region ActionResult Methods
        public ActionResult TwoColumnHero() {
			return View("/Views/Neamb.Banner/2ColumnHero.cshtml", CreateModel());
		}
        private  TwoColumnHeroDto CreateModel() {
			var model = new TwoColumnHeroDto();
            model.Initialize(RenderingContext.Current.Rendering);
            return model;
		}
		#endregion
	}
}