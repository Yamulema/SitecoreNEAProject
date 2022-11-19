using Neambc.Neamb.Foundation.MBCData.Model.RetrieveUser;

namespace Neambc.Neamb.Foundation.MBCData.Services.RetrieveUser
{
    public interface IRetrieveUserService {
        /// <summary>
        /// Gets user data with the mdsid provided
        /// </summary>
        /// <param name="mdsid">mdsid</param>
        /// <param name="unionId">Seiumb/Neamb</param>
        /// <returns>Response of the AWS WebService</returns>
        RetrieveUserModel RetrieveUserData(int mdsid, int unionId);
    }
}
