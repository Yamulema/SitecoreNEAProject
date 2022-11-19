using System;
using System.Collections.Generic;
using Neambc.Neamb.Feature.Product.Interfaces;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neambc.Neamb.Feature.Product.Repositories
{
    [Service(typeof(ISweepstakesRepository))]
    public class SweepstakesRepository: SweepstakesBaseRepository,ISweepstakesRepository
    {
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly IEligibilityManager _eligibilityManager;
        private readonly IReminderService _reminderService;
        private readonly IGtmService _gtmService;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IExactTargetClient _exactTargetClient;
        
        public SweepstakesRepository(ISessionAuthenticationManager sessionAuthenticationManager, IEligibilityManager eligibilityManager, IReminderService reminderService, IGtmService gtmService, IGlobalConfigurationManager globalConfigurationManager, IExactTargetClient exactTargetClient) {
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _eligibilityManager = eligibilityManager;
            _reminderService = reminderService;
            _gtmService = gtmService;
            _globalConfigurationManager = globalConfigurationManager;
            _exactTargetClient = exactTargetClient;
        }

        public void SetComponentId(ref SweepstakesDTO sweepstakesDto, Guid renderingId) {
            //Set the ActionClickAuthentication property with the component id
            var renderingComponentId = renderingId.ToString("N");
            sweepstakesDto.SweepstakesBase.ComponentIdAuthentication = renderingComponentId;
        }

        public void SetActionClickAuthentication(ref SweepstakesDTO sweepstakesDto)
        {
            sweepstakesDto.SweepstakesBase.ActionClickAuthentication = $"executeloginhandlepostaction('{sweepstakesDto.SweepstakesBase.ComponentIdAuthentication}','{sweepstakesDto.SweepstakesBase.LoginUrl}','{LoginHandlerEnum.Sweepstake}')";
        }

        public void SetPropertiesSweepstake(ref SweepstakesDTO sweepstakesDto, Item renderingItem ) {
            var accountRepository = _sessionAuthenticationManager.GetAccountMembership();
            var result = sweepstakesDto.SweepstakesBase;
            SetBasicProperties(ref result, renderingItem, accountRepository);
            SweepstakeGtm sweepstakeGtm;
            if (accountRepository.Status == StatusEnum.Hot) {
                sweepstakeGtm = new SweepstakeGtm {
                    NavText = $"{sweepstakesDto.SweepstakesBase.CtaText} | sweepstakes {renderingItem[Templates.LandingPageCta.Fields.SweepstakeId]}"
                };
            } else {
                sweepstakeGtm = new SweepstakeGtm
                {
                    NavText = $"{sweepstakesDto.SweepstakesBase.LoginText} | sweepstakes {renderingItem[Templates.LandingPageCta.Fields.SweepstakeId]}"
                };
            }

            sweepstakesDto.SweepstakesBase.GtmAction = _gtmService.GetGtmEvent(sweepstakeGtm);
            
            //fill the rest of properties
            sweepstakesDto.HasCheckEligibility = renderingItem.Fields[Templates.LandingPageCta.Fields.Eligibility].IsChecked();
            var isAuthenticated = accountRepository.Status == StatusEnum.Hot ||
                accountRepository.Status == StatusEnum.WarmCold ||
                accountRepository.Status == StatusEnum.WarmHot;
            if (sweepstakesDto.HasCheckEligibility && isAuthenticated)
            {
                var programCode = renderingItem[Templates.LandingPageCta.Fields.ProgramCode];
                //Get the eligibility
                var resultEligibility = _eligibilityManager.IsMemberEligible(accountRepository.Mdsid, programCode);
                if (resultEligibility == EligibilityResultEnum.Eligible)
                {
                    //Set the information of the user
                    sweepstakesDto.HasEligibility = true;
                    sweepstakesDto.FullName = string.Format("{0} {1}", accountRepository.Profile.FirstName, accountRepository.Profile.LastName);
                    sweepstakesDto.Address = accountRepository.Profile.StreetAddress;
                    sweepstakesDto.City = string.Format("{0}, {1} {2}", accountRepository.Profile.City, accountRepository.Profile.StateCode,
                        accountRepository.Profile.ZipCode);
                    if (accountRepository.Profile.Phone != null)
                    {
                        if (accountRepository.Profile.Phone.Length == 10)
                        {
                            sweepstakesDto.Phone = string.Format("{0}-{1}-{2}", accountRepository.Profile.Phone.Substring(0, 3),
                                accountRepository.Profile.Phone.Substring(3, 3), accountRepository.Profile.Phone.Substring(6, 4));
                        }
                    }

                    sweepstakesDto.Email = accountRepository.Username;
                }
            }
            sweepstakesDto.SweepstakesId = renderingItem[Templates.LandingPageCta.Fields.SweepstakeId];
            //Set the class according the eligibility result
            sweepstakesDto.HasClassEligibility = (sweepstakesDto.HasCheckEligibility && !sweepstakesDto.HasEligibility && sweepstakesDto.SweepstakesBase.IsAuthenticated);
            sweepstakesDto.SweepstakesBase.Reminder = _reminderService.GetReminder(sweepstakesDto);
        }

        /// <summary>
        /// Send an email in sweepstake component
        /// </summary>
        /// <param name="contextItem">context Item</param>
        /// <param name="accountMembership">User authenticated</param>
        /// <returns>true process executed successfully otherwise false</returns>
        public bool SendEmail(Item contextItem, AccountMembership accountMembership) {
            if (contextItem.Fields[Templates.Sweepstakes.Fields.SendEmailNotification].IsChecked()) {
                var customerDefinition = _globalConfigurationManager.CustomerDefinitionSweepstake;
                var cellCode = contextItem[Templates.LandingPageCta.Fields.Cellcode];
                var campaignCode = contextItem[Templates.LandingPageCta.Fields.Campaigncode];

                var parameters = new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("FIRST_NAME", accountMembership.Profile.FirstName),
                    new KeyValuePair<string, string>("LAST_NAME", accountMembership.Profile.LastName),
                    new KeyValuePair<string, string>("CELL_CODE", cellCode),
                    new KeyValuePair<string, string>("CAMPAIGN_CD", campaignCode),
                    new KeyValuePair<string, string>("INDIVIDUAL_ID", accountMembership.Mdsid.PadLeft(9, '0'))
                };

                //Send the email with exact target
                var resultEmail = _exactTargetClient.SendExactTargetService(customerDefinition,
                    accountMembership.Username,
                    parameters,
                    accountMembership.Mdsid.PadLeft(9, '0'));
                return resultEmail;
            }
            else {
                return true;
            }
        }
    }
}