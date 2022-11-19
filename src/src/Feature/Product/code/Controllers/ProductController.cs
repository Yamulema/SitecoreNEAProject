using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using System.Collections.Generic;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Product.Repositories;
using Neambc.Seiumb.Foundation.Analytics.GTM.Models;
using Neambc.Seiumb.Foundation.Analytics.GTM;

namespace Neambc.Seiumb.Feature.Product.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly IGTMServiceSeiumb _gtmService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productRepository"></param>
        public ProductController(IProductRepository productRepository, IGTMServiceSeiumb gtmService)
        {
            _productRepository = productRepository;
            _gtmService = gtmService;
        }

        public ActionResult GetProductLite()
        {
            var renderingItem = RenderingContext.Current.Rendering.Item;
            var result = _productRepository.GetUserStateProductLiteData(renderingItem);
            return View("/Views/Product/Renderings/_ProductLite.cshtml", result);
        }

        /// <summary>
        /// Get the list of the product according items selected in the multilist with search
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProductList()
        {
            var result = _productRepository.GetProductListData(RenderingContext.Current.ContextItem);
            result.OnClickEventContent = GetOnClickGTMContent(result.Items);
            return View("/Views/Product/Renderings/RightColumnProductList.cshtml", result);
        }

        private List<string> GetOnClickGTMContent(List<Sitecore.Data.Items.Item> productItems)
        {
            var result = new List<string>();

            foreach (var item in productItems)
            {
                var dataLayerData = _gtmService.GetOnClickEvent(new ModuleSeiumb
                {
                    Event = "small module",
                    ModuleTitle = item["Headline"] ?? string.Empty,
                    CtaText = item["MoreLink"] ?? string.Empty,
                });
                result.Add(dataLayerData);
            }
            return result;
        }
    }
}