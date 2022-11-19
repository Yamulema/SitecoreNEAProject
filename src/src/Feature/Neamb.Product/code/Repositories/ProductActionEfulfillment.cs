using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Sitecore.Data;


namespace Neambc.Neamb.Feature.Product.Repositories
{
	public class ProductActionEfulfillment
	{
		private readonly IOracleDatabase _oracleManager;
		public ProductActionEfulfillment(IOracleDatabase oracleManager) {
			_oracleManager = oracleManager;
		}
		public ProductAction GetActionData(ID componentId,string productCode,string action) {
			ProductAction productAction= new ProductAction();

			var methodDownload = string.Format("downloadpdf{0}", componentId);
			var actionEfullfilment = ActionEfullfilment(methodDownload, productCode, action, componentId);
			productAction.ActionOnClickLink = actionEfullfilment;
			return productAction;
		}
		/// <summary>
		/// Build the path to be executed in efulfillment
		/// </summary>
		/// <param name="functionName">JS function</param>
		/// <param name="productCode">Program code</param>
		/// <param name="action">Product type</param>
		/// <returns></returns>
		private string ActionEfullfilment(string functionName, string productCode, string action, ID componentId)
		{
			var materialId = _oracleManager.SelectItemCodeForProductCode(productCode);
			if (!string.IsNullOrEmpty(materialId))
			{

				var actionEfullfilment =
					$"{functionName}('{materialId}', '{productCode}','{action}{componentId}');return false;";
				return actionEfullfilment;
			}
			return string.Empty;
		}
	}
}