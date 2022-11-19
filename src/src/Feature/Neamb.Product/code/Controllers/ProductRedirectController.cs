using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fluentx.Mvc;
using Neambc.Neamb.Feature.Product.Interfaces;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Indexing.Interfaces;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Seiumb.Foundation.Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Controllers
{
	public class ProductRedirectController : BaseController
    {
        private readonly IProductRedirectManager _productRedirectManager;
        

        public ProductRedirectController(IProductRedirectManager productRedirectManager) {
            _productRedirectManager = productRedirectManager;
        }

        public ActionResult ProductRedirect()
		{
            //Process query parameter
            ProductRedirectRequest productRedirectRequest = new ProductRedirectRequest {
                Gclid = Request.QueryString[ConstantsNeamb.Gclid],
                ProductCode = Request.QueryString[ConstantsNeamb.ProductCodeParameter],
                Mdsid = Request.QueryString[ConstantsNeamb.Mdsid],
                UtmSource = Request.QueryString[ConstantsNeamb.UtmSource],
                UtmMedium = Request.QueryString[ConstantsNeamb.UtmMedium],
                UtmTerm = Request.QueryString[ConstantsNeamb.UtmTerm],
                Sob = Request.QueryString[ConstantsNeamb.Sob],
                UtmCampaign = Request.QueryString[ConstantsNeamb.UtmCampaign]
            };

            var productRedirectResponse= _productRedirectManager.ExecuteProductRedirect(productRedirectRequest, RenderingContext.Current.Rendering.Item);
            if (productRedirectResponse!=null && productRedirectResponse.PostData!=null && productRedirectResponse.PostData.Any())
                return this.RedirectAndPost(productRedirectResponse.UrlRedirect, productRedirectResponse.PostData);
            else
            {
                return Redirect(productRedirectResponse.UrlRedirect);
            }
        }
    }
}