using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Utilities;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    [Service(typeof(IProductUtilityManager))]
    public class ProductUtilityManager : IProductUtilityManager
    {
        public string GetProductCode(Item renderingItem, Sitecore.Data.ID productCodeDropLinkId )
        {
            if (renderingItem != null && ItemUtility.HasField(renderingItem, productCodeDropLinkId))
            {
                string dropDownItemId = renderingItem[productCodeDropLinkId];
                var globalItem = Sitecore.Context.Database.GetItem(dropDownItemId);
                if(globalItem!= null && globalItem.ID!= Sitecore.Data.ID.Null && !Sitecore.Data.ID.IsNullOrEmpty(globalItem.ID))
                return globalItem[Templates.ProductCode.Fields.ProductCode];
            }
            return string.Empty;
        }
        public string GetProductCodeFromGlobal(Item renderingItem, Sitecore.Data.ID productCodeDropLinkId)
        {
            if (renderingItem != null && ItemUtility.HasField(renderingItem, productCodeDropLinkId))
            {
                return renderingItem[productCodeDropLinkId];
            }
            return string.Empty;
        }
    }
}