using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.Interfaces
{
    public interface IRewardService
    {
        IEnumerable<Reward> GetRewards();
        int GetTotalPoints();
    }
}