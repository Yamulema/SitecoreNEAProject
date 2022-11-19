using System;
using Neambc.Neamb.Foundation.Membership.Enums;

namespace Neambc.Neamb.Foundation.Membership.Model
{
    public class LoginGtmBase
    {
        public string Event { get; set; }
        public string LoginAction { get; set; }
        public string LoginResult { get; set; }
        public LoginGtmBase()
        {
            Event = "login";
            LoginAction = "submit";
        }        
    }
}