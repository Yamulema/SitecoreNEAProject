using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    public interface IProductUtilityManager
    {
        string GetProductCode(Item renderingItem, Sitecore.Data.ID productCodeDropLinkId);
        string GetProductCodeFromGlobal(Item renderingItem, Sitecore.Data.ID productCodeDropLinkId);
    }
}
