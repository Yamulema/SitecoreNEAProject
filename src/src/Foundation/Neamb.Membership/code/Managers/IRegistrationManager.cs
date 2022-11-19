using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.MBCData.Model.RegisterUser;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
	public interface IRegistrationManager
	{
        RegisterUserResponse RegisterAccount(AccountMembership account, string password, string cellcode, string campaingcode);
		string ExecuteGtmActionRegistrationRedirection(string itemId);
		string GetGtmActionRegistration(RegistrationEventResultEnum registrationEventResultEnum);
	}
}