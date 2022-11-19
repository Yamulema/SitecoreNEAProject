using System;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.CompIntroLife;
using Neambc.Neamb.Foundation.MBCData.Model.ProductEligibility;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Neamb.Foundation.MBCData.Services.AccessToken;

namespace Neambc.Neamb.Foundation.MBCData.Services.CompIntroLife
{
    [Service(typeof(ICompIntroLifeService))]
    public class CompIntroLifeService : ICompIntroLifeService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IGlobalConfigurationManager _config;
        private readonly IMBCRestfulService _mbcRestfulService;

        public CompIntroLifeService(IAccessTokenService accessTokenService,
            IGlobalConfigurationManager config,
            IMBCRestfulService mbcRestfulService)
        {
            _accessTokenService = accessTokenService;
            _config = config;
            _mbcRestfulService = mbcRestfulService;
        }

        public CompIntroLifeEligibilityModel GetCompIntroEligibility(string mdsId)
        {
            int.TryParse(mdsId, out int mdsidInt);
            if (mdsidInt < 0) throw new ArgumentException("invalid mdsid (< 0)", "mdsId");
            var token = _accessTokenService.GetAccessTokenFromRedis();
            if (token?.Data == null || string.IsNullOrEmpty(token.Data.AccessToken)) {
                throw new ArgumentException("token invalid", "token");
            }

            var restRequestDto = new RestRequestDto()
            {
                Server = _config.RestUrl,
                Action = _config.RestUrlCompIntroEligibility,
                Body = new ProductEligibilityBaseRequest { MdsId = mdsidInt },
                Token = token.Data.AccessToken
            };

            var response = _mbcRestfulService.Post<CompIntroLifeEligibilityResponse>(restRequestDto);

            return response.Success ?
                response.Result.Data :
                new CompIntroLifeEligibilityModel { CompEligible = false, IntroEligible = false };

        }
    }
}