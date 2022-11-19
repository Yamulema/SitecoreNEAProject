using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Contest.Model;

namespace Neambc.Neamb.Feature.Contest.Interfaces
{
    public interface IFallbackProvider
    {
        IList<Vote> GetContestBackup(string contestId);
        void SetContestBackup(IList<Vote> votes);
    }
}