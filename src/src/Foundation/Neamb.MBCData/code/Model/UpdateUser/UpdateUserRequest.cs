namespace Neambc.Neamb.Foundation.MBCData.Model.UpdateUser
{
    public class UpdateUserRequest
    {
        public string username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string Dob { get; set; }
        public string Phone { get; set; }
        public string PermissionIndicator { get; set; }
        public int webUserId { get; set; }
        public int UnionId { get; set; }
     }
}