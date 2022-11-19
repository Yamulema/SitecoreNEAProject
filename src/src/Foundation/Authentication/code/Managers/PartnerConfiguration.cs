using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Foundation.Authentication.Enums;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Models;

namespace Neambc.Seiumb.Foundation.Authentication.Managers
{
    [Service(typeof(IPartnerConfiguration))]
    public class PartnerConfiguration : IPartnerConfiguration
    {
        public IEnumerable<string> ProgramCodes { get; set; }
        public IDictionary<CtaType, Cta> Ctas { get; set; }
        public string Token { get; set; }

        public PartnerConfiguration()
        {
            Ctas = new Dictionary<CtaType, Cta>();
            ProgramCodes = new List<string>();
        }
    }
}