
namespace Neambc.Neamb.Foundation.MBCData.Services.UpdateUserName
{
    public interface IUpdateUserNameService
    {
        /// <summary>
        /// update username status
        /// </summary> 
        /// <returns>
        /// </returns>
        bool UpdateUserNameStatus(string currentUsername, string newUsername, string confirmNewUsername, string unionId);
    }
}
