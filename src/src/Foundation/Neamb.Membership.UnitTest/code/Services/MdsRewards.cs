using System;
using System.Linq;
using Moq;
using Neambc.Neamb.Foundation.MBCData.Db;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Membership.Services;
using Neambc.Seiumb.Foundation.Sitecore;
using NUnit.Framework;
using SUT = Neambc.Neamb.Foundation.Membership.Services;

namespace Neambc.Neamb.Foundation.Membership.UnitTest.Services {

	[TestFixture]
	public class MdsRewards {

		#region Fields
		private Mock<SUT.MdsRewards> _mdsRewardsMock;
		private SUT.MdsRewards _mdsRewards;
		private Mock<IConnectedCommandFactory> _cmdFactory;
		private Mock<ILog> _log;
		#endregion

		[OneTimeSetUp]
		public void SetUp() {
			_cmdFactory = new Mock<IConnectedCommandFactory>();
			_log = new Mock<ILog>();
			_mdsRewardsMock = new Mock<SUT.MdsRewards>(_cmdFactory.Object, _log.Object, string.Empty) {
				CallBase = true
			};
			_mdsRewards = _mdsRewardsMock.Object;
		}
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
		public void Unredeemed_RejectsBadMdsId() {
			Assert.Throws<ArgumentException>(() => _mdsRewards.Unredeemed(null));
			Assert.Throws<ArgumentException>(() => _mdsRewards.Unredeemed(string.Empty));
		}
		[Test]
		public void Unredeemed_Succeeds() {
			var vur = CreateSample();
			_mdsRewardsMock
				.Setup(x => x.SelectUnredeemedRewards("123"))
				.Returns(new[] { vur });
			var expected = vur.ToReward();

			var results = _mdsRewards.Unredeemed("123");

			var result = results.First();
			Assert.AreEqual(expected.Mdsid, result.Mdsid);
			Assert.AreEqual(expected.Name, result.Name);
			Assert.AreEqual(expected.Description, result.Description);
			Assert.AreEqual(expected.Date, result.Date);
			Assert.AreEqual(expected.Value, result.Value);
		}
	}

}
