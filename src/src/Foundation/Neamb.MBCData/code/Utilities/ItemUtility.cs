using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;

namespace Neambc.Neamb.Foundation.MBCData.Utilities
{
    public class ItemUtility
    {
        public static bool HasField(Item item, ID fieldId)
        {
            return TemplateManager.IsFieldPartOfTemplate(fieldId, item);
        }
    }
}