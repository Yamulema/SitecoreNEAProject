using Moq;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using MN = Neambc.Neamb.Foundation.Product.Manager;


namespace Neambc.Neamb.Foundation.Product.UnitTest.Manager {
	[TestFixture]
	public class ComingSoonManagerTest {
		#region Fields
		private Mock<IUserStatusHandler> _userStatusHandler;
		private Mock<IEligibilityManager> _eligibilityManager;
		private Mock<IOracleDatabase> _oracleManager;
		#endregion

		[SetUp]
		public void SetUp() {
			_userStatusHandler = new Mock<IUserStatusHandler>();
			_eligibilityManager = new Mock<IEligibilityManager>();
			_oracleManager = new Mock<IOracleDatabase>();
		}

		[Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void Return_Unforbidden_UserNotEligible() {
            //Arrange
            const string renderingId = "{5EA33232-AC25-42E5-A550-6C9232F318ED}";
            const string eligibilityId = "{5EA33232-AC25-42E5-A550-6C9232F318EF}";
            var mdsid = "991";
			var productCode = "2";
			var expectedValue = new OperationResult { ResultUrl = ResultUrlEnum.UnForbidden };

			_userStatusHandler.Setup(x => x.GetUrlUserNotAuthenticated()).Returns("");
			var accountUserBase = new AccountUserBase { Mdsid = mdsid, Username = "Jenny", Profile = new Profile() };

			var comingSoonManager = new MN.ComingSoonManager(_eligibilityManager.Object, _oracleManager.Object);
            using (var db = new Db {
                new DbItem("renderingItem", new ID(renderingId)) {
                    Fields = {
                        new DbField("Eligibility", new ID(eligibilityId)) {
                            Value = "1"
                        }
                    }
                }
            }) {
                var comingSoonModel =
                    new ComingSoonModel {
                        AccountUser = accountUserBase,
                        ReminderId = productCode,
                        UrlReturn = "",
                        ContextItemId = renderingId,
                        EligibilityItemId = eligibilityId
                    };
                //Act
                var result = comingSoonManager.ExecuteProcess(comingSoonModel);

                //Assert
                Assert.AreEqual(result.ResultUrl, expectedValue.ResultUrl);
            }
        }

		[Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void Return_RedirectUrl_UserNotEligible() {
            //Arrange
            const string renderingId = "{5EA33232-AC25-42E5-A550-6C9232F318ED}";
            const string eligibilityId = "{5EA33232-AC25-42E5-A550-6C9232F318EF}";
            var url = "/neamb.local";
			var mdsid = "991";
			var productCode = "097 01";
			var expectedValue = new OperationResult { Url = url };

			_userStatusHandler.Setup(x => x.GetUrlUserNotAuthenticated()).Returns("");
			_eligibilityManager.Setup(x => x.IsMemberEligible(mdsid, productCode, 12)).Returns(EligibilityResultEnum.Eligible);

			var accountUserBase = new AccountUserBase { Mdsid = mdsid, Username = "Jenny", Profile = new Profile() };
            

            var comingSoonManager = new MN.ComingSoonManager(_eligibilityManager.Object, _oracleManager.Object);
            using (var db = new Db {
                new DbItem("renderingItem", new ID(renderingId)) {
                    Fields = {
                        new DbField("Eligibility", new ID(eligibilityId)) {
                            Value = "1"
                        }
                    }
                }
            }) {
                var comingSoonModel =
                    new ComingSoonModel {
                        AccountUser = accountUserBase,
                        ReminderId = productCode,
                        UrlReturn = url,
                        ContextItemId = renderingId,
                        EligibilityItemId = eligibilityId
                    };
                //Act
                var result = comingSoonManager.ExecuteProcess(comingSoonModel);

                //Assert
                Assert.AreEqual(result.Url, expectedValue.Url);
            }
        }
	}
}
