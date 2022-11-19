using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Db;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.Membership.Services {

	[Service(typeof(IConvertMdsToRewards))]
	public class MdsRewards : OracleDatabase, IConvertMdsToRewards {

		public MdsRewards(IConnectedCommandFactory cmdFactory, ILog log, string connectionString = null)
			: base(cmdFactory, log, connectionString) { }

		public virtual IEnumerable<Reward> Unredeemed(string mdsId) {
			if (string.IsNullOrEmpty(mdsId)) {
				throw new ArgumentException("mdsId cannot be null/empty");
			}
			return SelectUnredeemedRewards(mdsId).Select(x => x.ToReward());
		}
	}
}
