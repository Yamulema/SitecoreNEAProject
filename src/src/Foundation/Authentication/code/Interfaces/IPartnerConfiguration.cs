using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neambc.Seiumb.Foundation.Authentication.Enums;
using Neambc.Seiumb.Foundation.Authentication.Models;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface IPartnerConfiguration
    {
        IEnumerable<string> ProgramCodes { get; set; }
        IDictionary<CtaType, Cta> Ctas { get; set; }
        string Token { get; set; }
    }
}
