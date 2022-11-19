using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class RegistrationViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string BirthDate { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string ConfirmPassword { get; set; }
        public string CurrentPassword { get; set; }
        public string Email { get; set; }
        public string EmailPermission { get; set; }
        public string Phone { get; set; }
        public bool OptIn { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string GtmAction { get; set; }
        public string RequestedPage { get; set; }
        public string UserFullName { get; set; }
        public string ImageAvatar { get; set; }
        public string UpdateAvatarLink { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsValid { get; set; }
        public string UrlRedirection { get; set; }
    }
}