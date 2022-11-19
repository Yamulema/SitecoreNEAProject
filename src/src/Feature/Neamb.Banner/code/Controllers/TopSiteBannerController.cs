using System.Web.Mvc;
using Neambc.Neamb.Feature.Banner.Models;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Banner.Controllers {
	public class TopSiteBannerController : BaseController {
        private readonly ISessionManager _sessionManager;
        public TopSiteBannerController(ISessionManager sessionManager) {
            _sessionManager = sessionManager;
        }

        #region ActionResult Methods
        public ActionResult TopSiteBanner() {
			return View("/Views/Neamb.Banner/TopSiteBanner.cshtml", CreateModel());
		}

        [HttpPost]
        public ActionResult CloseBanner()
        {
            try {
                _sessionManager.StoreInSession<bool>(ConstantsNeamb.TopBanner, true);
                return Json(new
                    {
                        result = "ok"
                    },
                    JsonRequestBehavior.AllowGet);
            } catch {
                return Json(new
                    {
                        result = "error"
                    },
                    JsonRequestBehavior.AllowGet);
            }
            
        }

        private  TopSiteBannerDto CreateModel() {
			var model = new TopSiteBannerDto();
            model.Initialize(RenderingContext.Current.Rendering);
            if (!model.IsHidden) {
                model.IsHidden = _sessionManager.RetrieveFromSession<bool>(ConstantsNeamb.TopBanner);
            }
            return model;
		}

		
		#endregion
	}
}