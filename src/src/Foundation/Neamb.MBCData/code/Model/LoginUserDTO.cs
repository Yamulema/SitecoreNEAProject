using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
    public class LoginUserDto
    {
        public string Description { get; set; }

        public string Mdsid { get; set; }

        public string Registrations { get; set; }

        public string Returncode { get; set; }

        public string Status { get; set; }

        public int Registrationcount { get; set; }

        public bool RegistrationcountSpecified { get; set; }
    }
}