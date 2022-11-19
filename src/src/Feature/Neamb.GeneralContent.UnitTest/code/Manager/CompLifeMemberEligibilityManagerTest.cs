using Moq;
using Neambc.Neamb.Feature.GeneralContent.Managers;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model.CompIntroLife;
using Neambc.Neamb.Foundation.MBCData.Services.CompIntroLife;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Neambc.Neamb.Feature.GeneralContent.UnitTest.Manager {
	[TestFixture]
	public class CompLifeMemberEligibilityManagerTest {

		#region Fields
		private Mock<ISessionAuthenticationManager> _sessionAuthenticationManager;
		private Mock<ICompIntroLifeService> _compIntroLifeService;
        private string _introLifeEndDate;
		private string _mdsId;
		private string _globalItemId;
		private string _fieldGlobalItemId;
		private Db _database;
		#endregion

		#region Pro/Post Actions
		[SetUp]
		public void SetUp() {
			// before each test
			_mdsId = "997";
			_introLifeEndDate = "08312019";
			_globalItemId = "{EF9E66A9-E270-43EB-8AFB-9AC82995B422}";
			_fieldGlobalItemId = "EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4";
			_sessionAuthenticationManager = new Mock<ISessionAuthenticationManager>();
            _compIntroLifeService = new Mock<ICompIntroLifeService>();
            _database = new Db();
		}

		[TearDown]
		public void TearDown() {
			// after each test
			_database.Dispose();
		}
		#endregion

		#region Tests
		[Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void Return_True_When_UserHasIntroLifeEligibility() {
			//Arrange
			var statusUser = StatusEnum.Hot;
            _sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
				.Returns(new AccountMembership {
					Mdsid = _mdsId,
					Status = statusUser,
					Profile = new Profile { Introlifeenddate = _introLifeEndDate }
				});

            _compIntroLifeService.Setup(x => x.GetCompIntroEligibility(_mdsId))
                .Returns(new CompIntroLifeEligibilityModel {
                    IntroEligible = true,
                    CompEligible = true
                });

            var compLifeMemberEligibilityManager = new CompLifeMemberEligibilityManager(
				_sessionAuthenticationManager.Object,
				_compIntroLifeService.Object
            );

			_database.Add(
				new DbItem("Intro Life", new ID(_globalItemId)) {
					{new ID(_fieldGlobalItemId), "I"}
				}
			);

			//Act
			var result =
				compLifeMemberEligibilityManager.GetResultEligibility(_globalItemId);
			//Assert
			Assert.IsTrue(result);
		}

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void Return_False_When_UserNoHasIntroLifeEligibility()
        {
            //Arrange
            var statusUser = StatusEnum.Hot;

            _sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
                .Returns(new AccountMembership
                {
                    Mdsid = _mdsId,
                    Status = statusUser,
                    Profile = new Profile { Introlifeenddate = _introLifeEndDate }
                });

            _compIntroLifeService.Setup(x => x.GetCompIntroEligibility(_mdsId))
                .Returns(new CompIntroLifeEligibilityModel
                {
                    IntroEligible = false,
                    CompEligible = false
                });

            var compLifeMemberEligibilityManager = new CompLifeMemberEligibilityManager(
                _sessionAuthenticationManager.Object,
                _compIntroLifeService.Object
            );

            _database.Add(
                new DbItem("Intro Life", new ID(_globalItemId)) {
                    {
                        new ID(_fieldGlobalItemId), "I"
                    }
                }
            );
            //Act
            var result = compLifeMemberEligibilityManager.GetResultEligibility(_globalItemId);
            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]

        public void Return_False_When_UserNoAuthenticated()
        {
            //Arrange
            var statusUser = StatusEnum.Cold;

            _sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
                .Returns(new AccountMembership
                {
                    Mdsid = _mdsId,
                    Status = statusUser,
                    Profile = new Profile { Introlifeenddate = _introLifeEndDate }
                });
            var compLifeMemberEligibilityManager =
                new CompLifeMemberEligibilityManager(
                    _sessionAuthenticationManager.Object,
                          _compIntroLifeService.Object);

            //Act
            var result =
                compLifeMemberEligibilityManager.GetResultEligibility(_globalItemId);
            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void Return_False_When_ThereIsNoItemSitecore()
        {
            //Arrange
            var statusUser = StatusEnum.Hot;
            var globalItemIdOther = "{EF9E66A9-E270-43EB-8AFB-9AC82995B426}";

            _sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
                .Returns(new AccountMembership
                {
                    Mdsid = _mdsId,
                    Status = statusUser,
                    Profile = new Profile { Introlifeenddate = _introLifeEndDate }
                });

            _compIntroLifeService.Setup(x => x.GetCompIntroEligibility(_mdsId))
                .Returns(new CompIntroLifeEligibilityModel
                {
                    IntroEligible = false,
                    CompEligible = false
                });

            var compLifeMemberEligibilityManager = new CompLifeMemberEligibilityManager(
                _sessionAuthenticationManager.Object,
                      _compIntroLifeService.Object
                  );

            _database.Add(new DbItem("Intro Life", new ID(_globalItemId)) {
                    {new ID(_fieldGlobalItemId), "I"}
                }
            );
            //Act
            var result = compLifeMemberEligibilityManager.GetResultEligibility(globalItemIdOther);
            //Assert
            Assert.IsFalse(result);
        }
        #endregion
    }
}
