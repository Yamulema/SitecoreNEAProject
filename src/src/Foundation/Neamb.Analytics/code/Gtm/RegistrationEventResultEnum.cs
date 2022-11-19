using System.ComponentModel;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
	public enum RegistrationEventResultEnum
	{
		[Description("failed")]
		Failed,
		[Description("success")]
		Success,
		[Description("none")]
		None
	}
}