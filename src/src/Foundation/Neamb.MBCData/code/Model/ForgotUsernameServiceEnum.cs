

namespace Neambc.Neamb.Foundation.MBCData.Model
{
	/// <summary>
	/// Status that is returned in response of webservice ResetPassword
	/// </summary>
	public enum ForgotUsernameServiceEnum
	{
		Ok = 0,
		AccessDenied = 10000,
		InputDataValidationFail = 10001,
	}
}