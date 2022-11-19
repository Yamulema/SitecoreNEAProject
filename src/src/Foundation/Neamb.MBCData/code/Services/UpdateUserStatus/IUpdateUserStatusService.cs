namespace Neambc.Neamb.Foundation.MBCData.Services.UpdateUserStatus
{
    public interface IUpdateUserStatusService
    {
        /// <summary>
        /// Verify if the username is available for creating a new user 
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="unionId">Seiumb/Neamb</param>
        /// <param name="statusCode">User’s status code that needs to be applied</param>
        /// <returns>Response of the AWS WebService</returns>
        bool UpdateUserStatus(string username, string statusCode, int unionId);
    }
}
