using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data;


namespace Neambc.Neamb.Feature.Product.Repositories
{
	public class ProductActionDatapass
	{
		public ProductAction GetActionData(ID componentId,string productCode, ComponentTypeEnum componentType) {
			ProductAction productAction= new ProductAction();

			var methodDatapass = string.Format("executedatapass{0}", componentId);

			productAction.ActionOnClickLink =
				$"{methodDatapass}('{productCode}', '{componentType}');operationprocedureactioncta('{productCode}');return false;";
			return productAction;
		}
	}
}