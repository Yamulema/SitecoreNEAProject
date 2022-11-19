using System.Collections.Generic;
using Moq;
using Neambc.Neamb.Feature.Product.Interfaces;
using Neambc.Neamb.Feature.Product.Repositories;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Neambc.Neamb.Feature.Product.UnitTest.Repositories
{
    [TestFixture]
    public class SweepstakeRepositoryTest
    {
        #region Fields

        private Mock<ISessionAuthenticationManager> _sessionAuthenticationManager;
        private Mock<IEligibilityManager> _eligibilityManager;
        private Mock<IReminderService> _reminderService;
        private Mock<IGlobalConfigurationManager> _globalConfigurationManager;
        private Mock<IExactTargetClient> _exactTargetClient;
        private ISweepstakesRepository _sut;
        private Mock<IGtmService> _gtmService;

        #endregion

        #region Instrumentation

        [SetUp]
        public void Setup() {
            _sessionAuthenticationManager = new Mock<ISessionAuthenticationManager>();
            _eligibilityManager = new Mock<IEligibilityManager>();
            _reminderService = new Mock<IReminderService>();
            _globalConfigurationManager = new Mock<IGlobalConfigurationManager>();
            _exactTargetClient = new Mock<IExactTargetClient>();
            _gtmService = new Mock<IGtmService>();
            _sut = new SweepstakesRepository(
                _sessionAuthenticationManager.Object,_eligibilityManager.Object,_reminderService.Object,
                _gtmService.Object,
                _globalConfigurationManager.Object,
                _exactTargetClient.Object);

        }

        #endregion

        #region Test

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void No_SendEmail_When_Flag_Unchecked() {
            bool expectedResult = true;
            AccountMembership accountMembership = new AccountMembership {
                Mdsid = "995",
                Username = "nea.owen@gmail.com"
            };

            using (var db = new Sitecore.FakeDb.Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) {
                    Fields = {
                        new DbField("SendEmailNotification", new ID("{FDF37AEA-86E1-4EAE-A546-7EA6E6EA8AD1}")) {
                            Value = "0"
                        }
                    }
                },
            }) {
               var resultSendEmailSweepstakes= _sut.SendEmail(db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")), accountMembership);
               Assert.AreEqual(resultSendEmailSweepstakes, expectedResult);
            }
        }

        [Test]
        [Ignore("Error in Fakedb to be fixed")]
        public void No_SendEmail_When_Flag_Checked()
        {
            bool expectedResult = true;
            string capturedArg1;
            string capturedArg2="";
                List< KeyValuePair<string, string> > capturedArg3;
            string capturedArg4="";
            AccountMembership accountMembership = new AccountMembership
            {
                Mdsid = "995",
                Username = "nea.owen@gmail.com"
            };

            using (var db = new Sitecore.FakeDb.Db {
                new DbItem("renderingItem", new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")) {
                    Fields = {
                        new DbField("SendEmailNotification", new ID("{FDF37AEA-86E1-4EAE-A546-7EA6E6EA8AD1}")) {
                            Value = "1"
                        }
                    }
                },
            }) {
                string mdsid = accountMembership.Mdsid.PadLeft(9, '0');
                _exactTargetClient
                    .Setup(x => x.SendExactTargetService(It.IsAny<string>(), accountMembership.Username, It.IsAny<List<KeyValuePair<string, string>>>(),mdsid ))
                    .Callback((string customerDefinition,
                        string username,
                        List<KeyValuePair<string, string>> exactTargetParameters,
                        string subscriberKey) => {
                        capturedArg1 = customerDefinition;
                        capturedArg2 = username;
                        capturedArg3 = exactTargetParameters;
                        capturedArg4 = subscriberKey;
                        })
                    .Returns(expectedResult);
                var resultSendEmailSweepstakes = _sut.SendEmail(db.GetItem(new ID("{5EA33232-AC25-42E5-A550-6C9232F318ED}")), accountMembership);
                Assert.AreEqual(capturedArg2, accountMembership.Username);
                Assert.AreEqual(capturedArg4, mdsid);
                Assert.AreEqual(resultSendEmailSweepstakes, expectedResult);
            }
        }

        #endregion

    }

}

