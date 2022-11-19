using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Eligibility.Model;

namespace Neambc.Neamb.Foundation.Eligibility.Interfaces
{
    public interface IEligibilityManager
    {
        EligibilityResultEnum IsMemberEligible(string mdsid, string productcode, int months = 12);
    }
}