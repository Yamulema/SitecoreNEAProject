using Neambc.Neamb.Feature.Product.Model;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Product.Interfaces
{
    public interface IProductRedirectManager
    {
        ProductRedirectResponse ExecuteProductRedirect(ProductRedirectRequest productRedirectRequest, Item renderingItem);
    }
}