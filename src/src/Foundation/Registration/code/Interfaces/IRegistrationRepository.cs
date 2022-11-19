using Neambc.Neamb.Foundation.MBCData.Model.RegisterUser;

namespace Neambc.Seiumb.Foundation.Registration.Interfaces
{
    public interface IRegistrationRepository
    {
        RegisterUserResponse RegisterUser(string firstName, string lastName, string streetAddress, string city,
            string stateCode,
            string zipCode, string dob, string phone, string username, string password, string permissionIndicator,
            string campcode, string cellCode);
    }
}