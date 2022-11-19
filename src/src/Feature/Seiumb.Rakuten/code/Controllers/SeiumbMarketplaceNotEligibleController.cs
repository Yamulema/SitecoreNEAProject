using Neambc.Neamb.Feature.Rakuten.Model;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.Rakuten.Controllers
{
    public class SeiumbMarketplaceNotEligibleController : BaseController
    {
        private readonly ISeiumbProfileManager _seiumbProfileManager;
        private readonly IEligibilityManagerMarketplaceSeiumb _eligibilityManagerMarketplaceSeiumb;
        public SeiumbMarketplaceNotEligibleController(IEligibilityManagerMarketplaceSeiumb eligibilityManagerMarketplaceSeiumb, 
            ISeiumbProfileManager seiumbProfileManager) {
            _eligibilityManagerMarketplaceSeiumb = eligibilityManagerMarketplaceSeiumb;
            _seiumbProfileManager = seiumbProfileManager;
        }
        public ActionResult ShowNotEligible() {
            var model = new StoreEligibilityDto();
            model.Initialize(RenderingContext.Current.Rendering);
            var mdsidInt = _seiumbProfileManager.GetProfile().MdsIdInt;
            model.HasEligibility = mdsidInt == 0 || _eligibilityManagerMarketplaceSeiumb.IsMemberEligible(mdsidInt);
            var storeUrlResultParameter = Request != null ? Request.QueryString[ConstantsNeamb.StoreResultUrlParameter] : "";
            if (!string.IsNullOrEmpty(storeUrlResultParameter) && storeUrlResultParameter.Equals("2")) model.HasToShowModal = true;
            return View("/Views/Seiumb.Marketplace/MarketplaceNotEligible.cshtml", model);
        }
    }
}