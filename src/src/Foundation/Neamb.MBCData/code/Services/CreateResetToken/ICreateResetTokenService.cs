using Neambc.Neamb.Foundation.MBCData.Model.CreateResetToken;

namespace Neambc.Neamb.Foundation.MBCData.Services.CreateResetToken
{
    public interface ICreateResetTokenService
    {
        /// <summary>
        /// Create a password reset token for a user.
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="unionId">Seiumb/Neamb</param>
        /// <returns></returns>
        /// <returns>Response of the AWS WebService</returns>
        CreateResetTokenResponse CreateResetToken(string username, int unionId);
    }
}
