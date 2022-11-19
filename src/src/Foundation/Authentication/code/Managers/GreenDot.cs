using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Seiumb.Foundation.Authentication.Enums;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;

namespace Neambc.Seiumb.Foundation.Authentication.Managers
{
    public class GreenDot : IPartner
    {
        private readonly IPartnerConfiguration _partnerConfiguration;

        public GreenDot(IPartnerConfiguration partnerConfiguration)
        {
            _partnerConfiguration = partnerConfiguration;
        }

        public string GetActionPrimary(IDictionary<string, string> queryStringParameters)
        {
            return _partnerConfiguration.Ctas.FirstOrDefault(x => x.Key == CtaType.Primary).Value?.Url ?? string.Empty;
        }

        public string GetActionSecondary(IDictionary<string, string> queryStringParameters)
        {
            return string.Empty;
        }

        public string GetToken()
        {
            return string.Empty;
        }
    }
}