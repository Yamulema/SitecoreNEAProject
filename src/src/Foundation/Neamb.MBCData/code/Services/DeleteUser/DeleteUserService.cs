using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Seiumb.Foundation.Sitecore;
using Neambc.Neamb.Foundation.MBCData.Model.DeleteUser;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;

namespace Neambc.Neamb.Foundation.MBCData.Services.DeleteUser
{
    [Service(typeof(IDeleteUserService))]
    public class DeleteUserService : IDeleteUserService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public DeleteUserService(IAccessTokenService accessTokenService,IGlobalConfigurationManager config, IMBCRestfulService mbcRestfulService, ILog logService, IResponseHandler responseHandler)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService=mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }

        public DeleteUserResponse DeleteUserStatus(string username, int unionId)
        {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken))
            {
                throw new ArgumentException("token invalid", "token");
            }
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username is not found");
            }
            if (unionId < 0) throw new ArgumentException("invalid unionId (< 0)");

            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlDeleteUser,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new DeleteUserRequest
                {
                    username = username,
                    unionId = unionId
                },
                IsBasicAuthentication = false
            };

            var response = _mbcRestfulService.Post<DeleteUserResponse>(restRequestDto);
          
            if (response.Success)
            {
                if (response.Result != null && !response.Result.Success && response.Result.Error != null)
                {
                    _responseHandler.LogErrorResponse(response.Result.Error, "DeleteUserService", _logService);
                }
            }
            else
            {
                _logService.Error($"DeleteUser Post Error: {response.StatusCode}, {response.ExceptionDetail}", this);
            }

            return response.Result;
        }
    }
}
