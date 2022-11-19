using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Seiumb.Foundation.Authentication.Models;

namespace Neambc.Seiumb.Foundation.Authentication.Interfaces
{
    public interface ISeiumbProfileManager {
        bool IsRakutenMember();
        MemberCreationResponse GetRakutenMemberCreationResponse();
        SeiuProfile GetProfile();
        void DeleteProfile();
        void SaveProfile(SeiuProfile profile, bool verifyExistence = false);
        void SaveFavoriteStore(MemberCreationResponse rakutenResponse);
        bool InHotState();
    }
}