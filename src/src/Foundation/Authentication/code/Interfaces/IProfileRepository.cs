using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface IProfileRepository
    {
		bool UpdateUsername(string currentUsername, string newUsername, string confirmNewUsername);
        int IsUsernameAvailable(string username);
    }
}