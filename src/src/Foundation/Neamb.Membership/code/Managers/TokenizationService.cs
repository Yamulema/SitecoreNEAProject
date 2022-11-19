using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Enums;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.MBCData.Repositories;
using Neambc.Neamb.Foundation.MBCData.Services;
using Neambc.Neamb.Foundation.MBCData.Services.IceDollars;
using Neambc.Neamb.Foundation.Membership.Interfaces;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Membership.Services;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
    [Service(typeof(ITokenizationService))]
    public class TokenizationService : TokenizationServiceBase, ITokenizationService
    {

        #region Fields
        private readonly IConvertMdsToRewards _rewardsService;
        private readonly IResourcesService _resourcesService;
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        //added by Daniel 8-8
        private readonly IIceDollarsService _iceDollarsService;
        private readonly ISessionService _sessionService;
        private readonly ISeminarRepository _seminarRepository;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        protected IEligibilityManager _eligibilityManager;
        private readonly IEligibilityOmni _eligibilityOmni;
        private readonly IProductUtilityManager _productUtilityManager;
        private readonly IRenderingSitecoreContext _renderingSitecoreContext;
        private readonly ITokenManager _tokenManager;
        #endregion

        #region Constructor
        public TokenizationService(
            IConvertMdsToRewards rewardsService,
            IResourcesService resourcesService,
            ISessionAuthenticationManager sessionAuthenticationManager,
            ISessionService sessionService,
            IIceDollarsService iceDollarsService,
            ISeminarRepository seminarRepository,
            IGlobalConfigurationManager globalConfiguration,
            IEligibilityManager eligibilityManager,
            IEligibilityOmni eligibilityOmni,
            IProductUtilityManager productUtilityManager,
            IRenderingSitecoreContext renderingSitecoreContext,
            ITokenManager tokenManager
        )
        {
            _rewardsService = rewardsService;
            _resourcesService = resourcesService;
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _sessionService = sessionService;
            _iceDollarsService = iceDollarsService;
            _globalConfigurationManager = globalConfiguration;
            _eligibilityManager = eligibilityManager;
            _seminarRepository = seminarRepository;
            _eligibilityOmni = eligibilityOmni;
            _productUtilityManager = productUtilityManager;
            _renderingSitecoreContext = renderingSitecoreContext;
            _tokenManager = tokenManager;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns>Item 1: Raw token, Item 2: Typed Token, Item 3: Token value</returns>
        protected override IEnumerable<Tuple<string, Token, string>> GetMappedTokens(IEnumerable<Tuple<string, Token>> tokens)
        {
            return tokens.Select(x =>
            {
                var mappedValue = string.Empty;
                switch (x.Item2)
                {
                    case Token.cellcode:
                        mappedValue = _sessionAuthenticationManager.GetCellCode();
                        break;
                    case Token.campcode:
                        mappedValue = _sessionAuthenticationManager.GetCampaignCode();
                        break;
                    case Token.medium:
                        mappedValue = _sessionAuthenticationManager.GetMedium();
                        break;
                    case Token.term:
                        mappedValue = _sessionAuthenticationManager.GetUtmTerm();
                        break;
                    case Token.FirstName:
                        mappedValue = GetName();
                        break;
                    case Token.LastName:
                        mappedValue = GetLastName();
                        break;
                    case Token.Phone:
                        mappedValue = GetPhone();
                        break;
                    case Token.Email:
                        mappedValue = GetEmail();
                        break;
                    case Token.StateCode:
                        mappedValue = GetStateCode();
                        break;
                    case Token.IcePoints:
                        mappedValue = GetIcePoints();
                        break;
                    case Token.StateNames:
                        mappedValue = GetStateNames();
                        break;
                    case Token.ReturnUrl:
                        mappedValue = GetReturnUrl();
                        break;
                    case Token.IceTravelDollars:
                        mappedValue = GetIceTravelDollars();
                        break;
                    case Token.SeminarAddress:
                        mappedValue = GetSeminarField(Token.SeminarAddress);
                        break;
                    case Token.SeminarDate:
                        mappedValue = GetSeminarField(Token.SeminarDate);
                        break;
                    case Token.SeminarName:
                        mappedValue = GetSeminarField(Token.SeminarName);
                        break;
                    case Token.SeminarTime:
                        mappedValue = GetSeminarField(Token.SeminarTime);
                        break;
                    case Token.SeminarCityStateZipCode:
                        mappedValue = GetSeminarField(Token.SeminarCityStateZipCode);
                        break;
                    case Token.SeminarLocation1:
                        mappedValue = GetSeminarField(Token.SeminarLocation1);
                        break;
                    case Token.SeminarLocation2:
                        mappedValue = GetSeminarField(Token.SeminarLocation2);
                        break;
                    case Token.LeaName:
                        mappedValue = GetSeminarField(Token.LeaName);
                        break;
                    case Token.SeaName:
                        mappedValue = GetSeminarField(Token.SeaName);
                        break;
                    case Token.PresenterName:
                        mappedValue = GetSeminarField(Token.PresenterName);
                        break;
                    case Token.NotYou:
                        mappedValue = NotYou();
                        break;
                    case Token.FirstNameExclamation:
                        mappedValue = GetNameExclamation();
                        break;
                    case Token.mdsid_clear:
                        mappedValue = GetMdsidClear();
                        break;
                    case Token.mdsid_clear_int:
                        mappedValue = GetMdsidClearInt();
                        break;
                    case Token.rakuten_signup:
                        mappedValue = GetRakutenLink();
                        break;
                    case Token.OmniSoctUrl:
                        mappedValue = GetOmniSoctUrl(x.Item1);
                        break;
                    case Token.OmniExpirationDate:
                        mappedValue = GetOmniExpirationDate();
                        break;
                    case Token.NcesId:
                        mappedValue = _tokenManager.GetNcesId();
                        break;
                    default:
                        mappedValue = string.Empty;
                        break;
                }
                return new Tuple<string, Token, string>(x.Item1, x.Item2, mappedValue);
            });
        }

        private string GetReturnUrl()
        {
            return _sessionService.Get(Configuration.ReturnUrlArg)?.ToString() ?? string.Empty;
        }

        private string GetStateNames()
        {
            StringBuilder sb = new StringBuilder();
            //Get the states from Global folder in Sitecore
            var statesList = Sitecore.Context.Database.GetItem(Templates.StatesGlobal.ID).GetChildren();

            foreach (Item state in statesList)
            {
                if (state != null)
                {
                    sb.AppendLine($"<option value = \"{state[Templates.NameValueItem.Fields.ItemValue]}\">{state.Name}</option>");
                }
            }
            return sb.ToString();
        }

        private string GetIcePoints()
        {
            var template = _resourcesService.ReadTextResourceFromAssembly(Configuration.IcePointsTemplate);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(template);
            var htmlTableNode = htmlDoc.DocumentNode.SelectSingleNode("//tbody");

            var mdsId = _sessionAuthenticationManager.GetAccountMembership()?.Mdsid;
            if (!string.IsNullOrEmpty(mdsId))
            {

                var rewards = _rewardsService.Unredeemed(mdsId).Where(x => x.Date != DateTime.MinValue);
                foreach (var reward in rewards)
                {
                    var columns = new List<Tuple<string, Dictionary<string, string>>> {
                        new Tuple<string, Dictionary<string, string>>($"&nbsp;{reward.Date:MM/dd/yyy}", null),
                        new Tuple<string, Dictionary<string, string>>(reward.Description, null),
                        new Tuple<string, Dictionary<string, string>>($"{reward.Value}",
                            new Dictionary<string, string> {
                                {"style", "text-align: center;"}
                            })
                    };
                    htmlTableNode.ChildNodes.Append(
                        HtmlNode.CreateNode(GetHtmlTableRow(columns))
                    );
                }

                var totalPoints = _rewardsService.SelectUnredeemedRewardsTotal(mdsId);
                htmlTableNode.ChildNodes.Append(
                    HtmlNode.CreateNode(GetHtmlTableRow(new List<Tuple<string, Dictionary<string, string>>> {
                        new Tuple<string, Dictionary<string, string>>($"&nbsp;Total", null),
                        new Tuple<string, Dictionary<string, string>>($"&nbsp;", null),
                        new Tuple<string, Dictionary<string, string>>($"&nbsp;{totalPoints}",
                            new Dictionary<string, string> {
                                {"style", "text-align: center;"}
                            })
                    }))
                );
            }

            return htmlDoc.DocumentNode.OuterHtml;
        }
        private string GetIceTravelDollars()
        {
            var mdsId = _sessionAuthenticationManager.GetAccountMembership()?.Mdsid;
            if (!string.IsNullOrEmpty(mdsId))
            {
                if (int.TryParse(mdsId, out int mdsIdNumber)) {
                    var response = _iceDollarsService.GetBalance(mdsIdNumber);
                    if (response.Success && response.Data != null) return response.Data.PointsBalance.ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Item 1: Column value, Item 2: attributes
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        private string GetHtmlTableRow(IEnumerable<Tuple<string, Dictionary<string, string>>> columns)
        {
            var htmlRow = HtmlNode.CreateNode("<tr></tr>");
            foreach (var column in columns)
            {
                var htmlColumn = HtmlNode.CreateNode($"<td>{column.Item1}</td>");
                if (column.Item2 != null)
                {
                    foreach (var attribute in column.Item2)
                    {
                        htmlColumn.Attributes.Add(attribute.Key, attribute.Value);
                    }
                }
                htmlRow.ChildNodes.Append(htmlColumn);
            }
            return htmlRow.OwnerDocument.DocumentNode.OuterHtml;
        }

        private string GetName()
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            return accountMembership?.Profile?.FirstName ?? string.Empty;
        }

        private string GetNameExclamation()
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            return (accountMembership != null && accountMembership.Profile != null) && !string.IsNullOrEmpty(accountMembership?.Profile?.FirstName) ? $"{accountMembership.Profile.FirstName}!" : string.Empty;
        }

        private string GetLastName()
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            return accountMembership?.Profile?.LastName ?? string.Empty;
        }

        private string GetPhone()
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            return accountMembership?.Profile?.Phone ?? string.Empty;
        }

        private string GetEmail()
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            return accountMembership?.Profile?.Email ?? string.Empty;
        }

        private string GetStateCode()
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            return accountMembership?.Profile?.StateCode ?? string.Empty;
        }

        private string GetMdsidClear()
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            return (accountMembership != null && accountMembership.Profile != null) ? accountMembership.Mdsid : string.Empty;
        }

        private string GetMdsidClearInt()
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            return (accountMembership != null && accountMembership.Profile != null && !string.IsNullOrEmpty(accountMembership.Mdsid)) ? int.Parse(accountMembership.Mdsid).ToString() : string.Empty;
        }

        private string NotYou()
        {
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            if (accountMembership.Status == StatusEnum.WarmCold || accountMembership.Status == StatusEnum.WarmHot)
            {
                return "<a href=\"#\" onclick=\"logout('/login');\">Not You?</a>";
            }
            else
            {
                return "";
            }
        }

        private string GetSeminarField(Token token)
        {
            var seminaryFound = _seminarRepository.GetSeminary(RenderingContext.Current.Rendering.Item);
            if (seminaryFound != null)
            {
                switch (token)
                {
                    case Token.SeminarAddress:
                        return seminaryFound.Address;
                    case Token.SeminarDate:
                        return seminaryFound.SeminarDate;
                    case Token.SeminarName:
                        return seminaryFound.SeminarName;
                    case Token.SeminarTime:
                        return seminaryFound.SeminarTime;
                    case Token.SeminarLocation1:
                        return seminaryFound.Location1;
                    case Token.SeminarLocation2:
                        return seminaryFound.Location2;
                    case Token.LeaName:
                        return seminaryFound.LeaName;
                    case Token.SeminarCityStateZipCode:
                        return $"{seminaryFound.City} {seminaryFound.State} {seminaryFound.Zip}";
                    case Token.SeaName:
                        return seminaryFound.SeaName;
                    case Token.PresenterName:
                        return seminaryFound.PresenterName;
                }
            }
            return "";
        }

        private string GetRakutenLink()
        {
            string rakutenLink;
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            if (accountMembership.Status != StatusEnum.Hot)
            {
                var pathLoginPage = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID));
                rakutenLink = pathLoginPage + "?" + ConstantsNeamb.NoVendorRakuten + "=1&" + ConstantsNeamb.RedirectUrlLoginParameter + "=" + Templates.MarketplacePage.ID;
                return rakutenLink;
            }
            else
            {
                var pathMkpPage = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.MarketplacePage.ID));
                var eligible = "3";
                if (!CheckEligibilityUser()) eligible = "2";
                return pathMkpPage + "?" + ConstantsNeamb.NoVendorRakuten + "=1&result=" + eligible;
            }
        }

        public bool CheckEligibilityUser()
        {
            var account = _sessionAuthenticationManager.GetAccountMembership();
            if (account.Status == StatusEnum.Hot || account.Status == StatusEnum.WarmCold || account.Status == StatusEnum.WarmHot)
            {
                var resultEligibility = _eligibilityManager.IsMemberEligible(account.Mdsid, _globalConfigurationManager.ProductCodeStores);
                return resultEligibility == EligibilityResultEnum.Eligible;
            }
            return true;
        }

        private string GetOmniSoctUrl(string tokenValue) {
            var account = _sessionAuthenticationManager.GetAccountMembership();
            if (!string.IsNullOrEmpty(account.Mdsid)) {
                var productCode = _productUtilityManager.GetProductCode(RenderingContext.Current.Rendering.Item, Templates.ProductCTAs.Fields.ProductCodeDroplink);
                if (string.IsNullOrEmpty(productCode))
                    return string.Empty;

                var availability = _eligibilityOmni.CheckEligibility(account.Mdsid, productCode);
                if (availability != null) return availability.FirstOrDefault()?.WebSoctUrl;
            }
            var defaultUrl = tokenValue.Split('|');
            return defaultUrl.Length == 2 ? defaultUrl[1].Remove(defaultUrl[1].Length - 1) : string.Empty;
        }

        private string GetOmniExpirationDate()
        {
            var account = _sessionAuthenticationManager.GetAccountMembership();
            if (!string.IsNullOrEmpty(account.Mdsid))
            {
                var productCode = _productUtilityManager.GetProductCode(_renderingSitecoreContext.Current, Templates.ProductCTAs.Fields.ProductCodeDroplink);
                if (string.IsNullOrEmpty(productCode))
                    return string.Empty;

                var availability = _eligibilityOmni.CheckEligibility(account.Mdsid, productCode);
                if (availability != null) return availability.FirstOrDefault()?.WebEndDt;
            }
            return string.Empty;
        }

        #endregion

        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null)
        {
            return DeTokenize(input);
        }
    }
}