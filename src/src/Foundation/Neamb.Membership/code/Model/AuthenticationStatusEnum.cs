

namespace Neambc.Neamb.Foundation.Membership.Model
{
	/// <summary>
	/// Status that is returned in response of webservice LoginUser
	/// </summary>
	public enum AuthenticationStatusEnum
	{
		Ok = 0,
		InvalidUsernamePassword = 3,
		//AccountLocked = 2,
		//UserNameNotExist = 1,
		//AccessDenied = 10000,
		//InputDataValidationError = 10001,
        AcccountLockedRestService = 12004
    }
}