namespace Neambc.Neamb.Foundation.MBCData.Model.CreateResetToken
{
    public class CreateResetTokenModel
    {
        public bool NewToken { get; set; }
        public string ResetToken { get; set; }
        public string FirstName { get; set; }
        public string ExpiresAt { get; set; }
    }
}