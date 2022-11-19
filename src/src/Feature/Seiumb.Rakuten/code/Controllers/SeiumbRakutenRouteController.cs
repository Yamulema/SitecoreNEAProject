using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Rakuten.Manager;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Rakuten.Manager;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.Rakuten.Controllers
{
    public class SeiumbRakutenRouteController : BaseController {
        private readonly IRakutenRegistrationSeiumbManager _rakutenRegistrationSeiumbManager;
        private readonly IEligibilityManagerMarketplaceSeiumb _eligibilityManagerMarketplaceSeiumb;
        private readonly IStoreManager _storeManager;
        private readonly IRakutenLog _rakutenLog;
        private readonly ISeiumbProfileManager _seiumbProfileManager;

        public SeiumbRakutenRouteController(IRakutenRegistrationSeiumbManager rakutenRegistrationSeiumbManager, 
            IEligibilityManagerMarketplaceSeiumb eligibilityManagerMarketplaceSeiumb,IStoreManager storeManager,
            IRakutenLog rakutenLog, ISeiumbProfileManager seiumbProfileManager) {
            _rakutenRegistrationSeiumbManager = rakutenRegistrationSeiumbManager;
            _eligibilityManagerMarketplaceSeiumb = eligibilityManagerMarketplaceSeiumb;
            _storeManager = storeManager;
            _rakutenLog = rakutenLog;
            _seiumbProfileManager = seiumbProfileManager;
        }

        /// <summary>
        /// Sign up process Seiumb to create the Rakuten Member
        /// </summary>
        /// <returns>ok or error</returns>
        public ActionResult SignUpSeiumb() {
             var cellCode = System.Web.HttpContext.Current.Session[ConstantsSeiumb.SourceCode] != null ? System.Web.HttpContext.Current.Session[ConstantsSeiumb.SourceCode].ToString() : string.Empty; 
             var resultSignUp = _rakutenRegistrationSeiumbManager.CheckSignUpRakutenUser(cellCode);
           
            if (!resultSignUp)
                return Json(new {result = "error",ebtoken = ""},JsonRequestBehavior.AllowGet);
            var rakutenResponse = _seiumbProfileManager.GetRakutenMemberCreationResponse();
            return Json(new {result = "ok",ebtoken = rakutenResponse.EBtoken}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetStoreLinkPartnerSeiumb(string storeIdSeiumb) {
            var seiuProfile = _seiumbProfileManager.GetProfile();
            var resultEligibility = _eligibilityManagerMarketplaceSeiumb.IsMemberEligible(seiuProfile.MdsIdInt);
            if (!resultEligibility)
                return Request != null && Request.UrlReferrer != null ? (ActionResult) Redirect(Request.UrlReferrer.AbsolutePath) : new HttpForbiddenResult();
            var url = _storeManager.GetShoppingLinkSeiumb(storeIdSeiumb);
            if (string.IsNullOrEmpty(url)) return Redirect(Request.UrlReferrer.AbsolutePath);
            _rakutenLog.Debug($"Shoping link. Storeid = {storeIdSeiumb}. Url= {url}");
            return Redirect(url);
        }
    }
}
