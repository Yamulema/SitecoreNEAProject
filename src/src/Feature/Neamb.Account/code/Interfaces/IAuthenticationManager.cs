using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Account.Models;

namespace Neambc.Neamb.Feature.Account.Interfaces
{
    public interface IAuthenticationManager
    {
        AuthenticationResultEnum ExecuteAuthentication(ModelStateDictionary modelstate, string ckbrememberme,
            string pathReset, AccountDTO modelaccount, ViewDataDictionary viewData, bool executeLogout=true);
        
    }
}