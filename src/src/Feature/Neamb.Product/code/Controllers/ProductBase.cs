using System;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Feature.Product.Repositories;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Neamb.Foundation.Product.Pipelines;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Extensions;

namespace Neambc.Neamb.Feature.Product.Controllers
{
    public abstract class ProductBaseController : BaseController
    {
        protected IAccountServiceProxy _serviceManager;
        protected IOracleDatabase _oracleManager;
        protected IPipelineService _pipelineService;
        protected ICacheManager _cacheManager;
        protected ISessionManager _sessionManager;
        protected ISessionAuthenticationManager _sessionAuthenticationManager;
        protected IEligibilityManager _eligibilityManager;
        protected string pageId;
        protected ID _commingSoonItemId;
        protected ID _eligibilityItemId;
        protected ID _productCodeDroplinkItemId;
        protected ID _primaryCtaTypeItemId;
        protected ID _primaryCtaLinkItemId;
        protected ID _primaryPostDataItemId;
        protected ID _primaryCtaColorButton;
        protected ID _secondaryCtaTypeItemId;
        protected ID _secondaryCtaLinkItemId;
        protected ID _secondaryPostDataItemId;
        protected ID _secondaryCtaColorButton;
        protected ID _anonymousCtaColorButton;
        protected bool _isSpecialOffer;
        private string _componentId;
        private string _queryparameters;
        private string _productName;
        private string _productCode;
        private StatusEnum _statusUser;
        private bool _isAuthenticated;
        protected IProductGtmManager _productGtmManager;
        protected ComponentTypeEnum _componentType;
        protected ID _anonymousItemId;
        protected ID _reminderCtaId;
        protected IGlobalConfigurationManager _globalConfigurationManager;
        protected IComingSoonRepository _comingSoonRepository;
        protected ICtaActionRepository _ctaActionRepository;
        protected ID _goalPrimaryItemId;
        protected ID _goalSecondaryItemId;
        protected ID _requiresLogin;
        protected IProductUtilityManager _productUtilityManager;

        /// <summary>
        /// Main entry to get the cta actions
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="renderingItem">Rendering item</param>
        /// <param name="accountMembership">User data logged</param>
        public void ProcessEligibilityActions(ProductDetailDTO model, Item renderingItem, AccountMembership accountMembership)
        {
            //INITIALIZE VARIABLES
            //Get the product code value
            _statusUser = accountMembership.Status;
            _productCode = _productUtilityManager.GetProductCode(renderingItem, _productCodeDroplinkItemId);
            _productName = !String.IsNullOrEmpty(renderingItem.DisplayName) ? renderingItem.DisplayName : renderingItem.Name;
            _queryparameters = System.Web.HttpContext.Current.Request.QueryString.ToStringOrEmpty();
            _sessionManager.RetrieveFromSession<string>(ConstantsNeamb.QueryParameter);
            if (_isSpecialOffer && !string.IsNullOrEmpty(_queryparameters))
            {
                _sessionManager.StoreInSession<string>(ConstantsNeamb.QueryParameter, _queryparameters);
            }
            model.ComponentId = renderingItem.ID.ToString().Replace("{", string.Empty).Replace("}", string.Empty).Replace("-", string.Empty);
            _componentId = model.ComponentId;
            //Get the information for comming soon
            if (_commingSoonItemId != ID.Null && _commingSoonItemId != ID.Undefined)
            {
                model.HasCommingSoon = renderingItem.Fields[_commingSoonItemId].IsChecked();
            }

            //Check of eligibility
            model.HasCheckEligibility = renderingItem.Fields[_eligibilityItemId].IsChecked();
            if (_requiresLogin != ID.Null && _requiresLogin != ID.Undefined)
            {
                model.RequiresOnlyLogin = renderingItem.Fields[_requiresLogin].IsChecked();
            }

            //The user is authenticated
            _isAuthenticated = accountMembership.Status == StatusEnum.Hot ||
                accountMembership.Status == StatusEnum.WarmCold ||
                accountMembership.Status == StatusEnum.WarmHot;
            //Set the user status in the model
            model.UserStatus = accountMembership.Status;

            if (model.HasCheckEligibility && _isAuthenticated)
            {
                SetPropertiesEligibility(accountMembership, _productCode, model);
            }

            if (model.HasCommingSoon)
            {
                SetPropertiesCommingSoon(accountMembership, model, renderingItem);
            }
            else
            {
                ExecuteProcessAction(model, renderingItem, accountMembership, _productCode);
            }

            if (model.HasError)
            {
                throw new Exception($"There was an issue in Product with product code {_productCode}");
            }
        }

        public void SetButtonColorClass(ProductDetailDTO model, Item renderingItem)
        {
            model.CtaPrimaryColor = renderingItem.Fields[_primaryCtaColorButton].Value.ToLower();
            model.CtaSecondaryColor = renderingItem.Fields[_secondaryCtaColorButton].Value.ToLower();
            model.CtaAnonymousColor = renderingItem.Fields[_anonymousCtaColorButton].Value.ToLower();
        }


        /// <summary>
        /// Set some properties used in comming soon module
        /// </summary>
        /// <param name="accountMembership">User logged data</param>
        /// <param name="model">Model</param>
        /// <param name="renderingItem">Rendering item</param>
        private void SetPropertiesCommingSoon(AccountMembership accountMembership,
            ProductDetailDTO model, Item renderingItem)
        {
            ComingSoonRequest comingSoon = new ComingSoonRequest
            {
                ProductCode = _productCode,
                Mdsid = accountMembership.Mdsid,
                ProductName = _productName,
                ComponentId = _componentId,
                RenderingItem = renderingItem,
                ReminderCtaId = _reminderCtaId,
                UserName = accountMembership.Username,
                UrlActionHref = HttpContext.Request.Url != null ? HttpContext.Request.Url.AbsoluteUri : "",
                ComponentType = _componentType,
                EligibilityItemId = _eligibilityItemId,
                HasCheckEligibility = model.HasCheckEligibility
            };

            if (_isAuthenticated && _comingSoonRepository.VerifyAlreadyNotified(comingSoon))
            {
                //It is already notified
                model.HasAlreadyNotified = true;
            }
            else
            {
                var result = _comingSoonRepository.GetPropertiesUser(_statusUser, comingSoon);
                model.NotifyProductAvailableAction = result.Action;
                model.NotifyProductAvailableLink = result.Link;
                model.HasError = result.HasError;
            }
        }

        /// <summary>
        /// Set the properties for eligibility
        /// </summary>
        /// <param name="accountMembership">User logged data</param>
        /// <param name="productCode">Product code</param>
        /// <param name="model">Model</param>
        private void SetPropertiesEligibility(AccountMembership accountMembership, string productCode, ProductDetailDTO model)
        {
            if (string.IsNullOrEmpty(productCode) || string.IsNullOrEmpty(accountMembership?.Mdsid))
            {
                SetNotElegible(model);
                return;
            }

            //Get the result of eligibility from the webservice
            var resultEligibility = _eligibilityManager.IsMemberEligible(accountMembership.Mdsid, productCode);
            //Check the result of the webservice call
            if (resultEligibility == EligibilityResultEnum.Eligible)
            {
                model.ResultCheckEligibility = true;
            }
            else
            {
                SetNotElegible(model);
            }
        }

        private static void SetNotElegible(ProductDetailDTO model)
        {
            //Get some values for chat,phone,mail to be displayed when it is not eligible
            var siteSettings = Sitecore.Context.Database.GetItem(Templates.SiteSettings.ID);
            model.PhoneNumber = siteSettings[Templates.SiteSettings.Fields.Phone];
            model.SupportEmail = siteSettings[Templates.SiteSettings.Fields.Email];
            model.ResultCheckEligibility = false;
        }

        /// <summary>
        /// Verify the actions configured in Primary and Secondary actions
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="renderingItem">Item rendering</param>
        /// <param name="accountMembership">User logged data</param>
        /// <param name="productCode">Product code</param>
        private void ExecuteProcessAction(
            ProductDetailDTO model,
            Item renderingItem,
            AccountMembership accountMembership,
            string productCode
        )
        {
            ActionRequest actionRequestPrimary = new ActionRequest
            {
                ComponentType = _componentType,
                CtaLinkItemId = _primaryCtaLinkItemId,
                Model = model,
                ProductName = _productName,
                ProductCode = productCode,
                RenderingItem = renderingItem,
                UserName = accountMembership.Username,
                ComponentId = _componentId,
                EligibilityItemId = _eligibilityItemId,
                ActionButtonType = ActionButtonTypeEnum.Primary,
                IsSpecialOffer = _isSpecialOffer,
                CtaTypeItemId = _primaryCtaTypeItemId,
                PostDataItemId = _primaryPostDataItemId,
                ConstantCtaAction = ConstantsNeamb.CtaActionPrimary,
                ConstantCtaActionOnclick = ConstantsNeamb.CtaActionPrimaryOnclick,
                ConstantCtaActionTargetBlank = ConstantsNeamb.CtaActionPrimaryTargetBlank,
                ConstantActionType = ConstantsNeamb.Primary,
                ConstantProductGtmAction = ConstantsNeamb.ProductGtmActionPrimary,
                HasCheckEligibility = model.HasCheckEligibility,
                GoalPrimaryId = _goalPrimaryItemId,
                GoalSecondaryId = _goalSecondaryItemId
            };

            var resultActionButton = _ctaActionRepository.GetActionData(_statusUser, actionRequestPrimary);
            model.ActionPrimaryDescription = resultActionButton.Description;
            actionRequestPrimary.ActionDescription = resultActionButton.Description;            
            actionRequestPrimary.TargetAction = resultActionButton.Target;
            actionRequestPrimary.AnonymousItemId = _anonymousItemId;

            if ((!model.HasCheckEligibility) || (model.HasCheckEligibility && model.ResultCheckEligibility))
            {
                var resultPrimary = _ctaActionRepository.GetActionResult(_statusUser, actionRequestPrimary);
                model.HasErrorLink = resultPrimary.HasErrorLink;
                model.ActionPrimary = resultPrimary.ActionInner;
                model.ActionClickPrimary = resultPrimary.ActionClick;
                if (model.ActionClickPrimary!=null && !model.ActionClickPrimary.Contains("executeloginwarm"))
                {
                    model.ActionPrimaryTargetBlank = resultActionButton.Target;
                }

                if (resultActionButton.HasError || resultPrimary.HasError)
                {
                    model.HasError = true;
                }
                if (_secondaryCtaLinkItemId != ID.Null && !model.HasError
                && !String.IsNullOrEmpty(renderingItem[_secondaryCtaLinkItemId])
                && !String.IsNullOrEmpty(renderingItem[_secondaryCtaTypeItemId])
                )
                {
                    ActionRequest actionRequestSecondary = new ActionRequest
                    {
                        ComponentType = ComponentTypeEnum.CtaSecondary,
                        CtaLinkItemId = _secondaryCtaLinkItemId,
                        Model = model,
                        ProductName = _productName,
                        ProductCode = productCode,
                        RenderingItem = renderingItem,
                        UserName = accountMembership.Username,
                        ComponentId = _componentId,
                        EligibilityItemId = _eligibilityItemId,
                        ActionButtonType = ActionButtonTypeEnum.Secondary,
                        IsSpecialOffer = _isSpecialOffer,
                        CtaTypeItemId = _secondaryCtaTypeItemId,
                        PostDataItemId = _secondaryPostDataItemId,
                        ConstantCtaAction = ConstantsNeamb.CtaActionSecondary,
                        ConstantCtaActionOnclick = ConstantsNeamb.CtaActionSecondaryOnclick,
                        ConstantCtaActionTargetBlank = ConstantsNeamb.CtaActionSecondaryTargetBlank,
                        ConstantActionType = ConstantsNeamb.Secondary,
                        ConstantProductGtmAction = ConstantsNeamb.ProductGtmActionSecondary,
                        GoalPrimaryId = _goalPrimaryItemId,
                        GoalSecondaryId = _goalSecondaryItemId
                    };
                    var resultActionButtonSec = _ctaActionRepository.GetActionData(_statusUser, actionRequestSecondary);
                    model.ActionSecondaryDescription = resultActionButtonSec.Description;
                    actionRequestSecondary.ActionDescription = resultActionButtonSec.Description;
                    
                    actionRequestSecondary.TargetAction = resultActionButtonSec.Target;
                    actionRequestSecondary.AnonymousItemId = _anonymousItemId;

                    var resultSecondary = _ctaActionRepository.GetActionResult(_statusUser, actionRequestSecondary);
                    model.HasErrorLink = resultSecondary.HasErrorLink;
                    model.ActionSecondary = resultSecondary.ActionInner;
                    model.ActionClickSecondary = resultSecondary.ActionClick;
                    if (model.ActionClickSecondary != null && !model.ActionClickSecondary.Contains("executeloginwarm"))
                    {
                        model.ActionSecondaryTargetBlank = resultActionButtonSec.Target;
                    }
                    if (resultActionButtonSec.HasError || resultSecondary.HasError)
                    {
                        model.HasError = true;
                    }
                }
            }
            else if (model.HasCheckEligibility && (_statusUser == StatusEnum.Cold || _statusUser == StatusEnum.Unknown))
            {

                var actionResult = _ctaActionRepository.GetActionResult(_statusUser, actionRequestPrimary);
                model.HasErrorLink = actionResult.HasErrorLink;
                model.AnonymousText = actionResult.Anonymous.Text;
                model.AnonymousUrl = actionResult.Anonymous.Url;
                model.AnonymousGtmAction = actionResult.Anonymous.GtmAction;
                model.AnonymousFunctionAction = actionResult.ActionClick;
            }
        }
    }
}