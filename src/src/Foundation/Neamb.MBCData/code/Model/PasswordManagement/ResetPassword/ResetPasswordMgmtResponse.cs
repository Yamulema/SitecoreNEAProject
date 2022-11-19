using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.PasswordManagement.ResetPassword
{
    public class ResetPasswordMgmtResponse : RestBaseResponse
    {
        public ResetPasswordModel Data { get; set; }
        public UserAccountErrorCodesEnum ErrorCodeResponse { get; set; }
    }
}