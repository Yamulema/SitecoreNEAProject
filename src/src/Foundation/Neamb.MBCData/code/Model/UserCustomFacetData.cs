using System;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
    public class UserCustomFacetData
    {
        public string Username { get; set; }
        public string Mdsid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string UnionId { get; set; }
        public string EmailPermissionIndicator { get; set; }
        public bool IsRegistered { get; set; }
        [Obsolete("Using the new IsRegistered bool property", false)]
        public string Registered => IsRegistered ? "Y" : string.Empty;
        public string Webuserid { get; set; }
        public string IAId { get; set; }
        public bool IsNeaCurrentMember { get; set; }
        [Obsolete("Using the new IsNeaCurrentMember bool property", false)]
        public string NeaCurrentMemberFlag => IsNeaCurrentMember ? "Y" : string.Empty;
        public string NeaMembershipType { get; set; }
        public string SeaNumber { get; set; }
        public string SeaName { get; set; }
        public string MembershipCategoryCode { get; set; }
        public bool IsSeiuCurrentMember { get; set; }
        [Obsolete("Using the new IsSeiuCurrentMember bool property", false)]
        public string SeiuCurrentMemberFlag => IsSeiuCurrentMember ? "Y" : string.Empty;
        public string SeiuLocalNumber { get; set; }
        public string SeiuLocalName { get; set; }
        public string NewEnvInd { get; set; }
        public string ComplifesignDate { get; set; }
        public string GenderCode { get; set; }
        public string Introlifeenddate { get; set; }
        public string Newmembersegmentindicator { get; set; }
        public string LeaName { get; set; }
        public string LeaNumber { get; set; }
    }
}