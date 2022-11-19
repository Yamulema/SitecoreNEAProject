using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Seiumb.Foundation.Authentication.Models;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface IPassthroughService
    {
        AuthenticationResponse Authenticate(PassthroughRequest passthroughRequest);
    }
}