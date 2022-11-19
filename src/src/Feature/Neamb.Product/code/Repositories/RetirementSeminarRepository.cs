using System;
using System.Collections.Generic;
using System.Linq;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using Convert = System.Convert;

namespace Neambc.Neamb.Feature.Product.Repositories
{
    [Service(typeof(IRetirementSeminarRepository))]
    public class RetirementSeminarRepository : SweepstakesBaseRepository,IRetirementSeminarRepository
    {
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly IOracleDatabase _oracleDatabase;
        private readonly ISeminarRepository _seminaryRepository;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IExactTargetClient _exactTargetClient;
        private readonly IGtmService _gtmService;
        private readonly ILoginHandlerPostAction _loginHandlerPostAction;

        public RetirementSeminarRepository(ISessionAuthenticationManager sessionAuthenticationManager, IOracleDatabase oracleDatabase, ISeminarRepository seminaryRepository, IGlobalConfigurationManager globalConfigurationManager, IExactTargetClient exactTargetClient, IGtmService gtmService, ILoginHandlerPostAction loginHandlerPostAction) {
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _oracleDatabase = oracleDatabase;
            _seminaryRepository = seminaryRepository;
            _globalConfigurationManager = globalConfigurationManager;
            _exactTargetClient = exactTargetClient;
            _gtmService = gtmService;
            _loginHandlerPostAction = loginHandlerPostAction;
        }

        public void SetPropertiesRetirementSeminar(ref RetirementSeminarDTO retirementSeminarDto, Item renderingItem,Guid renderingId, string componentId) {
            var accountRepository = _sessionAuthenticationManager.GetAccountMembership();
            var result = retirementSeminarDto.SweepstakesBase;
            if (result != null) {
                SetBasicProperties(ref result, renderingItem, accountRepository);
                retirementSeminarDto.IsMember = accountRepository.Profile.IsNeaCurrentMember && accountRepository.Status == StatusEnum.Hot;
                retirementSeminarDto.IsValidSeminary = _seminaryRepository.IsValidSeminaryId(renderingItem);
                retirementSeminarDto.IsWarmUser = accountRepository.Status == StatusEnum.WarmCold || accountRepository.Status == StatusEnum.WarmHot;
                if (retirementSeminarDto.SweepstakesBase.IsAuthenticated) {
                    var navigation = new Navigation
                    {
                        Event = "navigation",
                        NavType = "embedded link",
                        NavText = "seminar - reserve your seat"
                    };
                    retirementSeminarDto.GtmAction  = _gtmService.GetGtmEvent(navigation);
                }
                else {
                    var navigation = new Navigation
                    {
                        Event = "navigation",
                        NavType = "embedded link",
                        NavText = "seminar - login"
                    };
                    retirementSeminarDto.GtmAction = _gtmService.GetGtmEvent(navigation);
                }
                var seminarId = _seminaryRepository.GetSeminarId(renderingItem);
                retirementSeminarDto.SeminarId = seminarId;
                var registrations = _oracleDatabase.ViewAllSeminarReg();
                var registrationUser =
                    registrations?.FirstOrDefault(item => item.SeminarId == seminarId && item.IndvId == Convert.ToInt32(accountRepository.Mdsid).ToString());
                if (registrationUser != null) {
                    retirementSeminarDto.AlreadyRegistered = true;
                }
                var renderingComponentIdFormatted = renderingId.ToString("N");

                if (accountRepository.Status == StatusEnum.Unknown ||
                    accountRepository.Status == StatusEnum.Cold ||
                    accountRepository.Status == StatusEnum.WarmHot ||
                    accountRepository.Status == StatusEnum.WarmCold) {
                    retirementSeminarDto.SweepstakesBase.ComponentIdAuthentication = renderingComponentIdFormatted;

                    retirementSeminarDto.SweepstakesBase.ActionClickAuthentication =
                        $"executeloginhandlepostaction('{retirementSeminarDto.SweepstakesBase.ComponentIdAuthentication}','{retirementSeminarDto.SweepstakesBase.LoginUrl}','{LoginHandlerEnum.Seminar}')";
                }
                retirementSeminarDto.SweepstakesBase.HasResultAuthentication = _loginHandlerPostAction.VerifyExecutionPostAction(renderingComponentIdFormatted, componentId, LoginHandlerEnum.Seminar);

            }
        }

        public RetirementSeminarResponse ExecuteRegistrationRetirementSeminar(RetirementSeminarDTO retirementSeminarDto) {
            RetirementSeminarResponse response = new RetirementSeminarResponse();
            if (retirementSeminarDto==null || string.IsNullOrEmpty(retirementSeminarDto.ContextItem)) {
                response.HasError = true;
                response.ProcessedSucessfully = false;
            } else {
                var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
                //Case is user not hot
                if (accountMembership == null || accountMembership.Status != StatusEnum.Hot) {
                    response.ErrorAuthentication = true;
                } else {
                    var contextItem = Sitecore.Context.Database.GetItem(retirementSeminarDto.ContextItem);
                    var leaCode = _seminaryRepository.GetLeaCode(contextItem);
                    Sitecore.Diagnostics.Log.Debug($"LeaCode {leaCode}", this);
                    var resultDatabase = _oracleDatabase.InsertInSeminar(Convert.ToInt32(accountMembership.Mdsid), leaCode);
                    var customerDefinition = _globalConfigurationManager.CustomerDefinitionSeminarForm;
                    var cellCode = contextItem[Templates.LandingPageCta.Fields.Cellcode];
                    var campaignCode = contextItem[Templates.LandingPageCta.Fields.Campaigncode];
                    var seminaryFound = _seminaryRepository.GetSeminary(contextItem);
                    Log.Debug($"SEMINAR_DATE {seminaryFound.SeminarDate}", this);
                    Log.Debug($"SEMINAR_TIME {seminaryFound.SeminarTime}", this);
                    var parameters = new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>("FIRST_NAME", accountMembership.Profile.FirstName),
                        new KeyValuePair<string, string>("LAST_NAME", accountMembership.Profile.LastName),
                        new KeyValuePair<string, string>("CELL_CODE", cellCode),
                        new KeyValuePair<string, string>("CAMPAIGN_CD", campaignCode),
                        new KeyValuePair<string, string>("INDIVIDUAL_ID", accountMembership.Mdsid.PadLeft(9, '0')),
                        new KeyValuePair<string, string>("SEMINAR_NAME", seminaryFound.SeminarName),
                        new KeyValuePair<string, string>("ADDRESS", seminaryFound.Address),
                        new KeyValuePair<string, string>("SEMINAR_DATE", seminaryFound.SeminarDate),
                        new KeyValuePair<string, string>("SEMINAR_TIME", seminaryFound.SeminarTime)
                    };

                    //Send the email with exact target
                    var resultEmail = _exactTargetClient.SendExactTargetService(customerDefinition,
                        accountMembership.Username,
                        parameters,
                        accountMembership.Mdsid.PadLeft(9, '0'));


                    if (resultDatabase && resultEmail) {
                        response.HasError = false;
                        response.ProcessedSucessfully = true;
                    } else {
                        response.HasError = true;
                        response.ProcessedSucessfully = false;
                    }
                }
            }
            return response;
        }

        public void SetActionClickAuthentication(ref SweepstakesDTO sweepstakesDto)
        {
            sweepstakesDto.SweepstakesBase.ActionClickAuthentication = $"executeloginseminary('{sweepstakesDto.SweepstakesBase.ComponentIdAuthentication}','{sweepstakesDto.SweepstakesBase.LoginUrl}')";
        }

    }
}