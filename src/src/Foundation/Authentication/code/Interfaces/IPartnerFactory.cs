using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface IPartnerFactory
    {
        IPartner GetPartner(string productCode);
    }
}
