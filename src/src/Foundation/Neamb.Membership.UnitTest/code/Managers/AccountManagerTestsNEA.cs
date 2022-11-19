using Moq;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model.PasswordManagement.ResetPassword;
using Neambc.Neamb.Foundation.MBCData.Model.PasswordManagement.UpdatePassword;
using Neambc.Neamb.Foundation.MBCData.Services.ResetPassword;
using Neambc.Neamb.Foundation.MBCData.Services.UpdatePassword;
using Neambc.Neamb.Foundation.MBCData.Services.UpdateUserStatus;
using Neambc.Neamb.Foundation.Membership.Managers;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Neambc.Neamb.Foundation.Membership.UnitTest.Managers
{
    [TestFixture]
    public class AccountManagerTestsNEA
    {
        #region Fields
        private Mock<IUpdatePasswordService> _updatePasswordServiceMock;
        private Mock<IResetPasswordService> _resetPasswordServiceMock;
        private Mock<IUpdateUserStatusService> _updateUserStatusServiceMock;
        
        private IUpdatePasswordService _updatePasswordService;
        private IResetPasswordService _resetPasswordService;
        private IUpdateUserStatusService _updateUserStatusService;
        private AccountManager _sut;
        #endregion

        [SetUp]
        public void SetUp()
        {
            _updatePasswordServiceMock = new Mock<IUpdatePasswordService>();
            _resetPasswordServiceMock = new Mock<IResetPasswordService>();
            _updateUserStatusServiceMock = new Mock<IUpdateUserStatusService>();
            _updatePasswordService = _updatePasswordServiceMock.Object;
            _resetPasswordService = _resetPasswordServiceMock.Object;
            _updateUserStatusService = _updateUserStatusServiceMock.Object;
            _sut = new AccountManager(_updatePasswordService, _resetPasswordService, _updateUserStatusService);
        }

        [Test]
        public void UpdatePasswordWhenReturnOk()
        {
            var updatePwdResponseOk = new UpdatePasswordMgmtResponse
            {
               Data = new UpdatePasswordModel {
                   Updated = true
               },
               Success = true
            };

            _updatePasswordServiceMock.Setup(x => x.UpdatePassword(
                    It.IsAny<string>(), It.IsAny<string>(), 
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(updatePwdResponseOk);

            var result = _sut.UpdatePassword("test", "test", "test", (int)Union.NEA);

            Assert.IsNotNull(result.Data);
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.Data.Updated, true);
        }

        [Test]
        public void UpdatePasswordWhenReturnError()
        {
            var updatePwdResponseOk = new UpdatePasswordMgmtResponse
            {
                Data = null,
                Success = false,
                ErrorCodeResponse = UserAccountErrorCodesEnum.UsernamePasswordCombinationNoMatch
            };

            _updatePasswordServiceMock.Setup(x => x.UpdatePassword(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(updatePwdResponseOk);

            var result = _sut.UpdatePassword("test", "test", "test", (int)Union.NEA);

            Assert.IsNull(result.Data);
            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.ErrorCodeResponse, UserAccountErrorCodesEnum.UsernamePasswordCombinationNoMatch);
        }


        [Test]
        public void ResetPasswordWhenReturnOk()
        {
            var resetPwdResponseOk = new ResetPasswordMgmtResponse
            {
                Data = new ResetPasswordModel
                {
                    Reset = true
                },
                Success = true
            };

            _resetPasswordServiceMock.Setup(x => x.ResetPassword(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<int>()))
                .Returns(resetPwdResponseOk);

            var result = _sut.ResetPassword("test", "test", "test", (int)Union.NEA);
            Assert.AreEqual(result, true);
        }

        [Test]
        public void ResetPasswordWhenReturnError()
        {
            var resetPwdResponseOk = new ResetPasswordMgmtResponse
            {
                Data = null,
                Success = false
            };

            _resetPasswordServiceMock.Setup(x => x.ResetPassword(
                    It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<int>()))
                .Returns(resetPwdResponseOk);

            var result = _sut.ResetPassword("test", "test", "test", (int)Union.NEA);
            Assert.AreEqual(result, false);
        }

        [Test]
        public void UpdateUserStatusWhenReturnOk()
        {
            _updateUserStatusServiceMock.Setup(x => x.UpdateUserStatus(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);

            var result = _sut.UpdateUserStatus("test", (int)UserStatus.Default, (int)Union.NEA);

            Assert.IsNotNull(result);
            Assert.AreEqual(result, true);
        }

        [Test]
        public void UpdateUserStatusWhenReturnError()
        {
            _updateUserStatusServiceMock.Setup(x => x.UpdateUserStatus(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(false);

            var result = _sut.UpdateUserStatus("test", (int)UserStatus.Default, (int)Union.NEA);

            Assert.IsNotNull(result);
            Assert.AreEqual(result, false);
        }
    }
}
