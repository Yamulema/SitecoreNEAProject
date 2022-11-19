using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Feature.Product.Repositories;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Utilities;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Neamb.Foundation.Product.Pipelines;
using Sitecore.Data;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Controllers
{
	public class PromotionalToutsController : ProductBaseController
	{
		private readonly IEligibilityOmni _eligibilityOmni;

        public PromotionalToutsController(
			IAccountServiceProxy serviceManager, IOracleDatabase oracleManager, IPipelineService pipelineService,
			ISessionAuthenticationManager sessionAuthenticationManager, ICacheManager cacheManager,
			ISessionManager sessionManager, IEligibilityManager eligibilityManager, 
            IProductGtmManager productGtmManager, IGlobalConfigurationManager globalConfigurationManager,
			IComingSoonRepository comingSoonRepository, ICtaActionRepository ctaActionRepository, 
            IEligibilityOmni eligibilityOmni,
            IProductUtilityManager productUtilityManager)
		{
			base._serviceManager = serviceManager;
			base._oracleManager = oracleManager;
			base._pipelineService = pipelineService;
			base._sessionAuthenticationManager = sessionAuthenticationManager;
			base._sessionManager = sessionManager;
			base.pageId = PageContext.Current.Item.ID.ToString();
			base._cacheManager = cacheManager;
			base._commingSoonItemId = Templates.ProductCTAs.Fields.ComingSoon;
			base._eligibilityItemId = Templates.ProductCTAs.Fields.Eligibility;
			base._productCodeDroplinkItemId = Templates.ProductCTAs.Fields.ProductCodeDroplink;
			base._primaryCtaTypeItemId = Templates.ProductCTAs.Fields.PrimaryCTAType;
			base._primaryCtaLinkItemId = Templates.ProductCTAs.Fields.PrimaryCTALink;
            base._primaryPostDataItemId = Templates.ProductCTAs.Fields.PrimaryPostData;
            base._secondaryCtaTypeItemId = Templates.ProductCTAs.Fields.SecondaryCTAType;
			base._secondaryCtaLinkItemId = Templates.ProductCTAs.Fields.SecondaryCTALink;
            base._secondaryPostDataItemId = Templates.ProductCTAs.Fields.SecondaryPostData;
            base._eligibilityManager = eligibilityManager;
            _eligibilityOmni = eligibilityOmni;
            base._productGtmManager = productGtmManager;
			base._componentType = ComponentTypeEnum.Cta;
			base._anonymousItemId = Templates.ProductCTAs.Fields.AnonymousCTA;
			base._reminderCtaId = Templates.ProductCTAs.Fields.ReminderCTA;
			base._globalConfigurationManager = globalConfigurationManager;
			base._comingSoonRepository = comingSoonRepository;
			base._ctaActionRepository = ctaActionRepository;
            base._goalPrimaryItemId = Templates.ProductCTAs.Fields.GoalTriggerPrimary;
            base._goalSecondaryItemId = Templates.ProductCTAs.Fields.GoalTriggerSecondary;
            base._requiresLogin = Templates.OfferLinkItem.Fields.RequiresLogin;
            base._productUtilityManager = productUtilityManager;
        }

		public ActionResult PromotionalToutDetail()
		{
			var renderingItem = RenderingContext.Current.Rendering.Item;
            var model = new ProductDetailDTO();

            if (renderingItem != null && ItemUtility.HasField(renderingItem, Templates.ProductCTAs.Fields.ProductCodeDroplink)) {

                var productCode = _productUtilityManager.GetProductCode(renderingItem, Templates.ProductCTAs.Fields.ProductCodeDroplink);
                model.IsProductPage = (RenderingContext.Current.PageContext.Item.TemplateID == Templates.ProductPage.ID);
                var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
                model.Initialize(RenderingContext.Current.Rendering);
                
                //Get the partner
                var partners = ((Sitecore.Data.Fields.MultilistField) renderingItem.Fields[
                    Templates.ProductCTAs.Fields.PartnerAttribution]).GetItems();
                foreach (var itemPartner in partners) {
                    model.ListPartnersItems.Add(itemPartner);
                }

                ProcessEligibilityActions(model, renderingItem, accountMembership);
                model.HasClassEligibility =
                    model.HasCheckEligibility &&
                    !model.ResultCheckEligibility &&
                    model.UserStatus != StatusEnum.Cold &&
                    model.UserStatus != StatusEnum.Unknown;

                if (model.UserStatus == StatusEnum.Hot ||
                    model.UserStatus == StatusEnum.WarmHot ||
                    model.UserStatus == StatusEnum.WarmCold
                ) {
                   var resultElibility= _eligibilityOmni.CheckEligibility(accountMembership.Mdsid, productCode);
                   model.IsEligibleOmni = resultElibility != null;
                } else {
                    model.IsEligibleOmni = false;
                }
                model.IsOmni = renderingItem.Fields[Templates.ProductCTAs.Fields.IsOmni].IsChecked();
            } else {
                model.HasBrokenLink = true;
            }
            return View("/Views/Neamb.Product/Renderings/PromotionalTouts.cshtml", model);
		}
	}
}