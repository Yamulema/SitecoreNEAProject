namespace Neambc.Neamb.Foundation.MBCData.Services.ValidateResetToken
{
    public interface IValidateResetTokenService
    {
        /// <summary>
        /// Validate a password reset token for a user.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="resetToken">Reset token to be validated</param>
        /// <param name="unionId">Seiumb/Neamb</param>
        /// <returns>Response of the AWS WebService</returns>
        bool ValidateResetToken(string username, string resetToken, int unionId);
    }
}
