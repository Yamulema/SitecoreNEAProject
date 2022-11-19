using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Model.Login;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface IAuthenticationRepository
    {
        int IsUsernameAvailable(string username);
        LoginResponse ValidateUsernameAndPassword(string username, string password, string cellcode,string matchRoutine);
		bool AuthenticatePassword(string username, string password);
    }
}