namespace Neambc.Neamb.Foundation.MBCData.Model.ValidateResetToken
{
    public class ValidateResetTokenRequest
    {
        public string Username { get; set; }
        public int UnionId { get; set; }
        public string ResetToken { get; set; }
    }
}