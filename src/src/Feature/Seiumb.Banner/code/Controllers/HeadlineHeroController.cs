using Neambc.Seiumb.Feature.Banner.Models;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Resources.Media;

namespace Neambc.Seiumb.Feature.Banner.Controllers
{
    public class HeadlineHeroController : BaseController {
		#region ActionResult Methods
		public ActionResult HeadlineHero() {
            var model = CreateModel();
            return View("/Views/Seiumb.Banner/HeadlineHero.cshtml", model);
		}
        #endregion

		#region Static Methods
		private static HeadlineHero CreateModel() {
			var model = new HeadlineHero();
			model.Initialize(RenderingContext.Current.Rendering);
            model.PageTitle = model.Item.Fields[Templates.HeadlineHero.Fields.PageTitle]?.Value;
            model.Subheadline = model.Item.Fields[Templates.HeadlineHero.Fields.Subheadline]?.Value;
            model.HeroImage = model.Item.ImageUrl(Templates.HeadlineHero.Fields.HeroImage);
            return model;
		}
		#endregion
	}
}