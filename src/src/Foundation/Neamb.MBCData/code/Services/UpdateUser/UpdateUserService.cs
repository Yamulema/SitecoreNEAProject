using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.UpdateUser;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;

namespace Neambc.Neamb.Foundation.MBCData.Services.UpdateUser
{
    [Service(typeof(IUpdateUserService))]
    public class UpdateUserService : IUpdateUserService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;


        public UpdateUserService(IAccessTokenService accessTokenService,IGlobalConfigurationManager config, IMBCRestfulService mbcRestfulService)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService=mbcRestfulService;
        }

        /// <summary>
        /// update user status
        /// </summary> 
        /// <returns>
        /// </returns>
        public UpdateUserResponse UpdateUser(string username, int webuserId, string firstname, string lastname, string streetaddress, string city, string statecode, string zipcode, string dob, string phone, string permissionIndicator, int unionId)
        {
            if (unionId < 0) throw new ArgumentException("invalid unionId (< 0)", "unionId");
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("username is empty", "username");
            if (string.IsNullOrEmpty(firstname)) throw new ArgumentException("firstname is empty", "firstname");

        var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken)) throw new ArgumentException("token invalid", "token");
            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlUpdateUser,
                ParseJson = true,
                Token = token.Data.AccessToken,
                Body = new UpdateUserRequest
                {
                    username = username,
                    webUserId = webuserId,
                    FirstName =firstname,
                    LastName = lastname,
                    StreetAddress = streetaddress,
                    City = city,
                    StateCode = statecode,
                    ZipCode = zipcode,
                    Dob = dob,
                    Phone = phone,
                    PermissionIndicator = permissionIndicator,
                    UnionId = unionId },
                IsBasicAuthentication = false
            };

            var response = _mbcRestfulService.Post<UpdateUserResponse>(restRequestDto);
            return response.Result;

        }
    }
}
