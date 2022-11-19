namespace Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests
{
    public class UpdateUserNameRestRequestDto : RestRequestDto
    {
        public string CurrentUsername { get; set; }
        public string NewUsername { get; set; }
        public string ConfirmNewUsername { get; set; }
        public string UnionId { get; set; }

    }
}