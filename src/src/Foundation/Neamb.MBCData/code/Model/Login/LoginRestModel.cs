using System;
using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.MBCData.Model.Login
{
    public class LoginRestModel
    {
        public bool LoggedIn { get; set; }
        public int MdsId { get; set; }
        public int RegistrationCount { get; set; }
        public List<LoginRegistration> Registrations { get; set; }
        public string MdsIdAsString { get; set; }
        public string RegistrationDuplicateOldFormat { get; set; }
    }
}