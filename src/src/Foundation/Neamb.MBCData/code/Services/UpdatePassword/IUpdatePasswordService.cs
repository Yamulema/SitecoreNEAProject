using Neambc.Neamb.Foundation.MBCData.Model.PasswordManagement.UpdatePassword;

namespace Neambc.Neamb.Foundation.MBCData.Services.UpdatePassword
{
    public interface IUpdatePasswordService
    {
        /// <summary>
        /// Change the password of the user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="currentPassword">CurrentPassword</param>
        /// <param name="newPassword">New Password</param>
        /// <param name="confirmNewPassword">Confirm New Password</param>
        /// <param name="unionId">Seiumb/Neamb</param>
        /// <returns>Response of the AWS WebService</returns>
        UpdatePasswordMgmtResponse UpdatePassword(string username, string currentPassword, 
            string newPassword, string confirmNewPassword, int unionId);
    }
}
