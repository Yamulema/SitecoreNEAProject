using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.PasswordManagement.UpdatePassword;
using Neambc.Neamb.Foundation.MBCData.Services.ResetPassword;
using Neambc.Neamb.Foundation.MBCData.Services.UpdatePassword;
using Neambc.Neamb.Foundation.MBCData.Services.UpdateUserStatus;
using Neambc.Neamb.Foundation.Membership.Interfaces;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
    [Service(typeof(IAccountManager))]
    public class AccountManager : IAccountManager
    {
        private readonly IUpdatePasswordService _updatePasswordService;
        private readonly IResetPasswordService _resetPasswordService;
        private readonly IUpdateUserStatusService _updateUserStatusService;

        public AccountManager(IUpdatePasswordService updatePasswordService,
            IResetPasswordService resetPasswordService, IUpdateUserStatusService updateUserStatusService) {
            _updatePasswordService = updatePasswordService;
            _resetPasswordService = resetPasswordService;
            _updateUserStatusService = updateUserStatusService;
        }

        public UpdatePasswordMgmtResponse UpdatePassword(string username, string currentPassword, string newPassword, int unionId)
        {
            var updatePasswordResponse = _updatePasswordService.UpdatePassword(username, currentPassword, newPassword, newPassword, unionId);
            return updatePasswordResponse;
        }

        public bool ResetPassword(string username, string newPassword, string confirmNewPassword, int unionId)
        {
            var resetPasswordResponse = _resetPasswordService.ResetPassword(username, newPassword, confirmNewPassword, unionId);
            return resetPasswordResponse.Success && (resetPasswordResponse.Data?.Reset ?? false);
        }

        public bool UpdateUserStatus(string email, int statusCode, int unionId) {
            var statusCodeValue = statusCode.ToString();
            return _updateUserStatusService.UpdateUserStatus(email, statusCodeValue, unionId);
        }
    }
}