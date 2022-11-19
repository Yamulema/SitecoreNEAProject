using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.Services {

	public static class ViewUnredeemedRewardsExtensions {
		public static Reward ToReward(this ViewUnredeemedRewards vur) {
			return new Reward() {
				Mdsid = vur.IndvId,
				Name = vur.RewardsNm,
				Description = vur.UserRewardsDesc,
				Date = vur.DateAwarded,
				Value = vur.AwardedVal
			};
		}
	}
}