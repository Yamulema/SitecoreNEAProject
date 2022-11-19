using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Model.PasswordManagement;
using Neambc.Neamb.Foundation.MBCData.Model.PasswordManagement.ResetPassword;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.ResetPassword
{
    [Service(typeof(IResetPasswordService))]
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public ResetPasswordService(IAccessTokenService accessTokenService,
            IGlobalConfigurationManager config,
            IMBCRestfulService mbcRestfulService, ILog logService, IResponseHandler responseHandler) {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }

        public ResetPasswordMgmtResponse ResetPassword(
            string username,
            string newPassword,
            string confirmNewPassword,
            int unionId
        ) {
            if (unionId < 0) throw new ArgumentException("Parameters for ResetPasswordService invalid - invalid unionId (< 0)", "unionId");
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Parameters for ResetPasswordService invalid - username is empty", "username");
            if (string.IsNullOrEmpty(newPassword)) throw new ArgumentException(" Parameters for ResetPasswordService invalid - new password is empty", "newPassword");
            if (string.IsNullOrEmpty(confirmNewPassword)) throw new ArgumentException(" Parameters for ResetPasswordService invalid - confirm new password is empty", "confirmNewPassword");

            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken)) throw new ArgumentException("token invalid", "token");

            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlResetPassword,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new PasswordMgmtRequest
                {
                    Username = username,
                    NewPassword = newPassword,
                    ConfirmNewPassword = confirmNewPassword,
                    UnionId = unionId
                },
                IsBasicAuthentication = false
            };

            _logService.Info("Starting calling ResetPasswordService - ResetPassword with params:", this);
            _logService.Info($"Username: {username}", this);
            _logService.Info($"NewPassword: {newPassword}", this);
            _logService.Info($"ConfirmNewPassword: {confirmNewPassword}", this);
            _logService.Info($"UnionId: {unionId}", this);

            var response = _mbcRestfulService.Post<ResetPasswordMgmtResponse>(restRequestDto);
            if (response.Success)
            {
                if (response.Result?.Data != null && response.Result.Success)
                    return response.Result;

                response.Result.ErrorCodeResponse = (UserAccountErrorCodesEnum)response.Result.Error.Code;
                _responseHandler.LogErrorResponse(response.Result.Error, "ResetPasswordService", _logService);
            }
            else
            {
                _logService.Error($"Post Error to ResetPasswordService: {response.StatusCode}, {response.ExceptionDetail}", this);
            }

            return response.Result;
        }
    }
}