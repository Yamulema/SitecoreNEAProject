

namespace Neambc.Neamb.Foundation.MBCData.Model
{
	/// <summary>
	/// Status that is returned in response of webservice RegisterUser
	/// </summary>
	public enum RegisterUserResponseEnum
	{
		Ok = 0,
		AccessDenied=10000,
		InputDataValidationError = 10001,
		InputRecordFailedError = 10002
	}
}