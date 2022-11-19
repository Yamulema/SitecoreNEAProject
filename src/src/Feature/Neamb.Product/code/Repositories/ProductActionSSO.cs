using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data;


namespace Neambc.Neamb.Feature.Product.Repositories
{
	public class ProductActionSSO
	{
		public ProductAction GetActionData(ID componentId,string productCode, ComponentTypeEnum componentType) {
			ProductAction productAction= new ProductAction();

			var methodSingleSignOn = string.Format("executesinglesignon{0}", componentId);
			productAction.ActionOnClickLink =
				$"{methodSingleSignOn}('{productCode}','{componentType}'); operationprocedureactioncta('{productCode}');return false;";
			return productAction;
		}
	}
}