namespace Neambc.Neamb.Foundation.MBCData.Model.RetrieveUser
{
    public class RetrieveUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string GenderCode { get; set; }
        public string Dob { get; set; }
        public bool NeaCurrentMember { get; set; } //Neacurrentmember
        public bool SeiuCurrentMember { get; set; }
        public string SeaName { get; set; }
        public int SeaNumber { get; set; }
        public string LeaName { get; set; }
        public int LeaNumber { get; set; }
        public string SeiuLocalName { get; set; }
        public int SeiuLocalNumber { get; set; }
        public string NeaMembershipType { get; set; }
        public string NeaMembershipTypeName { get; set; }
        public string MembershipCategoryCode { get; set; } //Membershipcatcode
        public string NewEnvironmentIndicator { get; set; } //Newenvind
        public string NewMemberSegmentIndicator { get; set; } //Newmembersegmentindicator
        public string EmailPermissionIndicator { get; set; } // Emailpermission
        public string IaId { get; set; }
        public string CompIntroSignDate { get; set; } //Complifesigndate
        public string CompIntroEndDate { get; set; } //Introlifeenddate
        public bool Registered { get; set; }
        public string RegistrationDate { get; set; }
        public int? UnionId { get; set; }
        public int? WebUserId { get; set; }
        public bool NeambPermissionMail { get; set; }
        public string NcesId { get; set; }
    }
}