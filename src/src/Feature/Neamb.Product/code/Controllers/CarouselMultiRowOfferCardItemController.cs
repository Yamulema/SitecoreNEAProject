using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Model;
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
	public class CarouselMultiRowOfferCardItemController : ProductBaseController
	{
		
		public CarouselMultiRowOfferCardItemController(
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
			base._commingSoonItemId = ID.Null;
			base._eligibilityItemId = Templates.ProductOfferCard.Fields.Eligibility;
			base._productCodeDroplinkItemId = Templates.ProductOfferCard.Fields.ProductCodeDroplink;
			base._primaryCtaTypeItemId = Templates.ProductOfferCard.Fields.Type;
			base._primaryCtaLinkItemId = Templates.ProductOfferCard.Fields.Link;
			base._secondaryCtaTypeItemId = ID.Null;
			base._secondaryCtaLinkItemId = ID.Null;
		    base._eligibilityManager = eligibilityManager;
			base._productGtmManager = productGtmManager;
			base._componentType = ComponentTypeEnum.MultiRow;
			base._anonymousItemId = Templates.ProductOfferCard.Fields.Cta;
			base._reminderCtaId = ID.Null;
			base._globalConfigurationManager = globalConfigurationManager;
            base._comingSoonRepository = comingSoonRepository;
            base._ctaActionRepository = ctaActionRepository;
            base._primaryPostDataItemId = Templates.ProductOfferCard.Fields.PostData;
            base._secondaryPostDataItemId = ID.Null;
            base._goalPrimaryItemId = Templates.ProductOfferCard.Fields.Goal;
            base._goalSecondaryItemId = ID.Null;
            base._requiresLogin = Templates.OfferLinkItem.Fields.RequiresLogin;
            base._productUtilityManager = productUtilityManager;
        }

		/// <summary>
		/// Get carousel items
		/// </summary>
		/// <returns></returns>
		public ActionResult GetMultiRowOfferCardItems()
		{
            var model = GetModelForMultiRowOfferCard();
            return View("/Views/Neamb.Product/Renderings/CarouselMultiRowOfferCardItem.cshtml", model);
		}

        /// <summary>
		/// Get carousel items
		/// </summary>
		/// <returns></returns>
		public ActionResult GetMultiRowOfferCardItemsVersion2()
        {
            var model = GetModelForMultiRowOfferCard();
            model.CardClass = "half-card";
            return View("/Views/Neamb.Product/Renderings/CarouselMultiRowOfferCardItemVersion2.cshtml", model);
        }

        private CarouselMultiRowOfferDTO GetModelForMultiRowOfferCard()
        {
            var model = new CarouselMultiRowOfferDTO();
            var rendering = RenderingContext.Current.Rendering;
            model.Initialize(rendering);
            var cards = ((Sitecore.Data.Fields.MultilistField)model.Item.Fields[Templates.CarouselOfferCards.Fields.Cards]).GetItems();
            foreach (var cardItem in cards)
            {
                var multiRowOfferItemDTO = new MultiRowOfferItemDto();
                multiRowOfferItemDTO.Item = cardItem;

                if (cardItem != null && ItemUtility.HasField(cardItem, Templates.ProductOfferCard.Fields.ProductCodeDroplink))
                {
                    var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
                    ProcessEligibilityActions(multiRowOfferItemDTO, cardItem, accountMembership);
                }
                else
                {
                    multiRowOfferItemDTO.HasBrokenLink = true;
                }
                var image = cardItem[Templates.CarouselOfferItem.Fields.Image];
                multiRowOfferItemDTO.HasImage = !String.IsNullOrEmpty(image);
                var buttonColor = cardItem[Templates.CarouselOfferItem.Fields.ButtonColor];
                switch (buttonColor)
                {
                    case "Blue":
                        {
                            multiRowOfferItemDTO.ButtonClass = "btn-blue";
                            break;
                        }
                    case "Orange":
                        {
                            multiRowOfferItemDTO.ButtonClass = "btn-orange";
                            break;
                        }
                    case "Green":
                        {
                            multiRowOfferItemDTO.ButtonClass = "btn-green";
                            break;
                        }
                }
                model.ItemsCarousel.Add(multiRowOfferItemDTO);
            }
            var firstItemImage = model.ItemsCarousel.FirstOrDefault(item => item.HasImage);
            model.HasImage = firstItemImage != null;
            return model;
        }
	}
}