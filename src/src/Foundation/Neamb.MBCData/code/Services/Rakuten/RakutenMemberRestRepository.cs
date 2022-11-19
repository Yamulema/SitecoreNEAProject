using System;
using System.Linq;
using System.Net;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;
using Neambc.Neamb.Foundation.MBCData.Repositories.Base;
using Neambc.Seiumb.Foundation.Sitecore;
using Newtonsoft.Json;

namespace Neambc.Neamb.Foundation.MBCData.Services.Rakuten
{
    [Service(typeof(IRakutenMemberRestRepository))]
    public class RakutenMemberRestRepository : IRakutenMemberRestRepository
    {
        private readonly ILog _log;
        private readonly IRakutenRestfulService _rakutenRestfulService;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;

        private readonly IOracleDatabase _oracleManager;
        public RakutenMemberRestRepository(
        ILog log, 
        IRakutenRestfulService rakutenRestfulService, 
        IOracleDatabase oracleManager,
        IGlobalConfigurationManager globalConfigurationManager)
        {
            _log = log;
            _rakutenRestfulService = rakutenRestfulService;
            _oracleManager = oracleManager;
            _globalConfigurationManager = globalConfigurationManager;
        }

        public RestResultDto<MemberCreationResponse> Post(RestRequestDto restRequestDto)
        {
            try
            {
                if (restRequestDto.Server == null || restRequestDto.Body == null)
                    throw new ArgumentException($"Parameters to MemberCreation in RakutenRestRepository for Post are incorrect");
                var memberCreationResponse = _rakutenRestfulService.Post<MemberCreationResponse>(restRequestDto);
                
                switch (memberCreationResponse.StatusCode)
                {
                    case HttpStatusCode.Created:
                        {
                            var ebToken = memberCreationResponse.Headers.ToList()
                                .Find(x => x.Name == "ebtoken")
                                .Value?.ToString();

                            memberCreationResponse.Result.EBtoken = ebToken;

                            var rakutenRequest = (RakutenRestRequestDto)restRequestDto;
                        
                            _oracleManager.Rakuten_Registration(rakutenRequest.MdsId, memberCreationResponse.Result.EmailAddress,
                                    memberCreationResponse.Result.Id, ebToken,
                                    rakutenRequest.UnionId, rakutenRequest.CellCode);

                            return memberCreationResponse;
                        }
                        
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.MethodNotAllowed:
                    case HttpStatusCode.HttpVersionNotSupported:
                    default:
                       {
                            _log.Error($"RakutenRestRepository MemberCreation Post Response from {restRequestDto.Server}: {JsonConvert.SerializeObject(memberCreationResponse)}", this);
                            return new RestResultDto<MemberCreationResponse> { StatusCode = memberCreationResponse.StatusCode };
                        }
                 }
            }
            catch (Exception ex) {
                _log.Error($"RakutenRestRepository Exception  {ex.Message }", this );
                return new RestResultDto<MemberCreationResponse> {
                    Success = false,
                    ExceptionDetail = $"Post Rakuten Repository {ex.Message}"
                };
            }
        }

    }
}