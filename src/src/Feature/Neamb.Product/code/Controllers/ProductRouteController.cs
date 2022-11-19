using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using Fluentx.Mvc;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Product.Controllers {
	public class ProductRouteController : BaseController {
		private readonly IProductManager _productmanager;
		private readonly IOracleDatabase _oracleManager;
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly IAuthenticationAccountManager _authenticationAccountManager;
		private readonly ILinkActionTypeManager _linkActionTypeManager;
        private readonly IOmniActionTypeManager _omniActionTypeManager;
		private readonly IEfulfillmentActionTypeManager _efulfillmentActionTypeManager;
		private readonly ISsoActionTypeManager _ssoActionTypeManager;
		private readonly IDatapassActionTypeManager _datapassActionTypeManager;
		private readonly IComingSoonManager _comingSoonManager;
        
        public ProductRouteController(
			IProductManager productmanager,
			IOracleDatabase oracleManager, IGlobalConfigurationManager globalConfigurationManager,
			ISessionAuthenticationManager sessionAuthenticationManager,
			IAuthenticationAccountManager authenticationAccountManager, ILinkActionTypeManager linkActionTypeManager,
			IEfulfillmentActionTypeManager efulfillmentActionTypeManager,
			ISsoActionTypeManager ssoActionTypeManager, IDatapassActionTypeManager datapassActionTypeManager,
			IComingSoonManager comingSoonManager,
            IOmniActionTypeManager omniActionTypeManager) {
			_productmanager = productmanager;
			_oracleManager = oracleManager;
			_globalConfigurationManager = globalConfigurationManager;
			_sessionAuthenticationManager = sessionAuthenticationManager;
			_authenticationAccountManager = authenticationAccountManager;
			_linkActionTypeManager = linkActionTypeManager;
			_efulfillmentActionTypeManager = efulfillmentActionTypeManager;
			_ssoActionTypeManager = ssoActionTypeManager;
			_datapassActionTypeManager = datapassActionTypeManager;
			_comingSoonManager = comingSoonManager;
            _omniActionTypeManager = omniActionTypeManager;
        }

		#region Efulfillment

		/// <summary>
		/// Download the PDF file action in Efulfillment
		/// </summary>
		/// <param name="materialIdMultirow">Material id</param>
		/// <param name="productcodepdfMultirow">Product code</param>
		/// <returns></returns>
		[HttpPost]
		
		public ActionResult DownloadEfulfillmentPdfMultirow(string materialIdMultirow, string productcodepdfMultirow,string checkOmniPdf) {
			return DownloadPdf(materialIdMultirow, productcodepdfMultirow, checkOmniPdf, true);
		}

		/// <summary>
		/// Download the PDF file action in Efulfillment
		/// </summary>
		/// <param name="materialIdCta">Material id</param>
		/// <returns></returns>
		[HttpPost]
		
		public ActionResult DownloadEfulfillmentPdfCta(string materialIdCta) {
			return DownloadPdf(materialIdCta, "","0", false);
		}

		/// <summary>
		/// Download the PDF file action in Efulfillment
		/// </summary>
		/// <param name="materialId">Material id</param>
		/// <param name="productcodepdf">Product code</param>
		/// <param name="checkEligibility">Flag to check eligibility</param>
		/// <returns></returns>
		private ActionResult DownloadPdf(string materialId, string productcodepdf, string checkOmniPdf, bool checkEligibility = true) {
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			if (string.IsNullOrEmpty(accountMembership.Mdsid) && Request != null && Request.UrlReferrer != null) {
				return Redirect(Request.UrlReferrer.OriginalString);
			} else {
                if (checkOmniPdf.Equals("1"))
                {
                    var resultOmniChannel = GetLinkOmni(accountMembership, productcodepdf);
                    if (!string.IsNullOrEmpty(resultOmniChannel.Url))
                    {
                        return HandleResponseProductActionType(
                            resultOmniChannel,
                            productcodepdf,
                            "Efullfilment process doesn't return data productcode"
                        );
                    }
                    else
                    {
                        return GetDownloadPdf(materialId, productcodepdf, checkEligibility, accountMembership);
                    }

                } else {

                    return GetDownloadPdf(materialId, productcodepdf, checkEligibility, accountMembership);
                }
            }
		}
        private ActionResult GetDownloadPdf(
            string materialId,
            string productcodepdf,
            bool checkEligibility,
            AccountMembership accountMembership
        ) {
            var accountUser = _productmanager.GetAccountUser(accountMembership);
            var efulfillmentModel = new EfulfillmentModel {
                CheckEligibility = checkEligibility,
                MaterialId = materialId,
                ProductCode = productcodepdf,
                CheckLogin = true,
            };
            efulfillmentModel.AccountUser = accountUser;

            var resultEfulfillment = _efulfillmentActionTypeManager.GetPdfFile(efulfillmentModel);
            if (!string.IsNullOrEmpty(resultEfulfillment.PdfSucessUrl)) {
                return Redirect(resultEfulfillment.PdfSucessUrl);
            }
            return HandleResponseProductActionType(resultEfulfillment,
                materialId,
                "The Pdf requested couldn't retrieved materialid");
        }

        /// <summary>
		/// MSC Efulfillment process
		/// </summary>
		/// <param name="matid">material id</param>
		/// <param name="mdsId">mdsId</param>
		/// <returns>PDF or error page</returns>
		public ActionResult ProcessExternalEfulfillment(string matid, string mdsId) {
			var pathErrorExternalEfulfillment = GetErrorPageExternalEfulfillment();
			if (!string.IsNullOrEmpty(mdsId) && !string.IsNullOrEmpty(matid)) {
				var account = new AccountMembership();
				_authenticationAccountManager.RetrieveAccount(account, mdsId);
				//Get the material id from the database
				var materialIdExists = _oracleManager.SelectItemCodeExists(matid);
				if (materialIdExists && 
				    account.Status != StatusEnum.Cold &&
					account.Status != StatusEnum.Unknown) {
					var accountUser = _productmanager.GetAccountUser(account);
					var eFulfillmentModel = new EfulfillmentModel {
						CheckEligibility = false,
						MaterialId = matid,
						ProductCode = "",
						AccountUser = accountUser,
						CheckLogin = false,
					};

					var resultEfulfillment = _efulfillmentActionTypeManager.GetPdfFile(eFulfillmentModel);
                    if (!string.IsNullOrEmpty(resultEfulfillment.PdfSucessUrl))
                    {
                        return Redirect(resultEfulfillment.PdfSucessUrl);
                    }
                    
                    return HandleResponseProductActionType(resultEfulfillment, matid,
						"The Pdf requested couldn't retrieve the materialid");
				}
				return Redirect(pathErrorExternalEfulfillment);
			}
			return Redirect(pathErrorExternalEfulfillment);
		}

        #endregion

        #region Notify Products

        /// <summary>
        /// Insert notification of comming soon in Oracle database
        /// </summary>
        /// <param name="reminderId">Reminder id</param>
        /// <param name="contextItemIdInReminder">Context item id</param>
        /// <param name="eligibilityItemIdInReminder">Eligibility item id</param>
        /// <returns></returns>
        [HttpPost]
		
		public ActionResult NotifyProductAvailableWarm(string reminderId, string contextItemIdInReminder, string eligibilityItemIdInReminder) {
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			var accountUser = _productmanager.GetAccountUser(accountMembership);
			var comingSoonModel = new ComingSoonModel {
				ReminderId = reminderId,
				UrlReturn = Request.UrlReferrer.AbsolutePath,
				AccountUser = accountUser,
                ContextItemId = contextItemIdInReminder,
                EligibilityItemId = eligibilityItemIdInReminder
            };

			var resultCominSoon = _comingSoonManager.ExecuteProcess(comingSoonModel);
			if (resultCominSoon.ResultUrl == ResultUrlEnum.UnForbidden) {
				return Json(new { results = "Error" }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { results = "OK" }, JsonRequestBehavior.AllowGet);
		}


        /// <summary>
        /// Insert notification of comming soon in Oracle database
        /// </summary>
        /// <param name="reminderId">Reminder id</param>
        /// <param name="contextItemIdInReminder">Context item id</param>
        /// <param name="eligibilityItemIdInReminder">Eligibility item id</param>
		/// <returns></returns>
        [HttpPost]
		
		public ActionResult NotifyProductAvailable(string reminderId,string contextItemIdInReminder, string eligibilityItemIdInReminder) {
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			var accountUser = _productmanager.GetAccountUser(accountMembership);
			var comingSoonModel = new ComingSoonModel {
				ReminderId = reminderId,
				UrlReturn = Request.UrlReferrer.AbsolutePath,
				AccountUser = accountUser,
                ContextItemId = contextItemIdInReminder,
                EligibilityItemId = eligibilityItemIdInReminder
			};

			var resultCominSoon = _comingSoonManager.ExecuteProcess(comingSoonModel);
			if (resultCominSoon.ResultUrl == ResultUrlEnum.UnForbidden) {
				return new HttpForbiddenResult();
			} else {
				return Redirect(resultCominSoon.Url);
			}
		}

		#endregion

		#region SSO

		/// <summary>
		/// Execute the Single sign on method action
		/// </summary>
		/// <param name="productCodeMultirow">Product code</param>
		/// <param name="componentTypeSso">Special Offer or multi row</param>
		/// <returns></returns>
		[HttpPost]
		
		public ActionResult ExecuteSingleSignOnMultirow(string productCodeMultirow, int componentTypeSso, string checkOmniSso) {
			return SingleSignOnBase(productCodeMultirow, componentTypeSso,checkOmniSso);
		}

		/// <summary>
		/// Execution of single sing on ot get the url to be redirected
		/// </summary>
		/// <param name="productCode">Product code</param>
		/// <param name="componentType">Flag for special offer</param>
		/// <returns>Url of the partner</returns>
		private ActionResult SingleSignOnBase(string productCode, int componentType, string checkOmniSso) {
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			if (string.IsNullOrEmpty(accountMembership.Mdsid) && Request != null && Request.UrlReferrer != null) {
				return Redirect(Request.UrlReferrer.OriginalString);
			}
            if (checkOmniSso.Equals("1"))
            {
                var resultOmniChannel = GetLinkOmni(accountMembership, productCode);
                if (!string.IsNullOrEmpty(resultOmniChannel.Url))
                {
                    return HandleResponseProductActionType(
                        resultOmniChannel,
                        productCode,
                        "SSO process doesn't return data productcode"
                    );
                }
                else
                {
                    return SingleSignOnInner(productCode, componentType, accountMembership);
                }

            } else {

                return SingleSignOnInner(productCode, componentType, accountMembership);
            }
        }
        private ActionResult SingleSignOnInner(string productCode, int componentType, AccountMembership accountMembership) {
            var accountUser = _productmanager.GetAccountUser(accountMembership);

            var ssoModel = new SsoModel {
                ProductCode = productCode,
                ComponentType = componentType,
                AccountUser = accountUser
            };

            var resultSso = _ssoActionTypeManager.GetUrlSso(ssoModel);
            return HandleResponseProductActionType(
                resultSso,
                productCode,
                "SSO process doesn't return data productcode"
            );
        }

        #endregion

		#region Link

        /// <summary>
        /// Get the url configured in Link type
        /// </summary>
        /// <param name="ctaLinkItemId">Link field id</param>
        /// <param name="contextidLink">Page/component id</param>
        /// <param name="productCodeLink">Product code</param>
        /// <param name="eligibilityItemId">Elegibility Item id</param>
        /// <param name="checkOmniLinkMultirow"></param>
        /// <param name="requestForm"></param>
        /// <returns></returns>
        private ActionResult ExecuteLinkInner(
            string ctaLinkItemId,
            string contextidLink,
            string productCodeLink,
            string eligibilityItemId,
            string checkOmniLinkMultirow,
            NameValueCollection requestForm
        ) {
            string methodName = "ExecuteLinkInner";
            ValidationInputParameters("ctaLinkItemId", methodName, ctaLinkItemId);
            ValidationInputParameters("contextidLink", methodName, contextidLink);
            ValidationInputParameters("productCodeLink", methodName, productCodeLink);
            ValidationInputParameters("eligibilityItemId", methodName, eligibilityItemId);
            ValidationInputParameters("checkOmniLinkMultirow", methodName, checkOmniLinkMultirow);

            if (requestForm==null)
            {
                Sitecore.Diagnostics.Log.Error($"requestForm is null on ExecuteLinkInner", this);
                throw new ArgumentException("requestForm is null on ExecuteLinkInner");
            }            

            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            if (!string.IsNullOrEmpty(accountMembership.Mdsid))
            {
                Sitecore.Diagnostics.Log.Debug($"mdsid value in ExecuteLinkInner:{accountMembership.Mdsid} {accountMembership.Profile.FirstName}");
            }

            if(Request == null || Request.UrlReferrer == null)
            {
                Sitecore.Diagnostics.Log.Debug($"ExecuteLinkInner problem with Request");
            }
            else
            {
                Sitecore.Diagnostics.Log.Debug($"ExecuteLinkInner UrlReferrer {Request.UrlReferrer}");
            }
            if (string.IsNullOrEmpty(accountMembership.Mdsid) && Request != null && Request.UrlReferrer != null) {
				return Redirect(Request.UrlReferrer.OriginalString);
			} else {
                if (checkOmniLinkMultirow.Equals("1"))
                {
                    var resultOmniChannel = GetLinkOmni(accountMembership, productCodeLink);
                    if (!string.IsNullOrEmpty(resultOmniChannel.Url))
                    {
                        return HandleResponseProductActionType(
                            resultOmniChannel,
                            productCodeLink,
                            "Link process doesn't return data productcode"
                        );
                    }
                    else
                    {
                        return GetLinkInner(ctaLinkItemId, contextidLink, productCodeLink, eligibilityItemId, accountMembership, requestForm);
                    }

                } else {

                    return GetLinkInner(ctaLinkItemId, contextidLink, productCodeLink, eligibilityItemId, accountMembership, requestForm);
                }
            }
		}

        private void ValidationInputParameters(string fieldName, string methodName, string valueField)
        {
            if (string.IsNullOrEmpty(valueField))
            {
                Sitecore.Diagnostics.Log.Error($"{fieldName} is empty on {methodName}", this);
                //throw new ArgumentException($"{fieldName} is empty on {methodName}");
            }
            else
            {
                Sitecore.Diagnostics.Log.Debug($"{fieldName} value in ExecuteLinkInner:{valueField}");
            }
        }
        private ActionResult GetLinkInner(string ctaLinkItemId, string contextidLink, string productCodeLink,
            string eligibilityItemId, AccountMembership accountMembership, NameValueCollection requestForm) {

            var accountUser = _productmanager.GetAccountUser(accountMembership);

            var linkModel = new LinkModel {
                ContextItem = contextidLink,
                CtaLinkItemId = ctaLinkItemId,
                EligibilityItemId = eligibilityItemId,
                ProductCodeLink = productCodeLink,
                AccountUser = accountUser
            };
            SetPostData(requestForm, contextidLink,ref linkModel);

            var linkResult = _linkActionTypeManager.GetUrlLink(linkModel);
            return HandleResponseProductActionType(linkResult, productCodeLink, "Link process doesn't return data productcode");
        }
        private void SetPostData(NameValueCollection requestForm, string contextidLink,ref LinkModel linkModel) {

            var contextidLinkFormatted = contextidLink.Replace("}", "").Replace("{", "");
            var result = new Dictionary<string, object>();
            
            foreach (string item in requestForm.Keys)
                if (item.Contains($"pp__{contextidLinkFormatted}"))
                    result.Add(item.Replace($"pp__{contextidLinkFormatted}_", string.Empty), _linkActionTypeManager.ReplaceToken(requestForm[item], linkModel.AccountUser.Mdsid, linkModel.PassthrougData));
            linkModel.PostData= result;
        }

        /// <summary>
		/// Get the url in Link type in multirow
		/// </summary>
		/// <param name="ctaLinkItemIdMultirow">Link field id</param>
		/// <param name="contextidLinkMultirow">Page/component id</param>
		/// <param name="productCodeLinkMultirow">Product code</param>
		/// <returns></returns>
		[HttpPost]
		
		public ActionResult ExecuteLinkMultirow(string ctaLinkItemIdMultirow, string contextidLinkMultirow,
			string productCodeLinkMultirow, string eligibilityItemIdMultirow,string checkOmniLinkMultirow) {
			return ExecuteLinkInner(ctaLinkItemIdMultirow, contextidLinkMultirow, productCodeLinkMultirow,
				eligibilityItemIdMultirow, checkOmniLinkMultirow, Request.Form);
		}

        #endregion

        #region Omni Link

        /// <summary>
        /// Get the url configured in Link type
        /// </summary>
        /// <param name="contextidLinkOmni">Page/component id</param>
        /// <param name="productCodeLinkOmni">Product code</param>
        /// <param name="eligibilityItemIdLinkOmni">Elegibility Item id</param>
        /// <returns></returns>
        private ActionResult ExecuteOmniLinkInner(string contextidLinkOmni, string productCodeLinkOmni,
            string eligibilityItemIdLinkOmni)
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            if (string.IsNullOrEmpty(accountMembership.Mdsid) && Request != null && Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.OriginalString);
            }
            else
            {
                var accountUser = _productmanager.GetAccountUser(accountMembership);

                var linkModel = new OmniLinkModel
                {
                    ContextItem = contextidLinkOmni,
                    EligibilityItemId = eligibilityItemIdLinkOmni,
                    ProductCodeLink = productCodeLinkOmni
                };
                linkModel.AccountUser = accountUser;

                var linkResult = _omniActionTypeManager.GetUrl(linkModel);
                return HandleResponseProductActionType(linkResult, productCodeLinkOmni, "Link process doesn't return data productcode");
            }
        }

        private OperationResult GetLinkOmni(AccountMembership accountMembership, string productCodeLinkOmni
            ) {
            var accountUser = _productmanager.GetAccountUser(accountMembership);

            var linkModel = new OmniLinkModel
            {
                ProductCodeLink = productCodeLinkOmni
            };
            linkModel.AccountUser = accountUser;

            return _omniActionTypeManager.GetUrl(linkModel);
        }
        /// <summary>
        /// Get the url in Link type in omni channel
        /// </summary>
        /// <param name="contextidLinkOmni">Page/component id</param>
        /// <param name="productCodeLinkOmni">Product code</param>
        /// <param name="eligibilityItemIdLinkOmni">Eligibility check</param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult ExecuteLinkOmni( string contextidLinkOmni,
            string productCodeLinkOmni, string eligibilityItemIdLinkOmni)
        {
            return ExecuteOmniLinkInner(contextidLinkOmni, productCodeLinkOmni,
                eligibilityItemIdLinkOmni);
        }

        #endregion

        #region Data pass

        /// <summary>
        /// Get the url configured in Datapass type (Multirow)
        /// </summary>
        /// <param name="productCodeDataPassMultirow">Product code</param>
        /// <param name="componentTypeDataPass">Flag for special offer</param>
        /// <param name="firstSecondAction">First or second button</param>
        /// <returns></returns>
        [HttpPost]
		
		public ActionResult ExecuteDatapassMultirow(string productCodeDataPassMultirow, int componentTypeDataPass,
			string firstSecondAction, string checkOmniDataPass) {
			return ExecuteDatapassInner(productCodeDataPassMultirow, componentTypeDataPass, firstSecondAction,checkOmniDataPass);
		}

        /// <summary>
        /// Get the url configured in Datapass type
        /// </summary>
        /// <param name="productCode">Product code</param>
        /// <param name="dataFirstSecond">String to build the cache key for retrieving the url</param>
        /// <param name="checkOmniDataPass">"1" when is omni channel otherwise false</param>
		/// <param name="componentType">Flag for special offer</param>
        /// <returns></returns>
        private ActionResult ExecuteDatapassInner(string productCode, int componentType, string dataFirstSecond, string checkOmniDataPass) {
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			if (string.IsNullOrEmpty(accountMembership.Mdsid) && Request != null && Request.UrlReferrer != null) {
				return Redirect(Request.UrlReferrer.OriginalString);
			} else {
                if (checkOmniDataPass.Equals("1")) {
                    var resultOmniChannel = GetLinkOmni(accountMembership, productCode);
                    if (!string.IsNullOrEmpty(resultOmniChannel.Url)) {
                        return HandleResponseProductActionType(
                            resultOmniChannel,
                            productCode,
                            "Datapass process doesn't return data productcode"
                        );
                    } else {
                        return ExecuteDatapassDefault(productCode, componentType, dataFirstSecond, accountMembership);
                    }

                } else {
                    return ExecuteDatapassDefault(productCode, componentType, dataFirstSecond, accountMembership);
                }
            }
		}
        private ActionResult ExecuteDatapassDefault(
            string productCode,
            int componentType,
            string dataFirstSecond,
            AccountMembership accountMembership
        ) {
            var accountUser = _productmanager.GetAccountUser(accountMembership);
            var datapassModel = new DatapassModel {
                ComponentType = componentType,
                PrimarySecondaryActionType = dataFirstSecond,
                ProductCode = productCode
            };
            datapassModel.AccountUser = accountUser;

            var datapassResult = _datapassActionTypeManager.GetUrlDatapass(datapassModel);
            return HandleResponseProductActionType(
                datapassResult,
                productCode,
                "Datapass process doesn't return data productcode"
            );
        }

        #endregion

		#region Private methods

		private ActionResult HandleResponseProductActionType(OperationResult operationResult, string productCode,
			string msgError) {
			if (operationResult.ResultUrl == ResultUrlEnum.UnForbidden) {
				if (Request != null && Request.UrlReferrer != null) {
					return Redirect(Request.UrlReferrer.AbsolutePath);
				} else {
					return new HttpForbiddenResult();
				}
			} else if (operationResult.ResultUrl == ResultUrlEnum.NoUrl) {
				Sitecore.Diagnostics.Log.Error(
					string.Format("{0}:{1}", msgError, productCode), this);
                return Redirect(GetErrorPageExternalEfulfillment());
            } else if (operationResult.ResultUrl == ResultUrlEnum.Login) {
				return Redirect(operationResult.Url);
			} else {
				_productmanager.ExecuteMdsLoggingProcessCta(productCode);

                if (operationResult.PostData.Any()) 
                    return this.RedirectAndPost(operationResult.Url, operationResult.PostData);

                return Redirect(operationResult.Url);
			}
		}

		private string GetErrorPageExternalEfulfillment() {
			return LinkManager.GetItemUrl(
				Sitecore.Context.Database.GetItem(_globalConfigurationManager.ItemErrorMscEfulfillment));
		}

		#endregion

		#region Mds Logging

		/// <summary>
		/// Insert into the Oracle database the action when the user is clicked
		/// </summary>
		/// <param name="programCode"></param>
		/// <returns></returns>
		[HttpPost]
		
		public void ExecuteStoredActionCta(string programCode) {
			_productmanager.ExecuteMdsLoggingProcessCta(programCode);
		}

        #endregion

        #region Goal Tracking

        
        [HttpPost]
        public void ExecuteGoalTracking(string productId,string goalId)
        {
            if(string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(goalId))
            {
                return;
            }

            _productmanager.SetGoalsProducts(productId, goalId);
        }

        #endregion
    }
}