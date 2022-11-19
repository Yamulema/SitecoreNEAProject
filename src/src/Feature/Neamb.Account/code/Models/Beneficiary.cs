using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Account.Enums;

namespace Neambc.Neamb.Feature.Account.Models
{
    public class Beneficiary
    {
        public string Id { get; set; }
        public string OtherEntityName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string Email { get; set; }
        public string Relationship { get; set; }
        public string DisplayName { get; set; }
        public string DisplayRelationship { get; set; }
        public int Share { get; set; }
        public string DisplayShare { get; set; }
        public BeneficiaryType Type { get; set; }
    }
}