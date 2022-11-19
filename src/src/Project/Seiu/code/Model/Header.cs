using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.MBCData;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Registration.Interfaces;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Neambc.Seiumb.Project.Seiu.Model
{
    public class Header
    {
        public Item SiteSettings { get; set; }
        public string HomeUrl { get; set; }
        public string ProfileUrl { get; set; }
        public string Mdsidco { get; set; }
        public string Mdsidqs { get; set; }
        public string Mdsid { get; set; }
        public string UserMdsid { get; set; }

        public LinkField RegistrationLink { get; set; }
        public string RegistrationLinkUrl { get; set; }
        public LinkField LoginLink { get; set; }
        public string LoginLinkUrl { get; set; }
        public string[] Salutations { get; set; }
        public string UserStatus { get; set; }
        public string FirstSalutation { get; set; }
        public string SecondSalutation { get; set; }
        public string SeaName { get; set; }
        public string FirstName { get; set; }
        public string OnClickEventContentLogin { get; set; }
        public string OnClickEventContentRegistration { get; set; }
    }
}