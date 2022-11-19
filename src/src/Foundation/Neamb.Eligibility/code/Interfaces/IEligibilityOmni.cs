using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Neamb.Foundation.Eligibility.Interfaces
{
    public interface IEligibilityOmni {
        IList<ViewOmni> CheckEligibility(string mdsid, string productCode);
    }
}