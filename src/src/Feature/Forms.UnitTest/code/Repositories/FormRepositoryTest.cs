using System;
using Moq;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.CancelResetToken;
using Neambc.Neamb.Foundation.MBCData.Services.CreateResetToken;
using Neambc.Neamb.Foundation.MBCData.Services.ForgotUserName;
using Neambc.Neamb.Foundation.MBCData.Services.ValidateResetToken;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Foundation.WebServices;
using Neambc.UnitTesting.Base.Fakes;
using NUnit.Framework;
using SUT = Neambc.Seiumb.Feature.Forms.Repositories;
namespace Neambc.Seiumb.Feature.Forms.UnitTest.Repositories
{
    [TestFixture]
    public class FormRepositoryTest
    {
        #region Fields
        private Mock<IBase64Service> _base64ServiceMock;
        private IBase64Service _base64Service;
        private IAccountServiceProxy _accountServiceProxy;
        private Mock<IAccountServiceProxy> _accountServiceProxyMock;
        private IWebServicesConfiguration _webServicesConfiguration;
        private Mock<IWebServicesConfiguration> _webServicesConfigurationMock;
        private ICreateResetTokenService _createResetTokenService;
        private Mock<ICreateResetTokenService> _createResetTokenServiceMock;
        private IAccountManager _accountManager;
        private Mock<IAccountManager> _accountManagerMock;
        private ICancelResetTokenService _cancelResetTokenService;
        private Mock<ICancelResetTokenService> _cancelResetTokenServiceMock;
        private IForgotUserNameService _forgotUserNameService;
        private Mock<IForgotUserNameService> _forgotUserNameServiceMock;
        private IValidateResetTokenService _validateResetTokenService;
        private Mock<IValidateResetTokenService> _validateResetTokenServiceMock;
        private SUT.FormsRepository _sut;
        private FakeLog _log;
        #endregion

        #region Instrumentation

        [SetUp]
        public void SetUp()
        {
            _base64ServiceMock = new Mock<IBase64Service>();
            _base64Service = _base64ServiceMock.Object;
            _accountServiceProxyMock = new Mock<IAccountServiceProxy>();
            _accountServiceProxy = _accountServiceProxyMock.Object;
            _webServicesConfigurationMock = new Mock<IWebServicesConfiguration>();
            _webServicesConfiguration = _webServicesConfigurationMock.Object;
            _createResetTokenServiceMock = new Mock<ICreateResetTokenService>();
            _createResetTokenService = _createResetTokenServiceMock.Object;
            _accountManagerMock = new Mock<IAccountManager>();
            _accountManager = _accountManagerMock.Object;
            _cancelResetTokenServiceMock = new Mock<ICancelResetTokenService>();
            _cancelResetTokenService = _cancelResetTokenServiceMock.Object;
            _forgotUserNameServiceMock = new Mock<IForgotUserNameService>();
            _forgotUserNameService = _forgotUserNameServiceMock.Object;
            _validateResetTokenServiceMock = new Mock<IValidateResetTokenService>();
            _validateResetTokenService = _validateResetTokenServiceMock.Object;
            _log = new FakeLog();
            _webServicesConfigurationMock.Setup(x => x.UnionId).Returns("2");
            _sut = new SUT.FormsRepository(_base64Service,_accountServiceProxy,_webServicesConfiguration,_createResetTokenService,_accountManager, _forgotUserNameService,  _cancelResetTokenService, _validateResetTokenService);
            
        }

        #endregion
        [Test]
        public void CancelResetToken_InputWrongParameters() {
            Assert.Throws<ArgumentException>(() => _sut.CancelResetToken(null,null));
        }

        [Test]
        public void CancelResetToken_ReturnedCancelledFalse()
        {
            PasswordDisavowModel model = new PasswordDisavowModel();
            _sut.CancelResetToken("seiu.wayne@gmail.com", model);
            Assert.IsFalse(model.IsCanceled);
        }

        [Test]
        public void CancelResetToken_ReturnedCancelledTrue() {
            const string username = "seiu.wayne@gmail.com";
            PasswordDisavowModel model = new PasswordDisavowModel();
            _cancelResetTokenServiceMock.Setup(x => x.CancelResetToken(username, 2)).Returns(true);
            _sut.CancelResetToken(username, model);
            Assert.IsTrue(model.IsCanceled);
        }

        [Test]
        public void ValidateResetToken_InputWrongParameters() {
            Assert.Throws<ArgumentException>(() => _sut.ValidateResetToken(null, null,null));
        }

        [Test]
        public void ValidateResetToken_ReturnFalse()
        {
            ResetPasswordModel model = new ResetPasswordModel();
            _sut.ValidateResetToken("seiu.wayne@gmail.com","test token", model);
            Assert.IsFalse(model.IsUsernameValidToken);
        }

        [Test]
        public void ValidateResetToken_RuturnTrue()
        {
            const string username = "seiu.wayne@gmail.com";
            const string token = "token 123";
            ResetPasswordModel model = new ResetPasswordModel();
            _validateResetTokenServiceMock.Setup(x => x.ValidateResetToken(username, token, 2)).Returns(true);
            _sut.ValidateResetToken(username, token, model);
            Assert.IsTrue(model.IsUsernameValidToken);
        }
    }
}
