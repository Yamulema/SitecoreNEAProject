using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Feature.Product.Repositories;
using Neambc.Neamb.Foundation.Analytics.Gtm;
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
using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using Neambc.Neamb.Feature.GeneralContent.Models;

namespace Neambc.Neamb.Feature.Product.Controllers
{
	public class ProductController : ProductBaseController
	{
		private readonly IProductManager _productManager;
        private readonly IGtmService _gtmService;  // copied from ProductAnchoredHeaderController
		
		public ProductController(
			IAccountServiceProxy serviceManager, IOracleDatabase oracleManager, IPipelineService pipelineService,
			ISessionAuthenticationManager sessionAuthenticationManager, ICacheManager cacheManager,
			ISessionManager sessionManager, IEligibilityManager eligibilityManager, IProductManager productManager, 
            IProductGtmManager productGtmManager, IGtmService gtmService, IGlobalConfigurationManager globalConfigurationManager,
			IComingSoonRepository comingSoonRepository, ICtaActionRepository ctaActionRepository, IProductUtilityManager productUtilityManager)
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
            base._primaryCtaColorButton = Templates.ProductCTAs.Fields.PrimaryCTAButtonColor;
            base._primaryPostDataItemId = Templates.ProductCTAs.Fields.PrimaryPostData;
			base._secondaryCtaTypeItemId = Templates.ProductCTAs.Fields.SecondaryCTAType;
			base._secondaryCtaLinkItemId = Templates.ProductCTAs.Fields.SecondaryCTALink;
            base._secondaryCtaColorButton = Templates.ProductCTAs.Fields.SecondaryCTButtonColor;
            base._anonymousCtaColorButton = Templates.ProductCTAs.Fields.AnonymousCTAButtonColor;
            base._secondaryPostDataItemId = Templates.ProductCTAs.Fields.SecondaryPostData;
		    base._eligibilityManager = eligibilityManager;
			_productManager = productManager;
            _gtmService = gtmService;
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

		public ActionResult ProductDetail()
		{
			var renderingItem = RenderingContext.Current.Rendering.Item;
            var model = new ProductDetailDTO();

            if (renderingItem != null && ItemUtility.HasField(renderingItem, Templates.ProductCTAs.Fields.ProductCodeDroplink)) {

                var productCode = _productUtilityManager.GetProductCode(renderingItem, Templates.ProductCTAs.Fields.ProductCodeDroplink);
                _productManager.ExecuteMdsLoggingProcessView(productCode);
                model.IsProductPage = (RenderingContext.Current.PageContext.Item.TemplateID == Templates.ProductPage.ID);
                var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
                model.Initialize(RenderingContext.Current.Rendering);
                //Implement Eyebrown
                LinkField eyebrowLink = renderingItem.Fields[Templates.ProductCTAs.Fields.Eyebrow];
                if (eyebrowLink != null && eyebrowLink.TargetItem != null) {
                    var category = eyebrowLink.TargetItem;
                    //Get the title
                    model.EyebrownName = !string.IsNullOrEmpty(eyebrowLink.Text) ? eyebrowLink.Text : category[Templates.PageInfo.Fields.PageTitle];
                    //Get the link
                    model.EyebrownLink = LinkManager.GetItemUrl(eyebrowLink.TargetItem);
                    if (!string.IsNullOrEmpty(eyebrowLink.Target)) {
                        model.EyebrownTarget = "_blank";
                    }
                }

                //Get the partner
                var partners = ((MultilistField) renderingItem.Fields[
                    Templates.ProductCTAs.Fields.PartnerAttribution]).GetItems();
                foreach (var itemPartner in partners) {
                    model.ListPartnersItems.Add(itemPartner);
                }

                ProcessEligibilityActions(model, renderingItem, accountMembership);
                SetButtonColorClass(model, renderingItem);
                model.HasClassEligibility =
                    model.HasCheckEligibility &&
                    !model.ResultCheckEligibility &&
                    model.UserStatus != StatusEnum.Cold &&
                    model.UserStatus != StatusEnum.Unknown;

                //Set GTM action for page load action
                var productCustomDimension = _productManager.GetProductDimensions(RenderingContext.Current.Rendering.Item);
                //product category, subcategory, subgroup gtm action only in product page
                if (renderingItem.TemplateID != Templates.ProductCtaLite.ID && RenderingContext.Current.PageContext.Item.TemplateID == Templates.ProductPage.ID) {
                    model.GtmActionPage = _gtmService.GetGtmEvent(productCustomDimension);
                }

                //Product Contact Details
                model.ProductContactDetails = model.Item[Templates.ProductCTAs.Fields.ProductContactDetails];
            } else {
                model.HasBrokenLink = true;
            }
            return View("/Views/Neamb.Product/Renderings/ProductDetail.cshtml", model);
		}
	}
}