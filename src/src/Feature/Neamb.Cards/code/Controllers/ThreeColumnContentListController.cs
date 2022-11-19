using System.Web.Mvc;
using Neambc.Neamb.Feature.Cards.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Cards.Controllers {
	public class ThreeColumnContentListController : BaseController {

        #region ActionResult Methods
        public ActionResult ThreeColumnContentList() {
            return View("/Views/Neamb.Cards/ThreeColumnContentList.cshtml", CreateModel());
		}
        #endregion

        #region Static Methods
        private ThreeColumnContentListDTO CreateModel()
        {
            var model = new ThreeColumnContentListDTO();
            model.Initialize(RenderingContext.Current.Rendering);
            return model;
        }
        #endregion
    }
}