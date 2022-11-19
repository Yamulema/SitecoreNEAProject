using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface IEncryptionService
    {
        string Mbencode(string value);
    }
}