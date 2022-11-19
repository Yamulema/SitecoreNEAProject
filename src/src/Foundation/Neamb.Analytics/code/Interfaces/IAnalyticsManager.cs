using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Foundation.Analytics.Interfaces
{
    public interface IAnalyticsManager
    {
        void TrackSiteSearch(string keyword);
        bool SetGoal(Item goalItem);
    }
}
