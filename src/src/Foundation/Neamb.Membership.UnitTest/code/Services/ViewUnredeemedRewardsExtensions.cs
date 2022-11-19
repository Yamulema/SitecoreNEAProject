using System;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Membership.Services;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.Membership.UnitTest.Services {
	[TestFixture]
	public class ViewUnredeemedRewardsExtensions {

		private static ViewUnredeemedRewards CreateSample() {
			return new ViewUnredeemedRewards() {
				AwardedVal = 10,
				DateAwarded = new DateTime(1980, 1, 1),
				IndvId = 11,
				UserRewardsDesc = "sample description",
				RewardsNm = "name"
			};
		}

		[Test]
		public void ToReward_TransformsToReward() {
			var vur = CreateSample();
			var expected = new Reward {
				Mdsid = vur.IndvId,
				Name = vur.RewardsNm,
				Description = vur.UserRewardsDesc,
				Date = vur.DateAwarded,
				Value = vur.AwardedVal
			};

			var result = vur.ToReward();
			Assert.AreEqual(expected.Mdsid, result.Mdsid);
			Assert.AreEqual(expected.Name, result.Name);
			Assert.AreEqual(expected.Description, result.Description);
			Assert.AreEqual(expected.Date, result.Date);
			Assert.AreEqual(expected.Value, result.Value);
		}
	}

}
