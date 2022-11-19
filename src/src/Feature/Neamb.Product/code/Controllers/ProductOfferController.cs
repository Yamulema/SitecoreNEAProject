using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Membership.Managers;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Controllers
{
	public class ProductOfferController : BaseController
	{
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;

		public ProductOfferController(
			ISessionAuthenticationManager sessionAuthenticationManager)
		{
			_sessionAuthenticationManager = sessionAuthenticationManager;
		}

		/// <summary>
		/// Get method
		/// </summary>
		/// <returns></returns>
		public ActionResult ProductOffer()
		{
			var model = new ProductOfferLinksDTO();
			var rendering = RenderingContext.Current.Rendering;
			model.Initialize(rendering);
			var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
			model.UserStatus = accountMembership.Status;
			return View("/Views/Neamb.Product/Renderings/ProductOfferLinks.cshtml", model);
		}
	}
}