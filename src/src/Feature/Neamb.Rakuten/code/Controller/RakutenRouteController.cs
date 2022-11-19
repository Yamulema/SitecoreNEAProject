using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Rakuten.Manager;

namespace Neambc.Neamb.Feature.Rakuten.Controller
{
    public class RakutenRouteController : BaseController {
        private readonly IStoreManager _storeManager;
        private readonly IRakutenLog _rakutenLog;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly IEligibilityManagerMarketplace _eligibilityManagerMarketplace;
        private readonly IRakutenMemberManager _rakutenMemberManager;

        public RakutenRouteController(
            IStoreManager storeManager,
            IRakutenLog rakutenLog,
            IGlobalConfigurationManager globalConfigurationManager,
            ISessionAuthenticationManager sessionAuthenticationManager,
            IEligibilityManagerMarketplace eligibilityManagerMarketplace, IRakutenMemberManager rakutenMemberManager
        ) {
            _storeManager = storeManager;
            _rakutenLog = rakutenLog;
            _globalConfigurationManager = globalConfigurationManager;
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _eligibilityManagerMarketplace = eligibilityManagerMarketplace;
            _rakutenMemberManager = rakutenMemberManager;
        }

        [HttpPost]
        public ActionResult GetStoreLinkPartner(string storeId) {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            var productCode = _globalConfigurationManager.ProductCodeStores;
            var resultEligibility = _eligibilityManagerMarketplace.IsMemberEligible(accountMembership.Mdsid, productCode);
            if (resultEligibility == EligibilityResultEnum.Eligible) {

                var url = _storeManager.GetShoppingLink(storeId);
                if (!string.IsNullOrEmpty(url)) {
                    _rakutenLog.Debug($"Shoping link. Storeid = {storeId}. Url= {url}");
                    return Redirect(url);
                } else {
                    return Redirect(Request.UrlReferrer.AbsolutePath);
                }
            } else {
                if (Request != null && Request.UrlReferrer != null) {
                    return Redirect(Request.UrlReferrer.AbsolutePath);
                } else {
                    return new HttpForbiddenResult();
                }
            }
        }

        /// <summary>
        /// Sign up process
        /// </summary>
        /// <returns>ok or error</returns>
        public ActionResult SignUp() {
            var resultSignUp = _rakutenMemberManager.CheckSignUpRakutenUser();

            if (resultSignUp) {
                var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
                return Json(new {
                    result = "ok",
                    ebtoken = accountMembership.Profile.RakutenProfile.EBToken
                }, JsonRequestBehavior.AllowGet);
            } else {
                return Json(new {
                        result = "error",
                        ebtoken = ""
                    },
                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}
