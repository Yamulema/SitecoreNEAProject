namespace Neambc.Neamb.Foundation.MBCData.Model.PasswordManagement
{
    public class PasswordMgmtRequest
    {
        public string Username { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public int UnionId { get; set; }
    }
}