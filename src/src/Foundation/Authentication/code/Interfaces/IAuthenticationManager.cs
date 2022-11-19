using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Seiumb.Foundation.Authentication.Models;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface IAuthenticationManager
    {
        bool AuthenticatePassword(string username, string password);
        LoginResponse AuthenticateUser(SeiuProfile profile, string username, string password, string cellcode);
        void FillUserData(SeiuProfile seiuProfile, string mdsid, string neaCookieMdsid, bool fromLogin = false, string registrations = null);
    }
}