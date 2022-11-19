using System.Collections.Generic;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Seiumb.Foundation.WebServices;

namespace Neambc.Neamb.Foundation.MBCData.Services.Rakuten
{
    [Service(typeof(IRakutenMemberService))]
    public class RakutenMemberService : IRakutenMemberService
    {
        private readonly IRakutenMemberRestRepository _rakutenRestRepository;
        private readonly IGlobalConfigurationManager _config;
        private readonly IWebServicesConfiguration _webServicesConfiguration;

        public RakutenMemberService(IRakutenMemberRestRepository rakutenRestRepository, IGlobalConfigurationManager globalConfigurationManager, IWebServicesConfiguration webServicesConfiguration)
        {
            _rakutenRestRepository = rakutenRestRepository;
            _config = globalConfigurationManager;
            _webServicesConfiguration = webServicesConfiguration;
        }

        public RestResultDto<MemberCreationResponse> CreateMember(bool isNEA, string email, string mdsId, string cellCode)
        {
            List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>();
            headers.Add(new KeyValuePair<string, string>("Locale", _config.RakutenLocale));
            headers.Add(new KeyValuePair<string, string>("Signature", 
                isNEA ? _config.RakutenSignature:_config.RakutenSEIUSignature));

            var restRequestDto = new RakutenRestRequestDto(headers)
            {
                Server = isNEA ? _config.RestUrlRakutenMemberCreation : _config.RestUrlRakutenMemberCreationSEIU,
                Body = new MemberCreationRequest
                {
                    EmailAddress = email,
                    MemberSource = isNEA? _config.RakutenMemberSource : _config.RakutenSEIUMemberSource,
                    BonusId = "",
                    TriggerpasswordEmail = true
                },
                MdsId = mdsId,
                CellCode = cellCode,
                UnionId = isNEA ? _config.Unionid: _webServicesConfiguration.UnionId,
                ParseJson = true
            };

            return _rakutenRestRepository.Post(restRequestDto);
        }
    }
}