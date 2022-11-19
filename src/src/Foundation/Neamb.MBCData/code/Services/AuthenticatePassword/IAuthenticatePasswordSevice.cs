using Neambc.Neamb.Foundation.MBCData.Model.AuthenticatePassword;

namespace Neambc.Neamb.Foundation.MBCData.Services.AuthenticatePassword
{
    public interface IAuthenticatePasswordService
    {
        AuthenticatePasswordResponse AuthenticatePasswordStatus(string username, string password, int unionId);
    }
}