using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Services.SearchUserName;
using Neambc.Neamb.Foundation.MBCData.Services.UpdateUserName;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.WebServices;


namespace Neambc.Seiumb.Foundation.Authentication.Repositories
{
    [Service(typeof(IProfileRepository))]
    public class ProfileRepository : IProfileRepository
    {
        private readonly IWebServicesConfiguration _webServicesConfiguration;
        private readonly IUpdateUserNameService _updateUserNameService;
        private readonly ISearchUserNameService _searchUserNameService;

        public ProfileRepository(IWebServicesConfiguration webServicesConfiguration, IUpdateUserNameService updateUserNameService, ISearchUserNameService searchUserNameService)
        {
            _webServicesConfiguration = webServicesConfiguration;
            _updateUserNameService = updateUserNameService;
            _searchUserNameService = searchUserNameService;
        }

        // return:  1  if username is valid and not registered
        //          0  if username is valid but registered
        //         -1  if username is invalid
        public int IsUsernameAvailable(string username)
        {
            var response = _searchUserNameService.SearchUserName(username);
            if (!response.Success) return -1;
            else if (response.Data.Registered) return 0;
            else return 1;
        }

        public bool UpdateUsername(string currentUsername, string newUsername, string confirmNewUsername)
        {
            return _updateUserNameService.UpdateUserNameStatus(currentUsername, newUsername, confirmNewUsername, _webServicesConfiguration.UnionId);
        }
    }
}