using System;
using Neambc.Neamb.Foundation.Membership.Enums;

namespace Neambc.Neamb.Foundation.Membership.Model
{
	/// <summary>
	/// User profile information
	/// </summary>
	[Serializable]
	public class ExactTargetResetEmail
	{
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string Token { get; set; }
		public string ResetPath { get; set; }
		public string CancelPath { get; set; }
		public string ExpiresAt { get; set; }
		public Boolean? NewToken { get; set; }
		public ResetPasswordEnum ResetPasswordEnum { get; set; }
	}
}