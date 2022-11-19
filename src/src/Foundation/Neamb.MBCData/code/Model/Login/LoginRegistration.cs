using System;

namespace Neambc.Neamb.Foundation.MBCData.Model.Login
{
    [Serializable]
    public class LoginRegistration
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegistrationDate { get; set; }
        public int WebUserId { get; set; }
        public string WebUserIdAsString { get; set; }
        
    }
}