using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Contest.Enums
{
    public enum ContestStatus
    {
        None,
        RequiresAuthentication,
        NotStarted,
        Closed,
        IsExperienceEditor,
        Active
    }
}