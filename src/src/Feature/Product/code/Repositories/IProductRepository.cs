using Neambc.Seiumb.Feature.Product.Models;
using Sitecore.Data.Items;

namespace Neambc.Seiumb.Feature.Product.Repositories
{
    /// <summary>
    /// Actions in product detail
    /// </summary>
    public interface IProductRepository
    {
        UserStateProduct GetUserStateProductLiteData(Item renderingItem);
        ProductList GetProductListData(Item contextItem);

        string GetPdfFile(string materialId);

	    void ExecuteMdsLoggingProcessCta(string productcode, string mdsid);
	    void ExecuteMdsLoggingProcessView(string programCode, string mdsid);
	    void ExecuteMdsLoggingProcessMaterial(string materialid, string mdsid);
    }
}