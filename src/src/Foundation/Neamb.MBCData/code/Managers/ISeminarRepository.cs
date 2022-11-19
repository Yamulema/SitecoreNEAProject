using System.Collections.Generic;
using Neambc.Neamb.Foundation.MBCData.Model;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
	public interface ISeminarRepository
    {
        IReadOnlyList<ViewSeminar> GetSeminaries();
        ViewSeminar GetSeminary(Item renderingItem);
        string GetSeminarId(Item renderingItem);
        string GetLeaCode(Item renderingItem);
        bool IsValidSeminaryId(Item renderingItem);
    }
}
