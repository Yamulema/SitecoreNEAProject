using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Product.Controllers
{
	public class OraclePageViewController : BaseController
	{
		private readonly IProductManager _productManager;
		private readonly IProductUtilityManager _productUtilityManager;

		public OraclePageViewController(IProductManager productManager,
			IProductUtilityManager productUtilityManager)
		{
			_productManager=productManager;
			_productUtilityManager = productUtilityManager;
        }

		/// <summary>
		/// Get method
		/// </summary>
		/// <returns></returns>
		public ActionResult PageView()
		{
			var renderingItem = RenderingContext.Current.Rendering.Item;

			var productCode = _productUtilityManager.GetProductCodeFromGlobal(renderingItem, Templates.ProductCode.Fields.ProductCode);
			_productManager.ExecuteMdsLoggingProcessView(productCode);
			return View("/Views/Neamb.Product/Renderings/OraclePageView.cshtml");
		}		
	}
}