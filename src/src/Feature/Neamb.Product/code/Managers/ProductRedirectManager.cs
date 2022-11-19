using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Feature.Product.Interfaces;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Seiumb.Foundation.Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Managers
{
    [Service(typeof(IProductRedirectManager))]
    public class ProductRedirectManager: IProductRedirectManager
    {

        private readonly ILinkActionTypeManager _linkActionTypeManager;
        private readonly IProductSearchManager _productSearchManager;
        private readonly ILog _log;
        private readonly IProductManager _productmanager;
        
        public ProductRedirectManager(ILinkActionTypeManager linkActionTypeManager, IProductSearchManager productSearchManager, ILog log, IProductManager productmanager)
        {
            _linkActionTypeManager = linkActionTypeManager;
            _productSearchManager = productSearchManager;
            _log = log;
            _productmanager = productmanager;
            
        }

        /// <summary>
        /// Get the first product page based on the product code
        /// </summary>
        /// <param name="productRedirectRequest">Data with query parameter strings</param>
        /// <returns>Url to redirect and post parameters</returns>
        public ProductRedirectResponse ExecuteProductRedirect(ProductRedirectRequest productRedirectRequest, Item renderingItem) {
            ProductRedirectResponse productRedirectResponse = new ProductRedirectResponse();
            if (productRedirectRequest == null || renderingItem == null) {
                productRedirectResponse.UrlRedirect = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.HomePage.ID));
            } else if (!string.IsNullOrEmpty(productRedirectRequest.ProductCode) && !string.IsNullOrEmpty(productRedirectRequest.Mdsid)) {
                //Get the product item with the specific product code
                var firstProductPage = _productSearchManager.GetContentPages(productRedirectRequest.ProductCode);
                if (firstProductPage != null) {
                    var accountUserData = new AccountUserBase {
                        Mdsid = productRedirectRequest.Mdsid
                    };

                    //Get the first product page
                    _log.Debug($"Product found in redirect {firstProductPage.ItemId} {productRedirectRequest.ProductCode}");
                    Item productItem = Sitecore.Context.Database.GetItem(firstProductPage.ItemId);
                    if (HasLinkType(productItem)) {

                        //Build the model object with the product item id, cta link field id, product code, post parameters, query parameter values
                        var linkModel = new LinkModel {
                            AccountUser = accountUserData,
                            ContextItem = productItem.ID.ToString(),
                            CtaLinkItemId = Templates.ProductCTAs.Fields.PrimaryCTALink.ToString(),
                            EligibilityItemId = ID.Null.ToString(),
                            ProductCodeLink = productRedirectRequest.ProductCode,
                            PassthrougData = new PassthroughModel {
                                UtmCampaign = productRedirectRequest.UtmCampaign,
                                UtmTerm = productRedirectRequest.UtmTerm,
                                UtmSource = productRedirectRequest.UtmSource,
                                Sob = productRedirectRequest.Sob,
                                Gclid = productRedirectRequest.Gclid,
                                UtmMedium = productRedirectRequest.UtmMedium
                            }
                        };

                        GetPostParams(productItem, ref linkModel);

                        //Get the url link
                        var operationResult = _linkActionTypeManager.GetUrlLink(linkModel);
                        if (operationResult != null) {

                            if (operationResult.PostData.Any()) {
                                //Set post data 
                                productRedirectResponse.PostData = operationResult.PostData;
                            } else {
                                //Tracking in MDS
                                _productmanager.ExecuteMdsLoggingProcessCta(productRedirectRequest.ProductCode);
                            }
                            //Set in the response the url
                            productRedirectResponse.UrlRedirect = operationResult.Url;
                        }
                    } else {
                        _log.Debug($"Product no link {firstProductPage.ItemId} {firstProductPage.ProductCodeSm}");
                    }
                } else {
                    _log.Debug($"Product NO found in redirect {productRedirectRequest.ProductCode}");
                }

            }
            if (string.IsNullOrEmpty(productRedirectResponse.UrlRedirect)) {
                productRedirectResponse.UrlRedirect = GetRedirectDefaultUrl(renderingItem);
            }
            return productRedirectResponse;
        }

        private bool HasLinkType(Item productItem) {
            var actionType = productItem[Templates.ProductCTAs.Fields.PrimaryCTAType];
            if (actionType != null && !string.IsNullOrEmpty(actionType))
            {
                var actionTypeItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(actionType));
                var typeAction = actionTypeItem[Templates.CategoryItem.Fields.Value];
                if (typeAction == ActionTypeEnum.Link.ToString()) {
                    return true;
                } 
            }
            return false;
        }
        /// <summary>
        /// Get the url for the error page or the home
        /// </summary>
        /// <returns></returns>
        private string GetRedirectDefaultUrl(Item renderingItem) {
            
            LinkField errorPageLink = renderingItem.Fields[Templates.ProductRedirect.Fields.ErrorPage];
            if (errorPageLink != null && errorPageLink.TargetItem != null)
            {
                var errorPage = errorPageLink.TargetItem;
                var errorPageUrl = LinkManager.GetItemUrl(errorPage);
                return errorPageUrl;
            }
            else
            {
                return LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.HomePage.ID));
            }
        }

        /// <summary>
        /// Get the post parameters from the primary post data field
        /// </summary>
        /// <param name="productItem">Product item</param>
        /// <param name="accountUser">Account user data</param>
        /// <returns></returns>
        private void GetPostParams(Item productItem,ref LinkModel linkModel)
        {
            var parameters = new Dictionary<string, object>();

            try
            {
                _log.Debug($"GetPostParams Product Redirect PostDataItemId: {productItem.ID}", this);

                var rawText = productItem[Templates.ProductCTAs.Fields.PrimaryPostData] ?? string.Empty;
                var text = rawText.Replace("{", String.Empty).Replace("}", String.Empty);

                foreach (var entry in text.Split(new[] { "\r\n" }, StringSplitOptions.None))
                {
                    var parameter = entry.Split(new[] { ":" }, StringSplitOptions.None);
                    if (parameter.Length == 2)
                        parameters.Add(parameter.First(), _linkActionTypeManager.ReplaceToken(parameter.Last(), linkModel?.AccountUser?.Mdsid, linkModel?.PassthrougData));
                }
            }
            catch (Exception e)
            {
                _log.Error("Error in GetPostParams: ", e, this);
            }

            linkModel.PostData = parameters;
        }
    }
}