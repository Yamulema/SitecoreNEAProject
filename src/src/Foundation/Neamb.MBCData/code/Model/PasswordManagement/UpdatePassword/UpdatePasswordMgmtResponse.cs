using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.PasswordManagement.UpdatePassword
{
    public class UpdatePasswordMgmtResponse : RestBaseResponse
    {
        public UpdatePasswordModel Data { get; set; }
        public UserAccountErrorCodesEnum ErrorCodeResponse { get; set; }
    }
}