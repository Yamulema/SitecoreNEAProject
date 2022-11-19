using Neambc.Neamb.Foundation.MBCData.Model.PasswordManagement.UpdatePassword;

namespace Neambc.Neamb.Foundation.Membership.Interfaces
{
    public interface IAccountManager
    {
        UpdatePasswordMgmtResponse UpdatePassword(string username, string currentPassword, string newPassword, int unionId);
        bool ResetPassword(string username, string newPassword, string confirmNewPassword, int unionId);
        bool UpdateUserStatus(string email, int statusCode, int unionId);
    }
}
