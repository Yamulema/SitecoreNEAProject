using Neambc.Neamb.Foundation.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
    [Service(typeof(ITokenManager))]
    public class TokenManager: ITokenManager
    {
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;

        public TokenManager(ISessionAuthenticationManager sessionAuthenticationManager)
        {
            _sessionAuthenticationManager = sessionAuthenticationManager;
        }
        /// <summary>
        /// Get the NcesId from the user profile 
        /// </summary>
        /// <returns>NcesId value</returns>
        public string GetNcesId()
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            return accountMembership?.Profile?.NcesId ?? string.Empty;
        }
    }
}