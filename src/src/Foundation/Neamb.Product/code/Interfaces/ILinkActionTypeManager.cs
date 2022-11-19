using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Product.Interfaces
{
	public interface ILinkActionTypeManager
	{
		OperationResult GetUrlLink(LinkModel linkModel);
		string RemoveAllEmptyParameters(string url);
        string ReplaceToken(string url, string mdsid, PassthroughModel passthrougData);
    }
}