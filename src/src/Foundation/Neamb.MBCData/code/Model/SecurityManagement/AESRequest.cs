namespace Neambc.Neamb.Foundation.MBCData.Model.SecurityManagement
{
    public class AESRequest
    {
        public string Salt { get; set; }
        public string Password { get; set; }  
        public int PasswordInterations { get; set; }
        public int KeySize { get; set; }
    }
}