using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Contest.Model;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Contest.Interfaces
{
    public interface IContestVoteManager
    {
        ContestVote GetContestModel(Rendering rendering, int page);
    }
}