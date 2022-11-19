using System;
using Moq;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Managers {
    [TestFixture]
    public class LoginHandlerPostActionTest {
        #region Fields

        private Mock<ISessionManager> _sessionManagerMock;
        private ILoginHandlerPostAction _sut;

        #endregion

        #region Instrumentation

        [SetUp]
        public void SetUp() {
            _sessionManagerMock = new Mock<ISessionManager>();
            _sut = new LoginHandlerPostAction(_sessionManagerMock.Object);
        }

        #endregion

        [Test]
        public void VerifyExecutionPostAction_WhenNoDataInSession() {
            string currentRenderingComponentId = "";
            string componentIdQueryParameter = "";
            var result = _sut.VerifyExecutionPostAction(currentRenderingComponentId, componentIdQueryParameter, LoginHandlerEnum.Sweepstake);
            Assert.AreEqual(result, false);
        }

        [Test]
        public void VerifyExecutionPostAction_WhenDataExistsInSession() {
            string currentRenderingComponentId = "XYZ";
            string componentIdQueryParameter = "XYZ";
            string valueFromSession = "ABC";
            string keySweepstakes = ConstantsNeamb.ComponentIdSweepstakesCallAuthentication;
            _sessionManagerMock.Setup(x => x.RetrieveFromSession<string>(keySweepstakes)).Returns(valueFromSession);
            var result = _sut.VerifyExecutionPostAction(currentRenderingComponentId, componentIdQueryParameter, LoginHandlerEnum.Sweepstake);
            Assert.AreEqual(result, true);
        }

        [Test]
        public void GetPageToRedirection_WithEmptyData() {
            var result = _sut.GetPageToRedirection("");
            Assert.AreEqual(result, "");
        }

        [Test]
        public void GetPageToRedirection_WithNoFeature() {
            string loginUrl = "http://neamb.local/login";
            var result = _sut.GetPageToRedirection(loginUrl);
            Assert.AreEqual(result, $"{loginUrl}");
        }

        [Test]
        public void GetPageToRedirection_WithSessionDataForSweepstakes() {
            string loginUrl = "http://neamb.local/login";
            LoginHandlerEnum loginHandlerEnum = LoginHandlerEnum.Sweepstake;
            string keyFeature = ConstantsNeamb.LoginHandlerFeatureType;
            _sessionManagerMock.Setup(x => x.RetrieveFromSession<LoginHandlerEnum>(keyFeature)).Returns(loginHandlerEnum);

            var result = _sut.GetPageToRedirection(loginUrl);
            Assert.AreEqual(result, $"{loginUrl}");
        }

        [Test]
        public void GetPageToRedirection_WithSessionDataForSweepstakesWithComponentId() {
            string loginUrl = "http://neamb.local/login";
            LoginHandlerEnum loginHandlerEnum = LoginHandlerEnum.Sweepstake;
            string keyFeature = ConstantsNeamb.LoginHandlerFeatureType;
            _sessionManagerMock.Setup(x => x.RetrieveFromSession<LoginHandlerEnum>(keyFeature)).Returns(loginHandlerEnum);
            string valueFromSession = "ABC";
            string keySweepstakes = ConstantsNeamb.ComponentIdSweepstakesCallAuthentication;
            _sessionManagerMock.Setup(x => x.RetrieveFromSession<string>(keySweepstakes)).Returns(valueFromSession);

            var result = _sut.GetPageToRedirection(loginUrl);
            Assert.AreEqual(result, $"{loginUrl}?componentId={valueFromSession}");
        }

        [Test]
        public void GetPageToRedirection_WithSessionDataForSweepstakesWithComponentIdAndQueryString() {
            string loginUrl = "http://neamb.local/login?utm_source=TWS";
            LoginHandlerEnum loginHandlerEnum = LoginHandlerEnum.Sweepstake;
            string keyFeature = ConstantsNeamb.LoginHandlerFeatureType;
            _sessionManagerMock.Setup(x => x.RetrieveFromSession<LoginHandlerEnum>(keyFeature)).Returns(loginHandlerEnum);
            string valueFromSession = "ABC";
            string keySweepstakes = ConstantsNeamb.ComponentIdSweepstakesCallAuthentication;
            _sessionManagerMock.Setup(x => x.RetrieveFromSession<string>(keySweepstakes)).Returns(valueFromSession);

            var result = _sut.GetPageToRedirection(loginUrl);
            Assert.AreEqual(result, $"{loginUrl}&componentId={valueFromSession}");
        }

        [Test]
        public void GetPageToRedirection_WithKeyEmpty() {
            string loginUrl = "http://neamb.local/login";
            var result = _sut.GetPageToRedirection(loginUrl);
            Assert.AreEqual(result, loginUrl);
        }

        [Test]
        public void SaveTrackingPageToRedirection_WithEmptyParameters() {
            Assert.Throws<ArgumentException>(() => _sut.SaveTrackingPageToRedirection(LoginHandlerEnum.None, null));
        }

        [Test]
        public void SaveTrackingPageToRedirection_WithNoEmptyParameters()
        {
            _sut.SaveTrackingPageToRedirection(LoginHandlerEnum.Sweepstake,"ABC");
        }
    }
}
