
namespace Neambc.Neamb.Foundation.MBCData.Model.ApiSecurity
{
    public class ApiSecurityRequest
    {
        public string PlainText { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public int PasswordIterations { get; set; }
        public int KeySize { get; set; }
    }
}