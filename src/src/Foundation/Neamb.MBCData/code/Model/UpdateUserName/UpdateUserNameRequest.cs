namespace Neambc.Neamb.Foundation.MBCData.Model.UpdateUserName
{
    public class UpdateUserNameRequest
    {
        public string CurrentUsername { get; set; }
        public string NewUsername { get; set; }
        public string ConfirmNewUsername { get; set; }
        public string UnionId { get; set; }
     }
}