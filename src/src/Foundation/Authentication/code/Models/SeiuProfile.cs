namespace Neambc.Seiumb.Foundation.Authentication.Models {
    public class SeiuProfile {
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
        public string EmailPermission { get; set; }
        public string EmailPermissionIndicator { get; set; }
        public string Status { get; set; }
        public string DuplicateEmail { get; set; }
        public string Webuserid { get; set; }
        public string MdsId { get; set; }
        public string LocalUnion { get; set; }
        public string Emails { get; set; }
        public string SeaName { get; set; }
        public string Registered { get; set; }
        public string MembershipCategoryCode { get; set; }
        public string MembershipType { get; set; }
        public string SeiuCurrentMember { get; set; }
        public string SeaNumber { get; set; }
        public string SeiuLocalName { get; set; }
        public string SeiuLocalNumber { get; set; }
        public string GenderCode { get; set; }
        public string IntroLifeEndDate { get; set; }
        public string NewMemberSegmentIndicator { get; set; }
        public string NewEnvId { get; set; }
        public string LeaNumber { get; set; }
        public string LeaName { get; set; }
        public string CompLifeSignDate { get; set; }
        public string QueryString { get; set; }
        public int MdsIdInt => int.TryParse(MdsId, out var result) ? result : 0;
    }
}