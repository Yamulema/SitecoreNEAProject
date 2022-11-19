using Neambc.Neamb.Feature.Rakuten.Model;
using Neambc.Neamb.Feature.Rakuten.Repositories;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Rakuten.Manager;
using Neambc.Seiumb.Feature.Rakuten.Constants;
using Neambc.Seiumb.Foundation.Authentication.Constants;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Neambc.Seiumb.Foundation.Authentication.Models;
using Neambc.Seiumb.Foundation.Sitecore.Extensions;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Web.Mvc;
using SignUpDto = Neambc.Seiumb.Feature.Rakuten.Models.SignUpDto;
using StoreDto = Neambc.Seiumb.Feature.Rakuten.Models.StoreDto;

namespace Neambc.Seiumb.Feature.Rakuten.Controllers
{
    public class SeiumbMarketplaceController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStoreManager _storeManager;
        private readonly IEligibilityManagerMarketplaceSeiumb _eligibilityManagerMarketplaceSeiumb;
        private readonly IMdsLoggingManager _mdsLoggingManager;
        private readonly IGlobalConfigurationManager _globalConfigurationManager;
        private readonly IUserRepository _userRepository;
        private readonly ISeiumbProfileManager _seiumbProfileManager;

        public SeiumbMarketplaceController(ICategoryRepository categoryRepository, IStoreManager storeManager,
            IEligibilityManagerMarketplaceSeiumb eligibilityManagerMarketplaceSeiumb, IMdsLoggingManager mdsLoggingManager,
            IGlobalConfigurationManager globalConfigurationManager, IUserRepository userRepository, ISeiumbProfileManager seiumbProfileManager)
        {
            _categoryRepository = categoryRepository;
            _storeManager = storeManager;
            _eligibilityManagerMarketplaceSeiumb = eligibilityManagerMarketplaceSeiumb;
            _mdsLoggingManager = mdsLoggingManager;
            _globalConfigurationManager = globalConfigurationManager;
            _userRepository = userRepository;
            _seiumbProfileManager = seiumbProfileManager;
        }

        public ActionResult Stores() {
            var productcode = _globalConfigurationManager.ProductCodeStoresSeiumb;
            var seiuProfile = _seiumbProfileManager.GetProfile();

            ExecuteMdsLoggingProcess(productcode,seiuProfile);
            var model = new StoreDto();
            model.Initialize(RenderingContext.Current.Rendering);
            model.StoresListLabel = model.Item.Fields[Templates.StoresList.Fields.StoresListLabel].Value;
            model.MoreDealsText = model.Item.Fields[Templates.StoresList.Fields.SeeMoreText].Value;
            model.NoFavoriteStoresContent = model.Item.Fields[Templates.StoresList.Fields.NoFavoriteStores].Value;
            var storeUrlParameter = Request != null ? Request.QueryString[ConstantsNeamb.StoreUrlParameter] : "";
            if (!string.IsNullOrEmpty(storeUrlParameter))
            {
                model.ShopingLink = _storeManager.GetShoppingLinkSeiumb(storeUrlParameter);
            }
            var mdsidInt = _seiumbProfileManager.GetProfile().MdsIdInt;
            if (mdsidInt==0)
            {
                model.Eligibility = EligibilityResultEnum.None;
            }
            else
            {
                var isEligible= _eligibilityManagerMarketplaceSeiumb.IsMemberEligible(mdsidInt);
                model.Eligibility = isEligible ? EligibilityResultEnum.Eligible : EligibilityResultEnum.NotEligible;
            }
            model.MemberEmail = GetMemberEmail();
            model.MdsId = seiuProfile.MdsId;
            model.ProductCode = productcode;
            return View("/Views/Seiumb.Marketplace/StoresList.cshtml", model);
        }


        public void ExecuteMdsLoggingProcess(string productcode, SeiuProfile seiuProfile) {
            var cellCode = System.Web.HttpContext.Current.Session[ConstantsSeiumb.SourceCode] != null ? System.Web.HttpContext.Current.Session[ConstantsSeiumb.SourceCode].ToString() : null;
            _mdsLoggingManager.ExecuteMdsLoggingProcess(productcode, ConstantsSeiumb.ViewProductCode, "V", seiuProfile.MdsId, cellCode, "");
        }

        public ActionResult Categories()
        {
            var model = new StoreCategories
            {
                Categories = _categoryRepository.GetCategories(),
                ShowFavorites = _seiumbProfileManager.InHotState() && _seiumbProfileManager.IsRakutenMember()
            };
            return View("/Views/Seiumb.Marketplace/StoreCategories.cshtml", model);
        }

        public ActionResult SignUp()
        {
            var model = new SignUpDto();
            model.Initialize(RenderingContext.Current.Rendering);
            return View("/Views/Seiumb.Marketplace/SignUpModal.cshtml", model);
        }

        #region Private Methods
        private string GetMemberEmail() {
            var status = _userRepository.GetUserStatus();
            var seiuProfile = _seiumbProfileManager.GetProfile();
            return new List<string> {UserStatusCons.HOT, UserStatusCons.WARM_HOT, UserStatusCons.WARM_COLD}.Contains(status)
                ? seiuProfile.Email : "";
        }
        #endregion
    }
}