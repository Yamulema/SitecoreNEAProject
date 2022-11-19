using Neambc.Neamb.Foundation.MBCData.Model.SearchUserName;

namespace Neambc.Neamb.Foundation.MBCData.Services.SearchUserName
{
    public interface ISearchUserNameService
    {
        SearchUserNameResponse SearchUserName(string username);
    }
}
