using System;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Utilities;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Neamb.Feature.GeneralContent.Models;

namespace Neambc.Neamb.Feature.Product.Controllers {
	public class GuideCtaController : BaseController {
		private ISessionAuthenticationManager _sessionAuthenticationManager;
		private readonly ISessionManager _sessionManager;
		private string _fileName;
		protected IProductGtmManager _productGtmManager;

		protected ComponentTypeEnum _componentType = ComponentTypeEnum.GuideCta;

		public GuideCtaController(
			ISessionAuthenticationManager sessionAuthenticationManager,
			ISessionManager sessionManager,
			IProductGtmManager productGtmManager
		) {
			_sessionAuthenticationManager = sessionAuthenticationManager;
			_sessionManager = sessionManager;
			_productGtmManager = productGtmManager;
		}

        public ActionResult CtaGuide()
        {
            var model = new GuideCtaDTO();
            model.Initialize(RenderingContext.Current.Rendering);
            var renderingItem = RenderingContext.Current.Rendering.Item;
            if (renderingItem != null && ItemUtility.HasField(renderingItem, Templates.GuideCta.Fields.MaterialId)) {

                //Set the component id according the rendering item id
                model.ComponentId = renderingItem.ID.ToString().Replace("{", "").Replace("}", "").Replace("-", "");
                //Get the information of the user logged in the system
                var accountMembership = _sessionAuthenticationManager.GetAccountMembership();

                model.UserStatus = accountMembership.Status;
                model.SocialShare = new SocialShareModel(renderingItem);

                _fileName = !string.IsNullOrEmpty(renderingItem[Templates.GuideCta.Fields.MaterialId])
                    ? renderingItem[Templates.GuideCta.Fields.MaterialId]
                    : renderingItem.Name;
                //Get the value of the cta general link
                LinkField primaryLink = renderingItem.Fields[Templates.GuideCta.Fields.Cta];

                if (primaryLink != null) {
                    //Get the text set on cta general link
                    model.ActionPrimaryDescription = primaryLink.Text;
                    //Get the target set on cta general link
                    if (!string.IsNullOrEmpty(primaryLink.Target)) {
                        model.ActionPrimaryTargetBlank = "_blank";
                    } else {
                        model.ActionPrimaryTargetBlank = "";
                    }

                    //Verify if material id has been set
                    if (string.IsNullOrEmpty(renderingItem[Templates.GuideCta.Fields.MaterialId])) {
                        //If it is not set the material id get the information of the general link to get the url
                        if (primaryLink.TargetItem != null && primaryLink.TargetID != Templates.MediaLibrary.ID) {
                            //Get the media link url

                            if (primaryLink.IsMediaLink) {

                                model.ActionPrimary = MediaManager.GetMediaUrl(primaryLink.TargetItem);

                            }
                            //Get the internal link url
                            else {
                                model.ActionPrimary = Sitecore.Links.LinkManager.GetItemUrl(primaryLink.TargetItem);

                            }
                            //add gtm
                            var actionclick = model.ActionClickPrimary;
                            string clickGtmAction = _productGtmManager.GetGtmFunction(_componentType, renderingItem, model.ActionPrimary);

                            if (!string.IsNullOrEmpty(clickGtmAction)) {
                                model.ActionClickPrimary = $"{clickGtmAction}{actionclick}";
                            }
                        } else {

                            model.DisplayAction = false;
                        }
                    } else {
                        //Build the js action to be called in click action
                        var actionclick = string.Format("downloadpdfguide{0}('{1}')",
                            model.ComponentId,
                            renderingItem[Templates.GuideCta.Fields.MaterialId]);
                        //add gtm

                        string clickGtmAction = _productGtmManager.GetGtmFunction(_componentType, renderingItem, "/api/ProductRoute/DownloadEfulfillmentPdfMultirow");

                        if (!string.IsNullOrEmpty(clickGtmAction)) {
                            actionclick = $"{clickGtmAction}{actionclick}";
                        }

                        if (accountMembership.Status == StatusEnum.Hot) {
                            model.ActionClickPrimary = actionclick;
                        } else if (accountMembership.Status == StatusEnum.WarmCold ||
                            accountMembership.Status == StatusEnum.WarmHot ||
                            accountMembership.Status == StatusEnum.Cold ||
                            accountMembership.Status == StatusEnum.Unknown) {

                            //Case warm cold or warm hot save in session variables the actions to be executed
                            _sessionManager.StoreInSession<string>(
                                String.Format("{0}{1}", ConstantsNeamb.CtaActionPrimary, model.ComponentId),
                                "");
                            _sessionManager.StoreInSession<string>(
                                String.Format("{0}{1}", ConstantsNeamb.CtaActionPrimaryOnclick, model.ComponentId),
                                actionclick);
                            _sessionManager.StoreInSession<string>(
                                String.Format("{0}{1}", ConstantsNeamb.CtaActionPrimaryTargetBlank, model.ComponentId),
                                model.ActionPrimaryTargetBlank);

                            model.ActionClickPrimary = $"executeloginwarm('{ConstantsNeamb.Primary}{model.ComponentId}')";
                            if (accountMembership.Status == StatusEnum.Cold || accountMembership.Status == StatusEnum.Unknown) {
                                _sessionManager.StoreInSession<string>(ConstantsNeamb.CtaActionWarmUser, ConstantsNeamb.UserCold);
                                LinkField anonymousLink = renderingItem.Fields[Templates.GuideCta.Fields.CtaLogin];
                                model.AnonymousText = anonymousLink.Text;
                            } else if (!string.IsNullOrEmpty(accountMembership.Username)) {
                                    _sessionManager.StoreInSession<string>(ConstantsNeamb.CtaActionWarmUser, accountMembership.Username);
                            }
                        }
                    }
                }
            } else {
                model.HasBrokenLink = true;
            }
            return View("/Views/Neamb.Product/Renderings/GuideCta.cshtml", model);
        }
    }
}