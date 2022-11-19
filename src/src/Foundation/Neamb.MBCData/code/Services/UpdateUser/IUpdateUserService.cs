using Neambc.Neamb.Foundation.MBCData.Model.UpdateUser;
namespace Neambc.Neamb.Foundation.MBCData.Services.UpdateUser
{
    public interface IUpdateUserService
    {
        UpdateUserResponse UpdateUser(string UserName, int WebuserId, string FirstName, string LastName, string StreetAddress, string City, string StateCode, string ZipCode, string Dob, string Phone, string PermissionIndicator, int UnionId);
    }
}
