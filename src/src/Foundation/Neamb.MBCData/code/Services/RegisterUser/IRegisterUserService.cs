using Neambc.Neamb.Foundation.MBCData.Model.RegisterUser;

namespace Neambc.Neamb.Foundation.MBCData.Services.RegisterUser
{
    public interface IRegisterUserService
    {
        RegisterUserResponse RegisterUserData(string firstName,
            string lastName,
            string streetAddress,
            string city,
            string stateCode,
            string zipCode,
            string dob,
            string phone,
            string username,
            string password,
            string permissionIndicator,
            string campcode,
            string cellCode, int unionId, string webusersource);
    }
}
