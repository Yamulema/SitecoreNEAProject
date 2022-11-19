using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Services
{

    public interface ISmartPublish
    {
        void PublishItem(ID parentId);
        void PublishItem(Item rootItem, bool withChildren = true);
    }
}