using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.UpdateUserName;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;

namespace Neambc.Neamb.Foundation.MBCData.Services.UpdateUserName
{
    [Service(typeof(IUpdateUserNameService))]
    public class UpdateUserNameService : IUpdateUserNameService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;

        public UpdateUserNameService(IAccessTokenService accessTokenService,IGlobalConfigurationManager config, IMBCRestfulService mbcRestfulService)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService=mbcRestfulService;
        }

        public bool UpdateUserNameStatus(string currentUsername, string newUsername, string confirmNewUsername, string unionId)
        {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken))
            {
                throw new ArgumentException("token invalid", "token");
            }
            if (string.IsNullOrEmpty(currentUsername))
            {
                throw new ArgumentException($"Old user name is not found");
            }
            
            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlUpdateUserName,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new UpdateUserNameRequest
                {
                    CurrentUsername = currentUsername,
                    NewUsername = newUsername,
                    ConfirmNewUsername = confirmNewUsername,
                    UnionId = unionId
                },
                IsBasicAuthentication = false
            };
            var response = _mbcRestfulService.Post<UpdateUserNameResponse>(restRequestDto);
            return response.Success && response.Result?.Data != null && response.Result.Data.Updated;
           
        }
    }
}
