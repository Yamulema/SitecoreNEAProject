using Neambc.Neamb.Foundation.MBCData.Model.RetrieveUser;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.Interfaces
{
    public interface IRetrieveUserManager
    {
        bool TryRetrieveUserDataSeiumb(string mdsid, out RetrieveUserModel userDataResponse);
        RetrieveUserModel RetrieveUserNeamb(string mdsid);
        RetrieveUserModel RetrieveUserSeiumb(string mdsid);
        Profile ToProfileModel(RetrieveUserModel userModel);
    }
}
