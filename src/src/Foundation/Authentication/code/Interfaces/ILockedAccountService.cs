using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Seiumb.Foundation.Authentication.Enums;
using Sitecore.Data.Fields;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface ILockedAccountService
    {
        LoginErrors HandleLockedAccount(string username, out bool isUsernameValid, bool isFromUserLocked = false);
        LoginErrors HandleLockedAccount(string username, LinkField cancelLink, LinkField resetLink,
            out bool isUsernameValid, bool isFromUserLocked = false);
    }
}