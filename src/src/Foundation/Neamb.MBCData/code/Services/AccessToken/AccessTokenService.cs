using System;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.AccessToken;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;

namespace Neambc.Neamb.Foundation.MBCData.Services.AccessToken
{
    [Service(typeof(IAccessTokenService))]
    public class AccessTokenService : IAccessTokenService
    {
        private readonly IAccessTokenRestRepository _accessTokenRestRepository;
        private readonly IGlobalConfigurationManager _config;
        private readonly ICacheManager _cacheManager;
        private readonly string _accessTokenKey = "AccessToken";
        
        public AccessTokenService(IGlobalConfigurationManager globalConfigurationManager,
            ICacheManager cacheManager, IAccessTokenRestRepository accessTokenRestRepository
            )
        {
            _config = globalConfigurationManager;
            _cacheManager = cacheManager;
            _accessTokenRestRepository = accessTokenRestRepository;
            
        }

        public TokenResponse GetAccessTokenFromRedis() {
            var accessTokenKey = $"{_accessTokenKey}:{_config.RestUserAuthentication}";
            var isTokenInCache = _cacheManager.ExistInCache(accessTokenKey);

            if (isTokenInCache) {
                var tokenCached = _cacheManager.RetrieveFromCache<TokenResponse>(accessTokenKey);
                if (IsTokenValidInRedis(tokenCached)) {
                    return tokenCached;
                }
            }
            return GetAccessTokenFromServerAndSaveRedis();
        }

        public TokenResponse GetAccessTokenFromServerAndSaveRedis() {
            var accessTokenKey = $"{_accessTokenKey}:{ _config.RestUserAuthentication}";

            var model = new TokenRequest
            {
                Username = _config.RestUserAuthentication,
                Password = _config.RestPasswordAuthentication
            };

            var restRequestDto = new RestRequestDto
            {
                Server = _config.RestUrl,
                Action = "/token-management/v1/access-token",
                Body = model,
                IsBasicAuthentication = true,
                ParseJson = true
            };
            var response = _accessTokenRestRepository.Post(restRequestDto);

            //Save in redis
            if (response.Success)
            {
                var token = response.Result;
                using (_cacheManager.AcquireLock(accessTokenKey))
                {
                    token.Data.ExpiresAtInRedis = GetRedisTokenLifetime(token.Data.ExpiresAt);
                    _cacheManager.StoreInCache<TokenResponse>(accessTokenKey, token, token.Data.ExpiresAtInRedis);
                }
                return token;
            } else {
                return new TokenResponse();
            }
        }

        private bool IsTokenValidInRedis(TokenResponse token)
        {
            //TODO: Review token invalidation
            return token.Data.ExpiresAtInRedis > DateTime.Now;
        }

        private DateTime GetRedisTokenLifetime(long epoch)
        {
            var epochBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var dateTime = epochBase.AddSeconds(epoch);
            return dateTime.AddMinutes(_config.LifetimeAccessTokenMinutes);
        }
    }
}