
using Neambc.Neamb.Foundation.MBCData.Model.DeleteUser;

namespace Neambc.Neamb.Foundation.MBCData.Services.DeleteUser
{
    public interface IDeleteUserService
    {
        DeleteUserResponse DeleteUserStatus(string username, int unionId);
    }
}