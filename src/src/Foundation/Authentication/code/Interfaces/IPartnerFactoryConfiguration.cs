using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neambc.Seiumb.Foundation.Authentication.Enums;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface IPartnerFactoryConfiguration
    {
        IDictionary<PartnerType, IPartnerConfiguration> Partners { get; set; }
    }
}
