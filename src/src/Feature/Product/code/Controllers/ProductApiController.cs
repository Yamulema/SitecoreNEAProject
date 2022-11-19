using Fluentx.Mvc;
using Neambc.Neamb.Foundation.Config.Utility;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Services.ProductEligibility;
using Neambc.Neamb.Foundation.MBCData.Services.SecurityManagement;
using Neambc.Seiumb.Feature.Product.Repositories;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Neambc.Seiumb.Foundation.WebServices.Managers;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.String;

namespace Neambc.Seiumb.Feature.Product.Controllers
{
    public class ProductApiController : BaseController {
        private readonly IProductRepository _productRepository;
        private readonly INeambServiceManager _neambServiceManager;
        private readonly ISecurityService _securityService;
        private readonly IProductRestManagerSeiumb _productRestManagerSeiumb;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IUserRepository _userRepository;
        private readonly ISeiumbProfileManager _seiumbProfileManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productRepository"></param>
        public ProductApiController(IProductRepository productRepository, INeambServiceManager neambServiceManager, ISecurityService securityService, 
            IProductRestManagerSeiumb productRestManagerSeiumb, IGlobalConfigurationManager globalConfigurationManager, IUserRepository userRepository,
            ISeiumbProfileManager seiumbProfileManager) {
            _productRepository = productRepository;
            _neambServiceManager = neambServiceManager;
            _securityService = securityService;
            _productRestManagerSeiumb = productRestManagerSeiumb;
            _globalConfigurationManager = globalConfigurationManager;
            _userRepository = userRepository;
            _seiumbProfileManager = seiumbProfileManager;
        }

        [HttpPost]

        public ActionResult DownloadEfulfillmentPdf(string materialId, string mdsid) {
            if (_userRepository.GetUserStatus().Equals(UserStatusCons.HOT)) {
                var urlpdf = _productRepository.GetPdfFile(materialId);
                //Insert into order_fulfill
                _productRepository.ExecuteMdsLoggingProcessMaterial(materialId, mdsid);
                return Redirect(!IsNullOrEmpty(urlpdf) ? urlpdf : "/EFulfillment Error");
            }
            if (Request != null && Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.OriginalString);
            var homeItem = Sitecore.Context.Database.GetItem(Templates.Home.ID);
            return Redirect(LinkManager.GetItemUrl(homeItem));
        }

        [HttpPost]

        public ActionResult GetTrueCarPartner(string materialId) {
            var seiuProfile = _seiumbProfileManager.GetProfile();

            if (_userRepository.GetUserStatus().Equals(UserStatusCons.HOT)) {
                Sitecore.Diagnostics.Log.Info($"Sending mdsid to TrueCar: {seiuProfile.MdsId.PadLeft(9, '0')}", this);
                var action = "https://seiu.truecar.com/index.html";
                var memberId = seiuProfile.MdsId.PadLeft(9, '0');
                var parameters =
                    $"givenName={seiuProfile.FirstName}&familyName={seiuProfile.LastName}&emailAddress={seiuProfile.Email}&addressLine1={seiuProfile}&" +
                    $"city={seiuProfile.City}&state={seiuProfile.Status}&postalCode={seiuProfile.ZipCode}&telephone={seiuProfile.Phone}" +
                    $"&memberId={memberId}&referrer_id={_globalConfigurationManager.ReferrerIdSeiumb}";
                try {
                    var nameValueCollection = new NameValueCollection(1) {{"x-truecar-app-token", _globalConfigurationManager.HeaderTrueCarTokenSeiumb}};
                    var initialurl = _globalConfigurationManager.UrlTrueCarSeiumb;
                    var myRequest = new WebRequestHelper(initialurl, "POST", parameters, nameValueCollection);
                    action = $"http://{myRequest.GetResponse()}";
                } catch (Exception ex) {
                    Sitecore.Diagnostics.Log.Error(this + "Error calling the url of TrueCar " + DateTime.Now, ex, this);
                }
                return Redirect(action);
            }
            if (Request != null && Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.OriginalString);
            var homeItem = Sitecore.Context.Database.GetItem(Templates.Home.ID);
            return Redirect(LinkManager.GetItemUrl(homeItem));
        }

        [HttpPost]

        public void ExecuteStoredProcedureOrder(string programCode, string mdsid) {
            if(string.IsNullOrEmpty(mdsid))
            {
                var seiuProfile = _seiumbProfileManager.GetProfile();
                mdsid = seiuProfile.MdsId;
            }           

            _productRepository.ExecuteMdsLoggingProcessCta(programCode, mdsid);
        }

        /// <summary>
        /// Verify the user is authenticated in seiumb
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        public ActionResult VerifyAuthenticationSeiumb() {
            return Json(_userRepository.GetUserStatus().Equals(UserStatusCons.HOT) ? new {results = "OK"} : new {results = "Error"}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExecuteLink(string contextitemid, string calllinkid,string postparameterid) {
            var db = Sitecore.Context.Database;
            var contextItem = db.GetItem(new ID(contextitemid));
            var hasCheckEligibility = contextItem.Fields[Templates.ProductLite.Fields.Eligibility].IsChecked();
            var resultCheckEligibility = false;
            var seiuProfile = _seiumbProfileManager.GetProfile();

            if (hasCheckEligibility)
                try
                {
                    int.TryParse(seiuProfile.MdsId, out var mdsidInt);
                    resultCheckEligibility = _productRestManagerSeiumb.GetEligibility(mdsidInt);
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error($"Error executing GetEligibility Seiumb, mdsid {seiuProfile.MdsId}", ex, this);
                    return new HttpForbiddenResult();
                }

            if ((!resultCheckEligibility || !hasCheckEligibility) && hasCheckEligibility) return new HttpForbiddenResult();
            var urlaction = contextItem.LinkFieldUrl(new ID(calllinkid));
            var result = SetTokensString(urlaction);
            var postParams = GetPostParams(contextItem, postparameterid);
            if (postParams.Count > 0) return this.RedirectAndPost(result, postParams);
            return Redirect(result);
        }

        private string SetTokensString(string urlaction) {
            var result = Empty;
            var mdsIndvId = _seiumbProfileManager.GetProfile().MdsId.PadLeft(9, '0');
            if (urlaction.Contains("[mdsid]")) {
                result = urlaction.Replace("[mdsid]", mdsIndvId);
            } else if (urlaction.Contains("[mdsid_afinium]")) {
                var encryptedMdsid = _neambServiceManager.EncryptPartner(mdsIndvId);
                result = urlaction.Replace("[mdsid_afinium]", HttpUtility.UrlEncode(encryptedMdsid));
            } else if (urlaction.Contains("[afinium]")) {
                var encryptedMdsid = _securityService.AesEncrypt(mdsIndvId, Token.Afinium);
                if (IsNullOrEmpty(encryptedMdsid)) throw new ApplicationException($"Error with afinium encryption process Seiumb");
                result = urlaction.Replace("[afinium]", encryptedMdsid);
            } else {
                result = urlaction;
            }

            if (result.Contains("[cellcode]")) {
                var cellCode = System.Web.HttpContext.Current.Session[ConstantsSeiumb.SourceCode] != null
                    ? System.Web.HttpContext.Current.Session[ConstantsSeiumb.SourceCode].ToString() : Empty;
                result = result.Replace("[cellcode]", HttpUtility.UrlEncode(cellCode));
            }

            if (!result.Contains("[campcode]")) return result;
            var campaignCode = System.Web.HttpContext.Current.Session[ConstantsSeiumb.CampaignCode] != null
                ? System.Web.HttpContext.Current.Session[ConstantsSeiumb.CampaignCode].ToString() : null;
            result = result.Replace("[campcode]", HttpUtility.UrlEncode(campaignCode));
            return result;
        }

        private Dictionary<string, object> GetPostParams(Item contextItem, string postparameterid)
        {
            var parameters = new Dictionary<string, object>();
            try
            {
                Sitecore.Diagnostics.Log.Debug($"GetPostParams PostDataItemId: {postparameterid}", this);
                var rawText = contextItem[new ID(postparameterid)] ?? Empty;
                var text = rawText.Replace("{", Empty).Replace("}", Empty);

                foreach (var entry in text.Split(new[] { "\r\n" }, StringSplitOptions.None))
                {
                    var parameter = entry.Split(new[] { ":" }, StringSplitOptions.None);
                    if (parameter.Length == 2)
                        parameters.Add(parameter.First(), SetTokensString(parameter.Last()));
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"Error GetPostParams", ex, this);
            }
            return parameters;
        }
    }
}