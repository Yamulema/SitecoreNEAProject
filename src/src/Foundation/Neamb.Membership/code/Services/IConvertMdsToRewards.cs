using System.Collections.Generic;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.Services {
	/// <summary>
	/// Extends the Oracle Manager for the Rewards domain
	/// </summary>
	public interface IConvertMdsToRewards : IOracleDatabase {
		/// <summary>
		/// All rewards in the DB view "view_unredeemed_rewards" for the given MdsId
		/// </summary>
		/// <param name="mdsId">Required.  Cannot be null</param>
		/// <returns>An empty set if mdsId unknown</returns>
		IEnumerable<Reward> Unredeemed(string mdsId);
	}
}
