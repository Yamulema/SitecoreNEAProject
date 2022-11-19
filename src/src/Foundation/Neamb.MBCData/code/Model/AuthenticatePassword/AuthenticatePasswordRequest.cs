namespace Neambc.Neamb.Foundation.MBCData.Model.AuthenticatePassword
{
    public class AuthenticatePasswordRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public int unionId { get; set; }
    }
}