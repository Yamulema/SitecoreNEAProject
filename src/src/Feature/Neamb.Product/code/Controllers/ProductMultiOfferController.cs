using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Membership.Managers;
using Sitecore.Mvc.Presentation;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Sitecore.Data;
using Sitecore;
using System;
using Neambc.Seiumb.Foundation.Sitecore;
using Sitecore.Links;

namespace Neambc.Neamb.Feature.Product.Controllers
{
	public class ProductMultiOfferController : BaseController
	{
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly IProductManager _productmanager;
		private readonly IMultiOfferManager _multiOfferManager;
		private readonly ILog _log;
		public ProductMultiOfferController(
			ISessionAuthenticationManager sessionAuthenticationManager, 
			IProductManager productManager, 
			IMultiOfferManager multiOfferManager,
			ILog log)
		{
			_sessionAuthenticationManager = sessionAuthenticationManager;
			_productmanager = productManager;
			_multiOfferManager = multiOfferManager;
			_log = log;
		}

		/// <summary>
		/// Get method ProductMultiOffer component
		/// </summary>
		/// <returns></returns>
		public ActionResult ProductMultiOffer()
		{
			var requestPage = Request.Url.OriginalString;
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			if (Request != null && Request.UrlReferrer != null 
				//&& hostNames.Contains(Request.UrlReferrer.Host)
				)
			{
				_multiOfferManager.SaveRedirectPreviousPage(Request.UrlReferrer.AbsolutePath);								
			}

			if (accountMembership.Status != Foundation.Membership.Model.StatusEnum.Hot)
			{
				var urlRedirect = _multiOfferManager.HandleRedirectUrlForLoginNotAuthenticated(requestPage, Request.Url.AbsolutePath);
				return Redirect(urlRedirect);
			}
			else
			{
                var model = new ProductMultiOfferDTO();
                var rendering = RenderingContext.Current.Rendering;
                model.Initialize(rendering);
				model.RadioOptionGroups = _multiOfferManager.GetRadioButtonForm(model.Item, Request.QueryString);
				model.PostParams =_multiOfferManager.GetPostParamsValues(model.Item, Templates.ProductMultiOffer.Fields.PostParamToken);
				model.CancelUrl = _multiOfferManager.GetRedirectPreviousPage();
				return View("/Views/Neamb.Product/Renderings/ProductMultiOffer.cshtml", model);
            }
		}


		[HttpPost]
		public ActionResult ProductMultiOfferAction(string dataform, string contextitemid)
		{
			try
			{
				var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
				if (accountMembership.Status != Foundation.Membership.Model.StatusEnum.Hot)
				{
					var pathLoginPage = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID));
					return Json(new { result = "errorlogin", urlredirect = pathLoginPage }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					var contextItem = Context.Database.GetItem(new ID(contextitemid));
					var resultProductId = _multiOfferManager.GetProductId(contextItem, dataform);
					var urlReturn = _multiOfferManager.GetRedirectPreviousPage();

					if (!string.IsNullOrEmpty(resultProductId))
					{
						_productmanager.ExecuteMdsLoggingProcessCta(resultProductId);
						return Json(new { result = "ok", product = resultProductId, urlreturn = urlReturn }, JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
					}
				}
			}
			catch(Exception ex)
            {
				_log.Error("Error in ProductMultiOfferAction", ex);
				return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
			}
		}
    }
}