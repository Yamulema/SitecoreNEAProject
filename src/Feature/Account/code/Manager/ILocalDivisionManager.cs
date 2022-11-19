using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Seiumb.Feature.Account.Models;

namespace Neambc.Seiumb.Feature.Account.Manager
{
    public interface ILocalDivisionManager {
        IList<LocalCodeDto> GetLocalCodesGlobal();
        bool ExistLocalCodeUser(IList<LocalCodeDto> localCodeFromSitecore, string divisionToCompare);
    }
}