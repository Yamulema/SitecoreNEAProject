using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Model.ForgotUserName;
using Neambc.Neamb.Foundation.MBCData.Services.ResponseHandler;
using Neambc.Seiumb.Foundation.Sitecore;

namespace Neambc.Neamb.Foundation.MBCData.Services.ForgotUserName
{
    [Service(typeof(IForgotUserNameService))]
    public class ForgotUserNameService : IForgotUserNameService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;
        private readonly ILog _logService;
        private readonly IResponseHandler _responseHandler;

        public ForgotUserNameService(IAccessTokenService accessTokenService,IGlobalConfigurationManager config, IMBCRestfulService mbcRestfulService, ILog logService, IResponseHandler responseHandler)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService=mbcRestfulService;
            _logService = logService;
            _responseHandler = responseHandler;
        }

        public ForgotUserNameResponse ForgotUserNameStatus(string firstName, string lastName, string zipCode, string dob, int unionId)
        {
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken))
            {
                throw new ArgumentException("token invalid", "token");
            }
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException($"First name is not found");
            }
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException($"Last name is not found");
            }
            if (string.IsNullOrEmpty(dob))
            {
                throw new ArgumentException($"dob is not found");
            }
            if (string.IsNullOrEmpty(zipCode))
            {
                throw new ArgumentException($"zipCpde is not found");
            }
            if (unionId < 0) throw new ArgumentException($"invalid unionId (< 0)");

            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlForgotUserName,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new ForgotUserNameRequest
                {
                    FirstName = firstName,
                    LastName = lastName,
                    ZipCode = zipCode,
                    Dob = dob,
                    UnionId = unionId
                },
                IsBasicAuthentication = false
            };

            var response = _mbcRestfulService.Post<ForgotUserNameResponse>(restRequestDto);
          
            if (response.Success)
            {
                if (response.Result != null && !response.Result.Success && response.Result.Error != null)
                {
                    _responseHandler.LogErrorResponse(response.Result.Error, "ForgotUserNameService", _logService);
                }
            }
            else
            {
                _logService.Error($"ForgotUserName Post Error: {response.StatusCode}, {response.ExceptionDetail}", this);
            }

            return response.Result;
        }
    }
}
