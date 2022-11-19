using System;
using Neambc.Neamb.Feature.Account.Repositories;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Rakuten.Model;
using Neambc.Neamb.Feature.Rakuten.Repositories;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Rakuten.Manager;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Rakuten.Controllers
{
    public class MarketplaceController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStoreManager _storeManager;
        private readonly IEligibilityManagerMarketplace _eligibilityManagerMarketplace;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly ISessionAuthenticationManager _sessionAuthenticationManager;
        private readonly IProductManager _productManager;
        private readonly IAccountRepository _account;

        public MarketplaceController(ICategoryRepository categoryRepository, IStoreManager storeManager, 
            IEligibilityManagerMarketplace eligibilityManagerMarketplace, IGlobalConfigurationManager globalConfiguration, 
            ISessionAuthenticationManager sessionAuthenticationManager, IProductManager productManager, IAccountRepository account)
        {
            _categoryRepository = categoryRepository;
            _storeManager = storeManager;
            _eligibilityManagerMarketplace = eligibilityManagerMarketplace;
            _globalConfigurationManager = globalConfiguration;
            _sessionAuthenticationManager = sessionAuthenticationManager;
            _productManager = productManager;
            _account = account;
        }

        public ActionResult Stores()
        {
            var model = new StoreDto();
            //Set 
            var productCode = _globalConfigurationManager.ProductCodeStores;
            _productManager.ExecuteMdsLoggingProcessView(productCode);
            model.Initialize(RenderingContext.Current.Rendering);
            model.StoresListLabel = model.Item.Fields[Templates.StoresList.Fields.StoresListLabel].Value;
            model.MoreDealsText = model.Item.Fields[Templates.StoresList.Fields.SeeMoreText].Value;
            model.NoFavoriteStoresContent = model.Item.Fields[Templates.StoresList.Fields.NoFavoriteStores].Value;
            var storeUrlParameter = Request != null ? Request.QueryString[ConstantsNeamb.StoreUrlParameter] : "";
            if (!string.IsNullOrEmpty(storeUrlParameter)) {
                model.ShopingLink = _storeManager.GetShoppingLink(storeUrlParameter);
            }
            model.Eligibility = GetEligibility();
            model.MemberEmail = GetMemberEmail();
            model.ProductCode = productCode;
            return View("/Views/Neamb.Marketplace/StoresList.cshtml", model);
        }

        public ActionResult Categories()
        {
            var _member = _sessionAuthenticationManager.GetAccountMembership();
            _member.Profile = _account.RetrieveRakutenProfile(_member);
            var model = new StoreCategories {
                Categories = _categoryRepository.GetCategories(),
                ShowFavorites = _member.Status == StatusEnum.Hot && _member.Profile.IsRakutenMember
            };
            return View("/Views/Neamb.Marketplace/StoreCategories.cshtml", model);
        }

        public ActionResult SignUp()
        {
            var model = new SignUpDto();
            model.Initialize(RenderingContext.Current.Rendering);
            return View("/Views/Neamb.Marketplace/SignUpModal.cshtml", model);
        }

        #region Private Methods
        private EligibilityResultEnum GetEligibility() {
            try
            {
                var accountMembership = _sessionAuthenticationManager.GetAccountMembership();

                if (string.IsNullOrEmpty(accountMembership.Mdsid)) {
                    return EligibilityResultEnum.None;
                } else {
                    var productCode = _globalConfigurationManager.ProductCodeStores;
                    return _eligibilityManagerMarketplace.IsMemberEligible(accountMembership.Mdsid, productCode);
                }
            }
            catch (Exception exception)
            {
                Log.Error("Calling Is Member Eligible",exception, this);
                return EligibilityResultEnum.None;
            }
        }

        private string GetMemberEmail() {
            var member = _sessionAuthenticationManager.GetAccountMembership();
            return member.IsUserWithKnownState() ? member.Profile.Email : string.Empty;
        }
        #endregion
    }
}