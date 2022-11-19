namespace Neambc.Neamb.Foundation.MBCData.Model.RetrieveUser
{
    public class RegisterUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int UnionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string Dob { get; set; }
        public string PermissionIndicator { get; set; }
        public string WebUserSource { get; set; }
        public string Phone { get; set; }
        public string CellCode { get; set; }
        public string CampCode { get; set; }
    }
}