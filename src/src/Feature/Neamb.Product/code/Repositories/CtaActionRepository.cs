using System;
using System.Collections.Generic;
using System.IO;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Seiumb.Foundation.Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Layouts;
using System.Linq;
using  System.Xml.Linq;
using Neambc.Neamb.Foundation.Configuration.Extensions;

namespace Neambc.Neamb.Feature.Product.Repositories
{
	[Service(typeof(ICtaActionRepository))]
	public class CtaActionRepository : ICtaActionRepository {

        private readonly ISessionManager _sessionManager;
		private readonly IProductGtmManager _productGtmManager;
		private readonly IBaseUrlActionLink _baseUrlActionLink;
		private readonly IBaseUrlActionDatapass _baseUrlActionDatapass;
		private readonly IBaseUrlActionEfulfillment _baseUrlActionEfulfillment;
        private readonly IBaseUrlActionOmni _baseUrlActionOmni;
		private readonly IBaseUrlColdUser _baseUrlColdUser;
		private readonly IOracleDatabase _oracleManager;
		private readonly ILog _log;
        private readonly IPageSitecoreContext _pageSitecoreContext;

        public CtaActionRepository(ISessionManager sessionManager, IProductGtmManager productGtmManager, IBaseUrlActionLink baseUrlActionLink,
			IBaseUrlActionDatapass baseUrlActionDatapass, IBaseUrlActionEfulfillment baseUrlActionEfulfillment,
			IBaseUrlColdUser baseUrlColdUser, IOracleDatabase oracleManager, ILog log, IBaseUrlActionOmni baseUrlActionOmni, IPageSitecoreContext pageSitecoreContext 
        ) {
            _baseUrlActionOmni = baseUrlActionOmni;
            _sessionManager = sessionManager;
			_productGtmManager = productGtmManager;
			_baseUrlActionDatapass = baseUrlActionDatapass;
			_baseUrlActionEfulfillment = baseUrlActionEfulfillment;
			_baseUrlActionLink = baseUrlActionLink;
			_baseUrlColdUser = baseUrlColdUser;
			_oracleManager = oracleManager;
			_log = log;
            _pageSitecoreContext = pageSitecoreContext;
        }

        public ActionButtonResult GetActionResult(StatusEnum statusUser, ActionRequest actionRequest)
        {
            if (actionRequest.RenderingItem != null && actionRequest.CtaTypeItemId != ID.Null && actionRequest.CtaTypeItemId != ID.Undefined)
            {
                if (_pageSitecoreContext.Current!=null && (_pageSitecoreContext.Current.Fields[Templates.ProductCTAs.Fields.IsOmni].IsChecked() || actionRequest.RenderingItem.Fields[Templates.ProductCTAs.Fields.IsOmni].IsChecked()))
                {
                    actionRequest.IsOmni = true;
                }
                actionRequest.ActionType = GetActionType(actionRequest);
                actionRequest.PostData = GetPostParams(actionRequest);

                ProductActionRequest productActionRequest = new ProductActionRequest
                {
                    ActionType = actionRequest.ActionType,
                    ProductCode = actionRequest.ProductCode,
                    ComponentId = actionRequest.ComponentId,
                    ContextItem = actionRequest.RenderingItem,
                    CtaLinkItemId = actionRequest.CtaLinkItemId,
                    EligibilityItemId = actionRequest.EligibilityItemId,
                    HasCheckEligibility = actionRequest.Model.HasCheckEligibility,
                    IsSpecialOffer = actionRequest.IsSpecialOffer,
                    PostData = actionRequest.PostData,
                    IsOmni = actionRequest.IsOmni,
                    RequiresOnlyLogin = actionRequest.Model.RequiresOnlyLogin
                };
                
                if (actionRequest.ActionButtonType == ActionButtonTypeEnum.Primary)
                {
                    productActionRequest.ActionConstant = ConstantsNeamb.Primary;
                }
                else
                {
                    productActionRequest.ActionConstant = ConstantsNeamb.Secondary;
                    productActionRequest.ActionString = "_sec";
                }
                var productActionResult = GetProductAction(statusUser, productActionRequest);
                if (productActionResult.HasError)
                {
                    return new ActionButtonResult
                    {
                        HasError = true
                    };
                }
                else
                {
                    actionRequest.IsOmniDefault= IsOmniLocal(actionRequest);
                    if (statusUser == StatusEnum.Hot || (!actionRequest.Model.HasCheckEligibility && !actionRequest.Model.RequiresOnlyLogin))
                    {
                        return GetActionHotUser(actionRequest, productActionResult, productActionRequest);
                    }
                    else if (statusUser == StatusEnum.WarmHot || statusUser == StatusEnum.WarmCold)
                    {
                        return GetActionWarmUser(actionRequest, productActionResult, productActionRequest,statusUser);
                    }
                    //Manage the cold user state
                    else if (statusUser == StatusEnum.Cold || statusUser == StatusEnum.Unknown)
                    {
                        return GetActionColdUser(actionRequest, productActionResult, productActionRequest);
                    }
                    else
                    {
                        _log.Warn("Error GetActionResult user status unknown", this);
                        return new ActionButtonResult
                        {
                            HasError = true
                        };
                    }
                }
            }
            else
            {
                return new ActionButtonResult
                {
                    HasError = true
                };
            }
        }

        private string GetActionType(ActionRequest actionRequest) {
			var typeAction = string.Empty;

			var actionType = actionRequest.RenderingItem[actionRequest.CtaTypeItemId];
			if (actionType != null && !string.IsNullOrEmpty(actionType)) {
				var actionTypeItem = Sitecore.Context.Database.GetItem(new ID(actionType));
                typeAction = actionTypeItem[Templates.CategoryItem.Fields.Value];
			}
            if (string.IsNullOrEmpty(typeAction) && actionRequest.IsOmni) {
                typeAction = "Omni";
            }
			return typeAction;
        }

		public ActionButton GetActionData(StatusEnum statusUser,ActionRequest actionRequest) {
			ActionButton actionButton = new ActionButton();
			//Get the link description for Primary action
            if (actionRequest.RenderingItem.FieldHasValue(actionRequest.CtaLinkItemId)) {
                LinkField link = actionRequest.RenderingItem.Fields[actionRequest.CtaLinkItemId];
                var target = string.Empty;

                actionButton.Description = link.Text;
                if (!string.IsNullOrEmpty(link.Target)) {
                    target = "_blank";
                }
                actionButton.Target = target;
            } else {
                _log.Warn("Error in GetActionData link is empty", this);
                actionButton.HasError = true;
            }
            return actionButton;
		}

        private ActionButtonResult GetActionColdUser(ActionRequest actionRequest, ProductAction productActionResult, ProductActionRequest productActionRequest) {
            string actionclick = "";
            ActionButtonResult actionResult = new ActionButtonResult
            {
                HasErrorLink = productActionResult.HasErrorLink
            };
            actionResult.Anonymous = SetAnonymousData(actionRequest.AnonymousItemId, actionRequest.RenderingItem, actionRequest.ProductName);
            string trackingFunction = GetGoalProductTracking(productActionRequest, actionRequest);
            actionResult.ActionClick = $"executeloginwarm('{actionRequest.ConstantActionType}{actionRequest.Model.ComponentId}');" +
                $"{trackingFunction}";
            _sessionManager.StoreInSession<string>(ConstantsNeamb.CtaActionWarmUser, ConstantsNeamb.UserCold);
            _sessionManager.StoreInSession($"{actionRequest.ConstantCtaAction}{actionRequest.Model.ComponentId}",
                !string.IsNullOrEmpty(productActionResult.ActionLink) ? productActionResult.ActionLink : string.Empty);
            if (!string.IsNullOrEmpty(productActionResult.ActionOnClickLink)) {
                actionclick = productActionResult.ActionOnClickLink.Replace(";return false", string.Empty);
                if (actionRequest.IsOmni) {
                    actionclick = actionclick.Replace(ConstantsNeamb.CheckOmni, "1");
                }
                else {
                    actionclick = actionclick.Replace(ConstantsNeamb.CheckOmni, "0");
                }
            }
            _sessionManager.StoreInSession($"{actionRequest.ConstantCtaActionOnclick}{actionRequest.Model.ComponentId}",actionclick);
            _sessionManager.StoreInSession($"{actionRequest.ConstantCtaActionTargetBlank}{actionRequest.Model.ComponentId}", actionRequest.TargetAction);
            _sessionManager.StoreInSession($"{ConstantsNeamb.ProductComponent}{actionRequest.Model.ComponentId}", actionRequest.ProductCode);
            _sessionManager.StoreInSession<bool>($"{ConstantsNeamb.ProductHasCheckEligibility}{actionRequest.Model.ComponentId}", actionRequest.HasCheckEligibility);
            
            string clickGtmAction = GetGtmAction(actionRequest, StatusEnum.Cold);
            _sessionManager.StoreInSession($"{actionRequest.ConstantProductGtmAction}{actionRequest.Model.ComponentId}", clickGtmAction);
            return actionResult;
        }

        /// <summary>
        /// Get the action tracking according the button primary or secondary
        /// </summary>
        /// <param name="productActionRequest">Product action request</param>
        /// <param name="actionRequest">action request</param>
        /// <returns></returns>
        private string GetGoalProductTracking(ProductActionRequest productActionRequest, ActionRequest actionRequest) {
            if (productActionRequest.ActionConstant.Equals(ConstantsNeamb.Primary)) {
                return $"trackingGoalProduct('{productActionRequest.ContextItem.ID}','{actionRequest.GoalPrimaryId}')";
            } else {
                return $"trackingGoalProduct('{productActionRequest.ContextItem.ID}','{actionRequest.GoalSecondaryId}')";
            }
        }

        private string GetGtmAction(ActionRequest actionRequest, StatusEnum statusUser) {
            string actionHref;
            if (statusUser == StatusEnum.WarmHot) {
                actionHref = _baseUrlColdUser.GetBaseUrlPartner();
            } else {
                Enum.TryParse(actionRequest.ActionType, out ActionTypeEnum actionTypeEnum);
                actionHref = GetBaseUrlPartner(actionTypeEnum, actionRequest.ProductCode, actionRequest.RenderingItem, actionRequest.CtaLinkItemId, actionRequest.IsOmniDefault);
            }

            ProductCtaBase productCta = new ProductCtaBase
            {
                ProductName = actionRequest.ProductName,
                CtaText = actionRequest.ActionDescription
            };

            return _productGtmManager.GetGtmFunction(actionRequest.ComponentType, actionRequest.RenderingItem, actionHref, productCta, statusUser);
        }

        private ActionButtonResult GetActionWarmUser(ActionRequest actionRequest, ProductAction productActionResult, ProductActionRequest productActionRequest, StatusEnum statusUser) {
            ActionButtonResult actionResult = new ActionButtonResult
            {
                HasErrorLink = productActionResult.HasErrorLink
            };
            if (!string.IsNullOrEmpty(actionRequest.UserName)) {
                _sessionManager.StoreInSession<string>(ConstantsNeamb.CtaActionWarmUser, actionRequest.UserName);
            }
            var linkText = actionRequest.RenderingItem.LinkFieldUrl(actionRequest.CtaLinkItemId);
            var postData = GetPostParams(actionRequest);
            //Case no required logged in the system (Link type primary)
            if (actionRequest.ActionType.Equals(ActionTypeEnum.Link.ToString()) &&
				!actionRequest.Model.HasCheckEligibility &&
                !actionRequest.Model.RequiresOnlyLogin &&
                !linkText.ToLower().Contains("[materialid]")
				&& !linkText.ToLower().Contains("[mdsid]")
				&& !linkText.ToLower().Contains("[mdsid_mercer]")
				&& !linkText.ToLower().Contains("[mdsid_clear]")
				&& !linkText.ToLower().Contains("[mdsid_afinium]")
				&& !linkText.ToLower().Contains("[cellcode]")
				&& !linkText.ToLower().Contains("[campcode]")
				&& !linkText.ToLower().Contains("[medium]")
                && !linkText.ToLower().Contains("[mercer]")
                && !linkText.ToLower().Contains("[afinium]")
                && !linkText.ToLower().Contains("[sob")
                && !linkText.ToLower().Contains("[gclid")
                && !linkText.ToLower().Contains("[term")
                && string.IsNullOrEmpty(postData)
                )
			{
				actionResult.ActionInner = linkText;
                actionResult.ActionClick = $"{actionResult.ActionClick}";

                string clickGtmAction = GetGtmAction(actionRequest, StatusEnum.Unknown);
                if (!string.IsNullOrEmpty(clickGtmAction))
                {
                    actionResult.ActionClick = $"{clickGtmAction}{actionResult.ActionClick}";
                }
            }
			else if (!string.IsNullOrEmpty(actionRequest.ActionType))
			{
                string trackingFunction = GetGoalProductTracking(productActionRequest, actionRequest);

                if (statusUser == StatusEnum.WarmHot) {

                    _sessionManager.StoreInSession($"{actionRequest.ConstantCtaAction}{actionRequest.Model.ComponentId}",
                        !string.IsNullOrEmpty(productActionResult.ActionLink) ? productActionResult.ActionLink : string.Empty);
                    _sessionManager.StoreInSession($"{actionRequest.ConstantCtaActionOnclick}{actionRequest.Model.ComponentId}",
                        !string.IsNullOrEmpty(productActionResult.ActionOnClickLink)
                            ? productActionResult.ActionOnClickLink.Replace(";return false", string.Empty).Replace(ConstantsNeamb.CheckOmni, actionRequest.IsOmniDefault)
                            : string.Empty);
                    _sessionManager.StoreInSession($"{actionRequest.ConstantCtaActionTargetBlank}{actionRequest.Model.ComponentId}", actionRequest.TargetAction);
                    _sessionManager.StoreInSession<bool>($"{ConstantsNeamb.ProductHasCheckEligibility}{actionRequest.Model.ComponentId}",
                        actionRequest.HasCheckEligibility);
                    
                    string clickGtmActionWarm = GetGtmAction(actionRequest, statusUser);
                    actionResult.ActionClick += $"executeloginwarm('{actionRequest.ConstantActionType}{actionRequest.Model.ComponentId}');" +
                        $"{trackingFunction}";

                    if (!string.IsNullOrEmpty(clickGtmActionWarm))
                    {
                        actionResult.ActionClick = $"{clickGtmActionWarm}{actionResult.ActionClick}";
                    }
                    string clickGtmActionHot = GetGtmAction(actionRequest, StatusEnum.Hot);
                    _sessionManager.StoreInSession($"{actionRequest.ConstantProductGtmAction}{actionRequest.Model.ComponentId}", clickGtmActionHot);
                } else {
                    actionResult.ActionClick += $"executeloginwarm('{actionRequest.ConstantActionType}{actionRequest.Model.ComponentId}');" +
                        $"{trackingFunction}";
                }
                
            }
            else {
				actionResult.HasError = true;
			}
			return actionResult;
		}

		private ActionButtonResult GetActionHotUser(ActionRequest actionRequest, ProductAction productActionResult, ProductActionRequest productActionRequest) {
            ActionButtonResult actionResult = new ActionButtonResult
            {
                HasErrorLink = productActionResult.HasErrorLink
            };
            actionResult.ActionInner = productActionResult.ActionLink;
			actionResult.ActionClick = productActionResult.ActionOnClickLink?.Replace(ConstantsNeamb.CheckOmni, actionRequest.IsOmniDefault);

            var clickGtmAction = GetGtmAction(actionRequest, StatusEnum.Hot);
            string trackingFunction = GetGoalProductTracking(productActionRequest, actionRequest);
            if (!string.IsNullOrEmpty(clickGtmAction) || !string.IsNullOrEmpty(trackingFunction))
            {
                actionResult.ActionClick = $"{clickGtmAction}{trackingFunction};{actionResult.ActionClick}";
            }

            return actionResult;
		}

        private string IsOmniLocal(ActionRequest actionRequest) {
            if (actionRequest.ComponentType == ComponentTypeEnum.CtaSecondary) {
                return "0";
            }
            if (actionRequest.IsOmni && _pageSitecoreContext.Current != actionRequest.RenderingItem) {
                return "1";
            }
            if (actionRequest.IsOmni) {
                var pageItem = _pageSitecoreContext.Current;

                //Grab the field that contains the final layout
                var finalLayoutField = new LayoutField(pageItem.Fields[Sitecore.FieldIDs.FinalLayoutField]);

                if (!string.IsNullOrWhiteSpace(finalLayoutField.Value)) {
                    var finalLayoutDefinition = LayoutDefinition.Parse(finalLayoutField.Value);
                    var xml = finalLayoutDefinition.ToXml();
                    TextReader sr = new StringReader(xml);

                    XElement root = XElement.Load(sr, LoadOptions.None);

                    sr.Close();
                    var elementCondition =
                        (from el in root.Descendants("condition")
                            where (string) el.Attribute("id") == "{BEBA9398-89F2-4307-85E2-F2023ED2AC45}"
                            select el).FirstOrDefault();
                    if (elementCondition == null) {
                        return "1";
                    } else {
                        return "0";
                    }
                }
                return "0";
            }
            return "0";
        }

        private string GetBaseUrlPartner(ActionTypeEnum actionTypeEnum, string productCode, Item renderingItem, ID ctaLinkItemId, string isOmniDefault) {
            string urlBase="";
            if (isOmniDefault.Equals("1")) {
                urlBase= _baseUrlActionOmni.GetBaseUrlPartner(productCode);
            }

            if (string.IsNullOrEmpty(urlBase)) {
                switch (actionTypeEnum) {
                    case ActionTypeEnum.DataPass:
                    case ActionTypeEnum.SingleSignOn: {
                        urlBase= _baseUrlActionDatapass.GetBaseUrlPartner(productCode);
                        break;
                    }
                    case ActionTypeEnum.Link: {
                        urlBase= _baseUrlActionLink.GetBaseUrlPartner(renderingItem, ctaLinkItemId);
                        break;
                    }
                    case ActionTypeEnum.Efulfillment: {
                        urlBase= _baseUrlActionEfulfillment.GetBaseUrlPartner();
                        break;
                    }
                    case ActionTypeEnum.Omni: {
                        urlBase= _baseUrlActionOmni.GetBaseUrlPartner(productCode);
                        break;
                    }
                    default: {
                        urlBase= "";
                        break;
                    }
                }
            }
            return urlBase;
        }

        /// <summary>
        /// Set the actions for primary button
        /// </summary>
        private ProductAction GetProductAction(StatusEnum statusUser, ProductActionRequest productActionRequest)
        {
            var productAction = new ProductAction();
            var componentType = productActionRequest.IsSpecialOffer ? (int)ComponentTypeEnum.SpecialOffer : (int)ComponentTypeEnum.None;
            if (productActionRequest.ActionType.Equals(ActionTypeEnum.Omni.ToString())) 
            {
                var methodLink = $"executelinkOmni{productActionRequest.ActionString}{productActionRequest.ComponentId}";
                productAction.ActionOnClickLink =
                    $"{methodLink}('{productActionRequest.ContextItem.ID}'," +
                    $"'{productActionRequest.ProductCode}','{productActionRequest.EligibilityItemId}');" +
                    $"operationprocedureactioncta('{productActionRequest.ProductCode}');return false";
            }

            //Type Link
           else  if (productActionRequest.ActionType.Equals(ActionTypeEnum.Link.ToString()))
            {
                var urlLink = productActionRequest.ContextItem.LinkFieldUrl(productActionRequest.CtaLinkItemId).Trim();
                if (productActionRequest.RequiresOnlyLogin || productActionRequest.HasCheckEligibility || urlLink.Contains("[materialid]") || urlLink.Contains("[mdsid]") ||
                    urlLink.Contains("[mdsid_mercer]") || urlLink.Contains("[mdsid_clear]") ||
                    urlLink.Contains("[mdsid_afinium]") || urlLink.Contains("[cellcode]") || urlLink.Contains("[campcode]") || urlLink.Contains("[medium]") || urlLink.Contains("[mercer]") || urlLink.Contains("[afinium]") || urlLink.Contains("[sob") || urlLink.Contains("[gclid") || urlLink.Contains("[term") || !string.IsNullOrEmpty(productActionRequest.PostData))
                {
                    var methodLink = $"executelink{productActionRequest.ActionString}{productActionRequest.ComponentId}";

                    productAction.ActionOnClickLink =
                        $"{methodLink}('{productActionRequest.CtaLinkItemId}','{productActionRequest.ContextItem.ID}'," +
                        $"'{productActionRequest.ProductCode}','{productActionRequest.EligibilityItemId}'," +
                        $"'{ConstantsNeamb.CheckOmni}', {productActionRequest.PostData});" +
                        $"operationprocedureactioncta('{productActionRequest.ProductCode}');return false";
                    if (!productActionRequest.HasCheckEligibility && (statusUser == StatusEnum.Cold || statusUser == StatusEnum.Unknown))
                    {
                        productAction.HasErrorLink = true;
                    }
                }
                else
                {
                    productAction.ActionLink = productActionRequest.ContextItem.LinkFieldUrl(productActionRequest.CtaLinkItemId).Trim();
                }
            }
            //Type datapass
            else if (productActionRequest.ActionType.Equals(ActionTypeEnum.DataPass.ToString()))
            {
                var methodDatapass = $"executedatapass{productActionRequest.ActionString}{productActionRequest.ComponentId}";

                productAction.ActionOnClickLink =
                    $"{methodDatapass}('{productActionRequest.ProductCode}', '{componentType}','{ConstantsNeamb.CheckOmni}');operationprocedureactioncta('{productActionRequest.ProductCode}');return false";
            }
            //Type Efulfillment
            else if (productActionRequest.ActionType.Equals(ActionTypeEnum.Efulfillment.ToString()))
            {

                var methodDownload = $"downloadpdf{productActionRequest.ActionString}{productActionRequest.ComponentId}";
                var actionEfullfilment = ActionEfullfilment(methodDownload, productActionRequest.ProductCode, productActionRequest.ActionType, productActionRequest.ComponentId);
                if (String.IsNullOrEmpty(actionEfullfilment))
                {
                    productAction.HasError = true;
                }
                else
                {
                    productAction.ActionOnClickLink = actionEfullfilment;
                }
            }
            //Type Single Sign On
            else if (productActionRequest.ActionType.Equals(ActionTypeEnum.SingleSignOn.ToString()))
            {

                var methodSingleSignOn = $"executesinglesignon{productActionRequest.ActionString}{productActionRequest.ComponentId}";
                productAction.ActionOnClickLink =
                    $"{methodSingleSignOn}('{productActionRequest.ProductCode}','{componentType}','{ConstantsNeamb.CheckOmni}');operationprocedureactioncta('{productActionRequest.ProductCode}');return false";
            }
            else
            {
                productAction.HasError = true;
            }

            return productAction;
        }

        private string GetPostParams(ActionRequest actionRequest) {
            var parameters = new Dictionary<string, string>();

            try {
                _log.Debug($"GetPostParams PostDataItemId: {actionRequest.PostDataItemId}", this);

                var rawText = actionRequest.RenderingItem[actionRequest.PostDataItemId] ?? string.Empty;
                var text = rawText.Replace("{", String.Empty).Replace("}", String.Empty);

                foreach (var entry in text.Split(new[] { "\r\n" }, StringSplitOptions.None))
                {
                    var parameter = entry.Split(new[] { ":" }, StringSplitOptions.None);
                    if (parameter.Length == 2)
                        parameters.Add(parameter.First(), parameter.Last());
                }
            }
            catch (Exception e) {
                _log.Error("Error in GetPostParams: ", e, this);
            }
            
            var result = string.Join(",", parameters.Select(x => $"{x.Key}: '{x.Value}'").ToArray());
            return "{" + result + "}";
        }

        /// <summary>
        /// Build the path to be executed in efulfillment
        /// </summary>
        /// <param name="functionName">JS function</param>
        /// <param name="productCode">Program code</param>
        /// <param name="action">Product type</param>
        /// <param name="componentId">ComponentId</param>
        /// <returns></returns>
        private string ActionEfullfilment(string functionName, string productCode, string action, string componentId)
		{
			var materialId = _oracleManager.SelectItemCodeForProductCode(productCode);
			if (!string.IsNullOrEmpty(materialId))
			{

				var actionEfullfilment =
					$"{functionName}('{materialId}', '{productCode}','{action}{componentId}','{ConstantsNeamb.CheckOmni}');return false";
				return actionEfullfilment;
			}
			return string.Empty;
		}

		public Anonymous SetAnonymousData(ID anonymousItemId, Item renderingItem, string productName) {
			Anonymous anonymous= new Anonymous();
			if (anonymousItemId != ID.Null && anonymousItemId != ID.Undefined)
			{
				LinkField anonymousLink = renderingItem.Fields[anonymousItemId];
				ProductCtaBase productGtm = new ProductCtaBase { ProductName = productName, CtaText = anonymousLink.Text };
				if (anonymousLink != null)
				{
					anonymous.Text = anonymousLink.Text;
					anonymous.Url = renderingItem.LinkFieldUrl(anonymousItemId);
					string actionHref = _baseUrlColdUser.GetBaseUrlPartner();
					anonymous.GtmAction = _productGtmManager.GetGtmFunction(ComponentTypeEnum.Anonymous, renderingItem, actionHref, productGtm);
				}
			}
			return anonymous;
		}

	}
}