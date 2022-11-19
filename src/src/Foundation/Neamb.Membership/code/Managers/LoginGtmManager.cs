using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
    [Service(typeof(ILoginGtmManager))]
    public class LoginGtmManager : ILoginGtmManager
    {
        private readonly IGtmService _gtmService;
        
        public LoginGtmManager(IGtmService gtmService)
        {
            _gtmService = gtmService;            
        }

        /// <summary>
        /// Get the login gtm action
        /// </summary>
        /// <param name="loginGtmStatus">Submitted or failed</param>
        /// <returns></returns>
        public string GetGtmFunction(LoginGtmStatus loginGtmStatus)
        {
            LoginGtmBase loginGtm = new LoginGtmBase();
            loginGtm.LoginResult = loginGtmStatus.ToString().ToLower();
            return _gtmService.GetGtmEvent(loginGtm);
            
        }
    }
}