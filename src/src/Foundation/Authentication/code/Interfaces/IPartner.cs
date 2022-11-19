using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface IPartner
    {
        string GetActionPrimary(IDictionary<string,string> queryStringParameters);
        string GetActionSecondary(IDictionary<string, string> queryStringParameters);
        string GetToken();
    }
}
