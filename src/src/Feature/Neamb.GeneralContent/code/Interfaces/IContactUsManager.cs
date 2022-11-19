using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Interfaces
{
    public interface IContactUsManager {
		string GetGtmAction(Item contextItem);
    }
}