using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Authentication.Models;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface IUserRepository
    {
        bool CreateVirtualUser(string username, SeiuProfile seiuProfile, LoginResponse response = null, bool duplicateRegistrationProcess = false);
        void FillUserData(Profile user, SeiuProfile seiuProfile, string mdsid, bool fromLogin, string registrations = null);
        string GetUserStatus();
        void LogoutUser();
    }
}