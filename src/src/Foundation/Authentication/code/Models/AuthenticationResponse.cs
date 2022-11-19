using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Seiumb.Foundation.Authentication.Enums;

namespace Neambc.Seiumb.Foundation.Authentication.Models
{
    public class AuthenticationResponse
    {
        public LoginUserErrorCodeEnum Status { get; set; }
        public string ReturnUrl { get; set; }
        public bool IsEligible { get; set; }
        public LoginErrors LoginErrors { get; set; }
        public bool LoggedIn { get; set; }

        public AuthenticationResponse() {
            LoggedIn = false;
        }
    }
}