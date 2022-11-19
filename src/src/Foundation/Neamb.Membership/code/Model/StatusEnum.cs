

namespace Neambc.Neamb.Foundation.Membership.Model {
	/// <summary>
	/// User status
	/// </summary>
	public enum StatusEnum {
		LockedNewToken,
		LockedOldToken,
		Duplicated,
		Registered,
		Unknown,
		Hot,
		WarmHot,
		WarmCold,
		Cold
	}
}