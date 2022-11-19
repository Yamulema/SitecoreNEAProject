using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Rakuten.Model;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;

namespace Neambc.Neamb.Foundation.Rakuten.Manager
{
    [Service(typeof(IStoreManager))]
    public class StoreManager: IStoreManager
    {
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        protected IEligibilityManagerMarketplace _eligibilityManagerMarketplace;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IGtmService _gtmService;
        private readonly ISeiumbProfileManager _seiumbProfileManager;

        public StoreManager(ISessionAuthenticationManager sessionAuthenticationManager, IEligibilityManagerMarketplace eligibilityManagerMarketplace, 
            IGlobalConfigurationManager globalConfigurationManager, IGtmService gtmService, ISeiumbProfileManager seiumbProfileManager) {
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _eligibilityManagerMarketplace = eligibilityManagerMarketplace;
            _globalConfigurationManager = globalConfigurationManager;
            _gtmService = gtmService;
            _seiumbProfileManager = seiumbProfileManager;
        }

        public string GetShoppingLinkSeiumb(string storeId)
        {
            var storeItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(storeId));
            if (storeItem == null) return "";
            var rakutenResponse = _seiumbProfileManager.GetRakutenMemberCreationResponse();
            return rakutenResponse != null ? $"{storeItem[Templates.RakutenStore.Fields.ShoppingUrlSeiumb]}{rakutenResponse.EBtoken}" : $"{storeItem[Templates.RakutenStore.Fields.ShoppingUrlSeiumb]}";
        }

        public string GetShoppingLink(string storeId) {
            var storeItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(storeId));
            var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
            return storeItem != null ? accountMembership.Profile.RakutenProfile != null ?
                    $"{storeItem[Templates.RakutenStore.Fields.ShoppingUrl]}{accountMembership.Profile.RakutenProfile.EBToken}" : 
                    $"{storeItem[Templates.RakutenStore.Fields.ShoppingUrl]}" : "";
        }

        public bool CheckEligibilityUser() {
            var account = _sessionAuthenticationManager.GetAccountMembership();
            if (account.Status != StatusEnum.Hot && account.Status != StatusEnum.WarmCold && account.Status != StatusEnum.WarmHot) return true;
            var resultEligibility = _eligibilityManagerMarketplace.IsMemberEligible(account.Mdsid, _globalConfigurationManager.ProductCodeStores);
            return resultEligibility == EligibilityResultEnum.Eligible;
        }

        public string GetGtmFunction(GtmRakutenType gtmRakutenType, string storeId, string ctaText, bool getFullGtmEvent=true) {
            switch (gtmRakutenType) {
                case GtmRakutenType.Action: {
                    var storeItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(storeId));
                    var itemName = storeItem[Templates.RakutenStore.Fields.Name];
                    var rakutenStoreGtm = new RakutenStoreGtm {
                        Event = "discount offer card",
                        ClickHref = storeItem[Templates.RakutenStore.Fields.ShoppingUrl],
                        CardTitle = itemName,
                        CtaText = ctaText
                    };
                    return getFullGtmEvent ? _gtmService.GetGtmEvent(rakutenStoreGtm) : _gtmService.GetParameterGtmEvent(rakutenStoreGtm);
                }
                case GtmRakutenType.HotDeal:
                {
                    var storeItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(storeId));
                    var itemName = storeItem[Templates.RakutenStore.Fields.Name];
                    var rakutenStoreGtm = new RakutenStoreGtm {
                        Event = "trending discounts",
                        ClickHref = storeItem[Templates.RakutenStore.Fields.ShoppingUrl],
                        CardTitle = itemName,
                        CtaText = ctaText
                    };
                    return getFullGtmEvent ? _gtmService.GetGtmEvent(rakutenStoreGtm) : _gtmService.GetParameterGtmEvent(rakutenStoreGtm);
                }
                default: {
                    return "";
                }
            }
        }
    }
}