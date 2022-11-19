using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Interfaces;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Feature.Product.Pipelines;
using Neambc.Neamb.Feature.Product.Repositories;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Utilities;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Neamb.Foundation.Product.Pipelines;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Controllers
{
	public class ProductSpecialOfferController : ProductBaseController
	{
		
		public ProductSpecialOfferController(
			IAccountServiceProxy serviceManager, IOracleDatabase oracleManager,
			IPipelineService pipelineService,
			ISessionAuthenticationManager sessionAuthenticationManager, ICacheManager cacheManager, ISessionManager sessionManager, IEligibilityManager eligibilityManager, IProductGtmManager productGtmManager, IGlobalConfigurationManager globalConfigurationManager,
            IComingSoonRepository comingSoonRepository, ICtaActionRepository ctaActionRepository, IProductUtilityManager productUtilityManager)
		{
			base._serviceManager = serviceManager;
			base._oracleManager = oracleManager;
			base._pipelineService = pipelineService;
			base._sessionAuthenticationManager = sessionAuthenticationManager;
			base.pageId = PageContext.Current.Item.ID.ToString();
			base._cacheManager = cacheManager;
			base._sessionManager = sessionManager;
			base._commingSoonItemId = Templates.SpecialOffer.Fields.ComingSoon;
			base._eligibilityItemId = Templates.SpecialOffer.Fields.Eligibility;
			base._productCodeDroplinkItemId = Templates.SpecialOffer.Fields.ProductCodeDroplink;
			base._primaryCtaTypeItemId = Templates.SpecialOffer.Fields.Type;
			base._primaryCtaLinkItemId = Templates.SpecialOffer.Fields.Link;
			base._secondaryCtaTypeItemId = ID.Null;
			base._secondaryCtaLinkItemId = ID.Null;
			base._isSpecialOffer = true;
		    base._eligibilityManager = eligibilityManager;
			base._productGtmManager = productGtmManager;
			base._componentType = ComponentTypeEnum.Cta;
			base._anonymousItemId = Templates.SpecialOffer.Fields.Cta;
			base._reminderCtaId = Templates.SpecialOffer.Fields.ReminderCTA;
			base._globalConfigurationManager = globalConfigurationManager;
            base._comingSoonRepository = comingSoonRepository;
            base._ctaActionRepository = ctaActionRepository;
            base._primaryPostDataItemId = Templates.SpecialOffer.Fields.PostData;
            base._secondaryPostDataItemId = ID.Null;
            base._goalPrimaryItemId = Templates.SpecialOffer.Fields.Goal;
            base._goalSecondaryItemId = ID.Null;
            base._requiresLogin = Templates.OfferLinkItem.Fields.RequiresLogin;
			base._productUtilityManager = productUtilityManager;
		}

		/// <summary>
		/// Get method
		/// </summary>
		/// <returns></returns>
		public ActionResult ProductSpecialOfferItem()
		{
			var model = new SpecialOfferItemDTO();
			var rendering = RenderingContext.Current.Rendering;
			model.Initialize(rendering);
            if (rendering.Item != null && ItemUtility.HasField(rendering.Item, Templates.SpecialOffer.Fields.ProductCodeDroplink)) {
                var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
                ProcessEligibilityActions(model, rendering.Item, accountMembership);
            } else {
                model.HasBrokenLink = true;
            }

            return View("/Views/Neamb.Product/Renderings/SpecialOffer.cshtml", model);
		}
	}
}