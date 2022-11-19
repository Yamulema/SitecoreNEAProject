using System.Web.Mvc;
using Neambc.Neamb.Feature.Rakuten.Model;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Rakuten.Manager;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Rakuten.Controllers
{
    public class MarketplaceNotEligibleController : BaseController
    {
        private readonly IStoreManager _storeManager;
        public MarketplaceNotEligibleController(IStoreManager storeManager)
        {
            _storeManager = storeManager;
        }
        public ActionResult ShowNotEligible()
        {
            var model = new StoreEligibilityDto();
            model.Initialize(RenderingContext.Current.Rendering);
            model.HasEligibility = _storeManager.CheckEligibilityUser();
            var storeUrlResultParameter = Request != null ? Request.QueryString[ConstantsNeamb.StoreResultUrlParameter] : "";
            if (!string.IsNullOrEmpty(storeUrlResultParameter) && storeUrlResultParameter.Equals("2"))
            {
                model.HasToShowModal = true;
            }
            return View("/Views/Neamb.Marketplace/MarketplaceNotEligible.cshtml", model);
        }
    }
}