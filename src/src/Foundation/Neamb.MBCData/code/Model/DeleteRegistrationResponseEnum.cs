

namespace Neambc.Neamb.Foundation.MBCData.Model
{
	/// <summary>
	/// Status that is returned in response of webservice RegisterUser
	/// </summary>
	public enum DeleteRegistrationResponseEnum
	{
		Ok = 0,
		AccessDenied=10000,
		InputDataValidationError = 10001,
		InputRecordFailedError = 10002,
		DeleteRecordFail = 10004,
		UserNameNotExist=1
	}
}