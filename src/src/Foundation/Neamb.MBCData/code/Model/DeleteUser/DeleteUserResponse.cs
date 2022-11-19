using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Model.DeleteUser
{
    
   public class DeleteUserResponse : RestBaseResponse
    {
        public DeleteUserResponseData Data { get; set; }
        public bool IsAuthenticated { get; set; }
        public DeleteUserResponse()
        {
            Data = null;
            IsAuthenticated = false;
        }

    }

}