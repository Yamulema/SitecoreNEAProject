using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Product.Models
{
    public class UserData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginUserId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string Zipcode { get; set; }
        public string MdsId { get; set; }
        public string MdsIndvId { get; set; }
        public string Phone { get; set; }
        public string SeiuLocNum { get; set; }
    }
}