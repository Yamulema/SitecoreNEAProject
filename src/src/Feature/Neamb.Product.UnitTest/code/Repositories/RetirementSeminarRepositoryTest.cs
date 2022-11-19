using System;
using System.Collections.Generic;
using Moq;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Feature.Product.Repositories;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;

namespace Neambc.Neamb.Feature.Product.UnitTest.Repositories
{
    [TestFixture]
    public class RetirementSeminarRepositoryTest {
        #region Fields

        private Mock<ISessionAuthenticationManager> _sessionAuthenticationManager;
        private Mock<IOracleDatabase> _oracleDatabase;
        private Mock<ISeminarRepository> _seminaryRepository;
        private Mock<ISessionManager> _sessionManager;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManager;
        private Mock<IExactTargetClient> _exactTargetClient;
        private IRetirementSeminarRepository _sut;
        private Mock<IGtmService> _gtmService;
        private Mock<ILoginHandlerPostAction> _loginHandlerPostAction;

        #endregion

        #region Instrumentation

        [SetUp]
        public void Setup() {
            _sessionAuthenticationManager = new Mock<ISessionAuthenticationManager>();
            _oracleDatabase = new Mock<IOracleDatabase>();
            _seminaryRepository = new Mock<ISeminarRepository>();
            _sessionManager = new Mock<ISessionManager>();
            _globalConfigurationManager = new Mock<IGlobalConfigurationManager>();
            _exactTargetClient = new Mock<IExactTargetClient>();
            _gtmService = new Mock<IGtmService>();
            _loginHandlerPostAction = new Mock<ILoginHandlerPostAction>();
            _sut = new RetirementSeminarRepository(
                _sessionAuthenticationManager.Object,
                _oracleDatabase.Object,
                _seminaryRepository.Object,
                _globalConfigurationManager.Object,
                _exactTargetClient.Object,_gtmService.Object, _loginHandlerPostAction.Object);

        }

        #endregion

        #region Test

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void SetPropertiesRetirementSeminar_When_Model_Not_Initialized() {
            RetirementSeminarDTO retirementSeminarDto = new RetirementSeminarDTO();

            using (var db = new Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            }) {
                _sut.SetPropertiesRetirementSeminar(ref retirementSeminarDto, db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")),new Guid(),"");
                Assert.AreEqual(retirementSeminarDto.SweepstakesBase, null);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void SetPropertiesRetirementSeminar_When_No_Seminaries() {
            RetirementSeminarDTO retirementSeminarDto = new RetirementSeminarDTO();
            retirementSeminarDto.SweepstakesBase = new SweepstakesBaseDTO();

            using (var db = new Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            }) {
                AccountMembership accountMembership = new AccountMembership();
                _sessionAuthenticationManager.Setup(item => item.GetAccountMembership()).Returns(accountMembership);
                _sut.SetPropertiesRetirementSeminar(ref retirementSeminarDto, db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")), new Guid(), "");
                Assert.AreEqual(retirementSeminarDto.SeminarId, null);
                Assert.AreEqual(retirementSeminarDto.IsMember, false);
                Assert.AreEqual(retirementSeminarDto.IsValidSeminary, false);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void SetPropertiesRetirementSeminar_With_Seminaries_Registered() {
            const string indvId = "995";
            const string seminaryRegisteredId = "1";
            RetirementSeminarDTO retirementSeminarDto = new RetirementSeminarDTO();
            retirementSeminarDto.SweepstakesBase = new SweepstakesBaseDTO();

            using (var db = new Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            }) {
                Item renderingItem = db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"));
                AccountMembership accountMembership = new AccountMembership();
                _sessionAuthenticationManager.Setup(item => item.GetAccountMembership()).Returns(accountMembership);
                List<ViewSeminarReg> seminarRegistered = new List<ViewSeminarReg>();
                seminarRegistered.Add(new ViewSeminarReg {
                    IndvId = indvId,
                    SeminarId = seminaryRegisteredId
                });
                seminarRegistered.Add(new ViewSeminarReg {
                    IndvId = indvId,
                    SeminarId = "2"
                });
                _oracleDatabase.Setup(x => x.ViewAllSeminarReg()).Returns(seminarRegistered);
                _sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
                    .Returns(new AccountMembership {
                        Mdsid = indvId
                    });
                _seminaryRepository.Setup(x => x.GetSeminarId(renderingItem)).Returns(seminaryRegisteredId);
                _sut.SetPropertiesRetirementSeminar(ref retirementSeminarDto, renderingItem, new Guid(), "");
                Assert.AreEqual(retirementSeminarDto.AlreadyRegistered, true);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void SetPropertiesRetirementSeminar_With_No_Seminaries_Registered() {
            const string indvId = "995";
            const string seminaryRegisteredId = "1";
            RetirementSeminarDTO retirementSeminarDto = new RetirementSeminarDTO();
            retirementSeminarDto.SweepstakesBase = new SweepstakesBaseDTO();

            using (var db = new Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            }) {
                Item renderingItem = db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}"));
                AccountMembership accountMembership = new AccountMembership();
                _sessionAuthenticationManager.Setup(item => item.GetAccountMembership()).Returns(accountMembership);
                List<ViewSeminarReg> seminarRegistered = new List<ViewSeminarReg>();
                seminarRegistered.Add(new ViewSeminarReg {
                    IndvId = indvId,
                    SeminarId = "3"
                });
                seminarRegistered.Add(new ViewSeminarReg {
                    IndvId = indvId,
                    SeminarId = "2"
                });
                _oracleDatabase.Setup(x => x.ViewAllSeminarReg()).Returns(seminarRegistered);
                _sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
                    .Returns(new AccountMembership {
                        Mdsid = indvId
                    });
                _seminaryRepository.Setup(x => x.GetSeminarId(renderingItem)).Returns(seminaryRegisteredId);
                _sut.SetPropertiesRetirementSeminar(ref retirementSeminarDto, renderingItem, new Guid(), "");
                Assert.AreEqual(retirementSeminarDto.AlreadyRegistered, false);
            }
        }
        
        [Test]
        public void ExecuteRegistrationRetirementSeminar_When_NoContextItem() {
            var response = _sut.ExecuteRegistrationRetirementSeminar(new RetirementSeminarDTO());
            Assert.AreEqual(response.HasError, true);
            Assert.AreEqual(response.ProcessedSucessfully, false);
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ExecuteRegistrationRetirementSeminar_When_User_Is_Cold()
        {
            const string indvId = "995";
            using (var db = new Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            })
            {
                _sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
                    .Returns(new AccountMembership
                    {
                        Status = StatusEnum.Cold,
                        Mdsid = indvId
                    });
                var response = _sut.ExecuteRegistrationRetirementSeminar(new RetirementSeminarDTO { ContextItem = "{5EA33232-AC25-42E5-A550-6C9232F318ED}" });
                Assert.AreEqual(response.ErrorAuthentication, true);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ExecuteRegistrationRetirementSeminar_When_User_Authenticated()
        {
            const string indvId = "995";
            ViewSeminar viewSeminar= new ViewSeminar {
                Address = "address",
                City = "city",
                LeaCode = "leacode",
                LeaName = "leaname",
                SeaName = "seaname"
            };

            using (var db = new Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            }) {
                _sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
                    .Returns(new AccountMembership {
                        Status = StatusEnum.Hot,
                        Mdsid = indvId
                    });
                _seminaryRepository.Setup(x => x.GetSeminary(db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")))).Returns(viewSeminar);
                var response = _sut.ExecuteRegistrationRetirementSeminar(new RetirementSeminarDTO{ContextItem = "{5EA33232-AC25-42E5-A550-6C9232F318ED}" });
                Assert.AreEqual(response.HasError, true);
                Assert.AreEqual(response.ProcessedSucessfully, false);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void ExecuteRegistrationRetirementSeminar_When_User_Authenticated_Process_Success()
        {
            const string indvId = "995";
            const string leaCode = "lea";
            ViewSeminar viewSeminar = new ViewSeminar
            {
                Address = "address",
                City = "city",
                LeaCode = "leacode",
                LeaName = "leaname",
                SeaName = "seaname"
            };
            using (var db = new Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) { },
            })
            {
                _sessionAuthenticationManager.Setup(x => x.GetAccountMembership())
                    .Returns(new AccountMembership
                    {
                        Status = StatusEnum.Hot,
                        Mdsid = indvId
                    });
                _seminaryRepository.Setup(x => x.GetLeaCode(db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")))).Returns(leaCode);
                _oracleDatabase.Setup(x => x.InsertInSeminar(It.IsAny<int>(), leaCode)).Returns(true);
                _exactTargetClient.Setup(x =>
                        x.SendExactTargetService(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<KeyValuePair<string, string>>>(), It.IsAny<string>()))
                    .Returns(true);
                _seminaryRepository.Setup(x => x.GetSeminary(db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")))).Returns(viewSeminar);
                var response = _sut.ExecuteRegistrationRetirementSeminar(new RetirementSeminarDTO { ContextItem = "{5EA33232-AC25-42E5-A550-6C9232F318ED}" });
                Assert.AreEqual(response.HasError, false);
                Assert.AreEqual(response.ProcessedSucessfully, true);
            }
        }

        #endregion

    }

}

