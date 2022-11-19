using Neambc.Neamb.Foundation.MBCData.Model.Login;

namespace Neambc.Neamb.Foundation.MBCData.Services.Login
{
    public interface ILoginUserService
    {
        LoginResponse LoginUser(string userName, string password, int unionId, string cellcode, string matchRoutineIdentifier);
    }
}
