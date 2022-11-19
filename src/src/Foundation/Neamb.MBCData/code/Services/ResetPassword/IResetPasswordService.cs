using Neambc.Neamb.Foundation.MBCData.Model.PasswordManagement.ResetPassword;

namespace Neambc.Neamb.Foundation.MBCData.Services.ResetPassword
{
    public interface IResetPasswordService
    {
        /// <summary>
        /// Reset the password of the user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="newPassword">New Password</param>
        /// <param name="confirmNewPassword">Confirm New Password</param>
        /// <param name="unionId">Seiumb/Neamb</param>
        /// <returns>Response of the AWS WebService</returns>
        ResetPasswordMgmtResponse ResetPassword(string username, 
            string newPassword, string confirmNewPassword, int unionId);
    }
}
