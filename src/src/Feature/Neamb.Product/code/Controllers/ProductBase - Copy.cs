using System;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Product.Interfaces;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Config.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Neamb.Foundation.Product.Pipelines;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Links;
using Sitecore.Mvc.Extensions;

namespace Neambc.Neamb.Feature.Product.Controllers
{
	public abstract class ProductBase1Controller : Controller
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
		protected ID _productCodeItemId;
		protected ID _primaryCtaTypeItemId;
		protected ID _primaryCtaLinkItemId;
		protected ID _secondaryCtaTypeItemId;
		protected ID _secondaryCtaLinkItemId;
		protected bool _isSpecialOffer;
		private string _componentId;
		private string _queryparameters;
		private string _productName;
		protected IProductGtmManager _productGtmManager;
		protected ComponentTypeEnum _componentType;
		protected ID _anonymousItemId;
		protected ID _reminderCtaId;
		protected IGlobalConfigurationManager _globalConfigurationManager;


		/// <summary>
		/// Main entry to get the cta actions
		/// </summary>
		/// <param name="model">Model</param>
		/// <param name="renderingItem">Rendering item</param>
		/// <param name="accountMembership">User data logged</param>
		public void ProcessEligibilityActions(ProductDetailDTO model, Item renderingItem, AccountMembership accountMembership)
		{
			//Get the product code value
			var productCode = renderingItem[_productCodeItemId];

			_productName = !String.IsNullOrEmpty(renderingItem.DisplayName) ? renderingItem.DisplayName : renderingItem.Name;
			if (_anonymousItemId != ID.Null && _anonymousItemId != ID.Undefined) {
				LinkField anonymousLink = renderingItem.Fields[_anonymousItemId];
				ProductCta productGtm = new ProductCta { ProductName = _productName, CtaText = anonymousLink.Text};
				if (anonymousLink != null) {
					model.AnonymousText = anonymousLink.Text;
					model.AnonymousUrl = renderingItem.LinkFieldUrl(_anonymousItemId);
					string actionHref = GetBaseUrlPartner(productCode, ActionTypeEnum.None, renderingItem, ID.Null);
					model.AnonymousGtmAction = _productGtmManager.GetGtmFunction(ComponentTypeEnum.Anonymous, renderingItem, actionHref, productGtm);
				}
			}
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
			//The user is authenticated
			var isAuthenticated = accountMembership.Status == StatusEnum.Hot ||
								  accountMembership.Status == StatusEnum.WarmCold ||
								  accountMembership.Status == StatusEnum.WarmHot;
			//Set the user status in the model
			model.UserStatus = accountMembership.Status;

			if (model.HasCheckEligibility && isAuthenticated)
			{
				SetPropertiesEligibility(accountMembership, productCode, model);
			}

			if (model.HasCommingSoon)
			{
				SetPropertiesCommingSoon(accountMembership, productCode, model,renderingItem);
			}
			else
			{
				ExecuteProcessAction(model, renderingItem, accountMembership, productCode, isAuthenticated);
			}
		}

		/// <summary>
		/// Set some properties used in comming soon module
		/// </summary>
		/// <param name="accountMembership">User logged data</param>
		/// <param name="productCode">Product code</param>
		/// <param name="model">Model</param>
		/// <param name="renderingItem">Rendering item</param>
		private void SetPropertiesCommingSoon(AccountMembership accountMembership, string productCode,
			ProductDetailDTO model, Item renderingItem)
		{
			var logCount = _oracleManager.ReminderLogCount(productCode, accountMembership.Mdsid);
			if ((logCount > 0) && accountMembership.Status != StatusEnum.Cold && accountMembership.Status != StatusEnum.Unknown)
			{
				//It is already notified
				model.HasAlreadyNotified = true;
			}
			else
			{
				var notifyAction = $"notifyproductavailablewarm('{productCode}');return false";
				string reminderCta = _reminderCtaId != ID.Null && _reminderCtaId != ID.Undefined ? renderingItem[_reminderCtaId] : "";
				ProductCta productGtm = new ProductCta { ProductName = _productName, CtaText = reminderCta };

				if (accountMembership.Status == StatusEnum.Cold || accountMembership.Status == StatusEnum.Unknown)
				{
					//Set the redirection to login
					var pathLoginPage = LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID));
					model.NotifyProductAvailableLink = pathLoginPage;
					if (_reminderCtaId != ID.Null && _reminderCtaId != ID.Undefined) {
						string actionHref = GetBaseUrlPartner(productCode, ActionTypeEnum.None, renderingItem, ID.Null);
						string clickGtmAction = _productGtmManager.GetGtmFunction(ComponentTypeEnum.Anonymous, renderingItem, actionHref, productGtm);
						if (!string.IsNullOrEmpty(clickGtmAction)) {
							model.NotifyProductAvailableAction = $"{clickGtmAction}{model.NotifyProductAvailableAction}";
						}
					}
				}
				else
				{
					//The user is authenticated
					if (accountMembership.Status == StatusEnum.Hot)
					{
						//Build or set the javascript to notify the action
						model.NotifyProductAvailableAction = $"notifyproductavailable{_componentId}('{productCode}');return false";
					}
					else
					{
						_sessionManager.StoreInSession<string>($"{ConstantsNeamb.CtaActionPrimary}{model.ComponentId}", string.Empty);
						_sessionManager.StoreInSession<string>(
							$"{ConstantsNeamb.CtaActionPrimaryOnclick}{model.ComponentId}",
							!string.IsNullOrEmpty(notifyAction) ? notifyAction.Replace(";return false", string.Empty) : string.Empty);
						_sessionManager.StoreInSession<string>(
							$"{ConstantsNeamb.CtaActionPrimaryTargetBlank}{model.ComponentId}", "_blank");
						model.NotifyProductAvailableAction = string.Format("executeloginwarm('{0}{1}')", ConstantsNeamb.Primary, model.ComponentId);
						//Execute login when the user is warm hot
						_sessionManager.StoreInSession<string>(ConstantsNeamb.CtaActionWarmUser, accountMembership.Username);
					}
					if (_reminderCtaId != ID.Null && _reminderCtaId != ID.Undefined) {
						string actionHref = GetBaseUrlPartner(productCode, ActionTypeEnum.ComingSoon, renderingItem, ID.Null);
						string clickGtmAction = _productGtmManager.GetGtmFunction(_componentType, renderingItem, actionHref, productGtm);
						if (!string.IsNullOrEmpty(clickGtmAction)) {
							model.NotifyProductAvailableAction = $"{clickGtmAction}{model.NotifyProductAvailableAction}";
						}
					}
				}
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
			//Get the result of eligibility from the webservice
			var resultEligibility = _eligibilityManager.IsMemberEligible(accountMembership.Mdsid, productCode);
			//Check the result of the webservice call
			if (resultEligibility == EligibilityResultEnum.Eligible)
			{
				model.ResultCheckEligibility = true;
			}
			else
			{
				//Get some values for chat,phone,mail to be displayed when it is not eligible
				var siteSettings = Sitecore.Context.Database.GetItem(Templates.SiteSettings.ID);
				model.PhoneNumber = siteSettings[Templates.SiteSettings.Fields.Phone];
				model.SupportEmail = siteSettings[Templates.SiteSettings.Fields.Email];
				model.ResultCheckEligibility = false;
			}
		}

		/// <summary>
		/// Verify the actions configured in Primary and Secondary actions
		/// </summary>
		/// <param name="model">Model</param>
		/// <param name="renderingItem">Item rendering</param>
		/// <param name="accountMembership">User logged data</param>
		/// <param name="productCode">Product code</param>
		/// <param name="isAuthenticated">Flag when the user is authenticated</param>
		private void ExecuteProcessAction(
			ProductDetailDTO model,
			Item renderingItem,
			AccountMembership accountMembership,
			string productCode,
			bool isAuthenticated
		) {
			var typePrimary = string.Empty;
			var typeSecondary = string.Empty;

			//Get type Primary action
			var actionPrimaryType = renderingItem[_primaryCtaTypeItemId];
			if (actionPrimaryType != null && !string.IsNullOrEmpty(actionPrimaryType)) {
				var actionPrimaryTypeItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(actionPrimaryType));
				typePrimary = actionPrimaryTypeItem[Templates.CategoryItem.Fields.Value];
			}

			if (_secondaryCtaTypeItemId != ID.Null) {
				//Get type Secondary action
				var actionSecondaryType = renderingItem[_secondaryCtaTypeItemId];
				if (actionSecondaryType != null && !string.IsNullOrEmpty(actionSecondaryType)) {
					var actionSecondaryTypeItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(actionSecondaryType));
					typeSecondary = actionSecondaryTypeItem[Templates.CategoryItem.Fields.Value];
				}
			}

			//Get the link description for Primary action
			LinkField primaryLink = renderingItem.Fields[_primaryCtaLinkItemId];
			var targetPrimary = string.Empty;
			if (primaryLink != null) {
				model.ActionPrimaryDescription = primaryLink.Text;
				if (!string.IsNullOrEmpty(primaryLink.Target)) {
					targetPrimary = "_blank";
					if (accountMembership.Status != StatusEnum.WarmHot && accountMembership.Status != StatusEnum.WarmCold) {
						model.ActionPrimaryTargetBlank = targetPrimary;
					}
				}
			}
			var targetSecondary = string.Empty;

			//Get the link description for Secondary action
			if (_secondaryCtaLinkItemId != ID.Null) {
				LinkField secondaryLink = renderingItem.Fields[_secondaryCtaLinkItemId];
				if (secondaryLink != null) {
					model.ActionSecondaryDescription = secondaryLink.Text;
					if (!string.IsNullOrEmpty(secondaryLink.Target)) {
						targetSecondary = "_blank";
						if (accountMembership.Status != StatusEnum.WarmHot && accountMembership.Status != StatusEnum.WarmCold) {
							model.ActionSecondaryTargetBlank = targetSecondary;
						}
					}
				}
			}
			//Execute login when the user is warm hot
			if (accountMembership.Status == StatusEnum.WarmHot || accountMembership.Status == StatusEnum.WarmCold) {
				var productActionPrimary = CheckPrimaryActions(typePrimary, productCode, model.HasCheckEligibility, renderingItem, isAuthenticated);
				_sessionManager.StoreInSession<string>(ConstantsNeamb.CtaActionWarmUser, accountMembership.Username);

				var linkText = renderingItem.LinkFieldUrl(_primaryCtaLinkItemId);

				//Case no required logged in the system (Link type primary)
				if (typePrimary.Equals(ActionTypeEnum.Link.ToString()) &&
					!model.HasCheckEligibility &&
					!linkText.ToLower().Contains("[materialid]") 
					&& !linkText.ToLower().Contains("[mdsid]")
					&& !linkText.ToLower().Contains("[mdsid_mercer]")
					&& !linkText.ToLower().Contains("[mdsid_clear]")
					&& !linkText.ToLower().Contains("[mdsid_afinium]")
					&& !linkText.ToLower().Contains("[cellcode]")
					&& !linkText.ToLower().Contains("[campcode]")
					&& !linkText.ToLower().Contains("[medium]")
					)
				{
					model.ActionPrimary = renderingItem.LinkFieldUrl(_primaryCtaLinkItemId);

					ProductCta productGtm = new ProductCta { ProductName = _productName, CtaText = model.ActionPrimaryDescription };
					Enum.TryParse(typePrimary, out ActionTypeEnum actionTypeEnum);
					string actionHref = GetBaseUrlPartner(productCode, actionTypeEnum, renderingItem, _primaryCtaLinkItemId);
					string clickGtmAction = _productGtmManager.GetGtmFunction(_componentType, renderingItem, actionHref, productGtm);

					if (!string.IsNullOrEmpty(clickGtmAction))
					{
						model.ActionClickPrimary = $"{clickGtmAction}{model.ActionClickPrimary}";
					}
				} else if (!string.IsNullOrEmpty(typePrimary)) {
					_sessionManager.StoreInSession($"{ConstantsNeamb.CtaActionPrimary}{model.ComponentId}",
						!string.IsNullOrEmpty(productActionPrimary.ActionLink) ? productActionPrimary.ActionLink : string.Empty);
					_sessionManager.StoreInSession($"{ConstantsNeamb.CtaActionPrimaryOnclick}{model.ComponentId}",
						!string.IsNullOrEmpty(productActionPrimary.ActionOnClickLink) ? productActionPrimary.ActionOnClickLink.Replace(";return false", string.Empty) : string.Empty);
					_sessionManager.StoreInSession($"{ConstantsNeamb.CtaActionPrimaryTargetBlank}{model.ComponentId}", targetPrimary);

					model.ActionClickPrimary = $"executeloginwarm('{ConstantsNeamb.Primary}{model.ComponentId}')";
					ProductCta productGtm = new ProductCta { ProductName = _productName, CtaText = model.ActionPrimaryDescription };
					string actionHref = GetBaseUrlPartner(productCode, ActionTypeEnum.None, renderingItem, ID.Null);
					string clickGtmAction = _productGtmManager.GetGtmFunction(_componentType, renderingItem, actionHref, productGtm);
					if (!string.IsNullOrEmpty(clickGtmAction))
					{
						model.ActionClickPrimary = $"{clickGtmAction}{model.ActionClickPrimary}";
					}
				}

				//Case no required logged in the system (Link type secondary)

				if (_secondaryCtaLinkItemId != ID.Null) {
					var productActionSecondary = CheckSecondaryActions(typeSecondary, productCode, renderingItem, isAuthenticated, model.HasCheckEligibility);

					if (typeSecondary.Equals(ActionTypeEnum.Link.ToString()) && !model.HasCheckEligibility
						&& !linkText.ToLower().Contains("[materialid]")
						&& !linkText.ToLower().Contains("[mdsid]")
						&& !linkText.ToLower().Contains("[mdsid_mercer]")
						&& !linkText.ToLower().Contains("[mdsid_clear]")
						&& !linkText.ToLower().Contains("[mdsid_afinium]")
						&& !linkText.ToLower().Contains("[cellcode]")
						&& !linkText.ToLower().Contains("[campcode]")
						&& !linkText.ToLower().Contains("[medium]")
					) {
						model.ActionSecondary = renderingItem.LinkFieldUrl(_secondaryCtaLinkItemId);

						ProductCta productGtm = new ProductCta { ProductName = _productName, CtaText = model.ActionSecondaryDescription};
						Enum.TryParse(typeSecondary, out ActionTypeEnum actionTypeEnum);
						string actionHref = GetBaseUrlPartner(productCode, actionTypeEnum, renderingItem, _secondaryCtaLinkItemId);
						string clickGtmAction = _productGtmManager.GetGtmFunction(ComponentTypeEnum.CtaSecondary, renderingItem, actionHref, productGtm);

						if (!string.IsNullOrEmpty(clickGtmAction))
						{
							model.ActionClickSecondary = $"{clickGtmAction}{model.ActionClickSecondary}";
						}

					} else if (!string.IsNullOrEmpty(typeSecondary)) {
						_sessionManager.StoreInSession<string>(string.Format("{0}{1}", ConstantsNeamb.CtaActionSecondary, model.ComponentId),
							!string.IsNullOrEmpty(productActionSecondary.ActionLink) ? productActionSecondary.ActionLink : string.Empty);
						_sessionManager.StoreInSession<string>(string.Format("{0}{1}", ConstantsNeamb.CtaActionSecondaryOnclick, model.ComponentId),
							!string.IsNullOrEmpty(productActionSecondary.ActionOnClickLink)
								? productActionSecondary.ActionOnClickLink.Replace(";return false", string.Empty)
								: string.Empty);
						_sessionManager.StoreInSession<string>(string.Format("{0}{1}", ConstantsNeamb.CtaActionSecondaryTargetBlank, model.ComponentId), targetSecondary);
						model.ActionClickSecondary = string.Format("executeloginwarm('{0}{1}')", ConstantsNeamb.Secondary, model.ComponentId);
						string actionHref = GetBaseUrlPartner(productCode, ActionTypeEnum.None, renderingItem, ID.Null);
						ProductCta productGtm = new ProductCta { ProductName = _productName, CtaText = model.ActionSecondaryDescription };
						string clickGtmAction =_productGtmManager.GetGtmFunction(ComponentTypeEnum.CtaSecondary, renderingItem, actionHref, productGtm);
						if (!string.IsNullOrEmpty(clickGtmAction))
						{
							model.ActionClickSecondary = $"{clickGtmAction}{model.ActionClickSecondary}";
						}
					}
				}
			} else {
				var productActionPrimary =
					CheckPrimaryActions(typePrimary, productCode, model.HasCheckEligibility, renderingItem, isAuthenticated);
				
				model.ActionPrimary = productActionPrimary.ActionLink;
				model.ActionClickPrimary = productActionPrimary.ActionOnClickLink;
				ProductCta productGtmPrimary = new ProductCta { ProductName = _productName, CtaText = model.ActionPrimaryDescription };
				Enum.TryParse(typePrimary, out ActionTypeEnum actionTypeEnum);
				string actionHref = GetBaseUrlPartner(productCode, actionTypeEnum, renderingItem, _primaryCtaLinkItemId);
				string clickGtmAction = _productGtmManager.GetGtmFunction(_componentType, renderingItem, actionHref, productGtmPrimary);
				if (!string.IsNullOrEmpty(clickGtmAction)) {
					model.ActionClickPrimary = $"{clickGtmAction}{model.ActionClickPrimary}";
				}

				if (_secondaryCtaLinkItemId != ID.Null) {
					var productActionSecondary =
						CheckSecondaryActions(typeSecondary, productCode, renderingItem, isAuthenticated, model.HasCheckEligibility);
					model.ActionSecondary = productActionSecondary.ActionLink;
					model.ActionClickSecondary = productActionSecondary.ActionOnClickLink;
					ProductCta productGtmSec = new ProductCta { ProductName = _productName, CtaText = model.ActionSecondaryDescription };
					Enum.TryParse(typeSecondary, out ActionTypeEnum actionTypeEnumSec);
					string actionHrefSec = GetBaseUrlPartner(productCode, actionTypeEnumSec, renderingItem, _secondaryCtaLinkItemId);
					string clickGtmActionSec = _productGtmManager.GetGtmFunction(ComponentTypeEnum.CtaSecondary, renderingItem, actionHrefSec,productGtmSec);
					if (!string.IsNullOrEmpty(clickGtmActionSec)) {
						model.ActionClickSecondary = $"{clickGtmActionSec}{model.ActionClickSecondary}";
					}
				}
			}
		}

		/// <summary>
		/// Set the actions for primary button
		/// </summary>
		/// <param name="typePrimary">Primary type</param>
		/// <param name="productCode">Product code</param>
		/// <param name="hasCheckEligibility">Flag to check the eligibility</param>
		/// <param name="contextItem">Item rendering</param>
		/// <param name="isAuthenticated">Flag for authenticated user</param>
		private ProductAction CheckPrimaryActions(string typePrimary, string productCode, bool hasCheckEligibility,
			Item contextItem, bool isAuthenticated)
		{
			var productAction = new ProductAction();
			var componentType = _isSpecialOffer ? (int)ComponentTypeEnum.SpecialOffer : (int)ComponentTypeEnum.None;
			//Type Link
			if (typePrimary.Equals(ActionTypeEnum.Link.ToString()))
			{
				var urlLink = contextItem.LinkFieldUrl(_primaryCtaLinkItemId).Trim();
				if (hasCheckEligibility || urlLink.Contains("[materialid]") || urlLink.Contains("[mdsid]") ||
					urlLink.Contains("[mdsid_mercer]") || urlLink.Contains("[mdsid_clear]") ||
					urlLink.Contains("[mdsid_afinium]") || urlLink.Contains("[cellcode]") || urlLink.Contains("[campcode]") || urlLink.Contains("[medium]"))
				{
					var methodLink = $"executelink{_componentId}";

					productAction.ActionOnClickLink =
						$"{methodLink}('{_primaryCtaLinkItemId}','{contextItem.ID}'," +
						$"'{productCode}','{_eligibilityItemId}'," +
						$"'{ConstantsNeamb.Primary}{_componentId}');" +
						$"operationprocedureactioncta('{productCode}');return false";

				}
				else
				{
					productAction.ActionLink = contextItem.LinkFieldUrl(_primaryCtaLinkItemId).Trim();
				}
			}
			//Type datapass
			else if (typePrimary.Equals(ActionTypeEnum.DataPass.ToString()) && isAuthenticated)
			{

				var methodDatapass = string.Format("executedatapass{0}", _componentId);

				productAction.ActionOnClickLink =
					$"{methodDatapass}('{productCode}', '{componentType}');operationprocedureactioncta('{productCode}');return false";
			}
			//Type Efulfillment
			else if (typePrimary.Equals(ActionTypeEnum.Efulfillment.ToString()) && isAuthenticated)
			{

				var methodDownload = string.Format("downloadpdf{0}", _componentId);
				var actionEfullfilment = ActionEfullfilment(methodDownload, productCode, ConstantsNeamb.Primary);
				productAction.ActionOnClickLink = actionEfullfilment;
			}
			//Type Single Sign On
			else if (typePrimary.Equals(ActionTypeEnum.SingleSignOn.ToString()) && isAuthenticated)
			{

				var methodSingleSignOn = string.Format("executesinglesignon{0}", _componentId);
				productAction.ActionOnClickLink =
					$"{methodSingleSignOn}('{productCode}','{componentType}'); operationprocedureactioncta('{productCode}');return false";
			}

			return productAction;
		}

		/// <summary>
		/// Set the actions for secondary button
		/// </summary>
		/// <param name="typeSecondary">Secondary type</param>
		/// <param name="productCode">Product code</param>
		/// <param name="contextItem">Item rendering</param>
		/// <param name="isAuthenticated">Flag for authenticated user</param>
		/// <param name="hasCheckEligibility">Flag to check eligibility</param>
		private ProductAction CheckSecondaryActions(string typeSecondary, string productCode, Item contextItem, bool isAuthenticated, bool hasCheckEligibility)
		{
			var productAction = new ProductAction();
			if (typeSecondary.Equals(ActionTypeEnum.Link.ToString()))
			{
				var urlLink = contextItem.LinkFieldUrl(_secondaryCtaLinkItemId).Trim();

				if (((urlLink.ToLower().Contains("[materialid]") || urlLink.ToLower().Contains("[mdsid]") ||
					  urlLink.Contains("[mdsid_mercer]") || urlLink.Contains("[mdsid_clear]") ||
					  urlLink.Contains("[mdsid_afinium]") || urlLink.Contains("[cellcode]") || urlLink.Contains("[campcode]") || urlLink.Contains("[medium]")) &&
					 isAuthenticated) || hasCheckEligibility)
				{
					var methodLink = string.Format("executelink_sec{0}", _componentId);

					productAction.ActionOnClickLink =
						$"{methodLink}('{_secondaryCtaLinkItemId.ToString()}','{contextItem.ID}','{productCode}','{_eligibilityItemId.ToString()}','{ConstantsNeamb.Secondary}{_componentId}');operationprocedureactioncta('{productCode}');return false";
				}
				else
				{
					productAction.ActionLink = contextItem.LinkFieldUrl(_secondaryCtaLinkItemId).Trim();
				}
			}
			else if (typeSecondary.Equals(ActionTypeEnum.DataPass.ToString()) && isAuthenticated)
			{
				var method = $"executedatapass_sec{_componentId}";

				productAction.ActionOnClickLink =
					$"{method}('{productCode}','{(int)ComponentTypeEnum.None}');operationprocedureactioncta('{productCode}');return false";
			}
			else if (typeSecondary.Equals(ActionTypeEnum.Efulfillment.ToString()) && isAuthenticated)
			{
				var methodDownload = $"downloadpdf_sec{_componentId}";
				var actionEfullfilment = ActionEfullfilment(methodDownload, productCode, ConstantsNeamb.Secondary);
				productAction.ActionOnClickLink = actionEfullfilment;
			}
			else if (typeSecondary.Equals(ActionTypeEnum.SingleSignOn.ToString()) && isAuthenticated)
			{
				var methodSingleSignOn = $"executesinglesignon_sec{_componentId}";
				productAction.ActionOnClickLink =
					$"{methodSingleSignOn}('{productCode}','{(int)ComponentTypeEnum.None}'); operationprocedureactioncta('{productCode}');return false";
			}
			return productAction;
		}

		/// <summary>
		/// Build the path to be executed in efulfillment
		/// </summary>
		/// <param name="functionName">JS function</param>
		/// <param name="productCode">Program code</param>
		/// <param name="action">Product type</param>
		/// <returns></returns>
		private string ActionEfullfilment(string functionName, string productCode, string action)
		{
			var materialId = _oracleManager.SelectItemCodeForProductCode(productCode);
			if (!string.IsNullOrEmpty(materialId))
			{

				var actionEfullfilment =
					$"{functionName}('{materialId}', '{productCode}','{action}{_componentId}');return false";
				return actionEfullfilment;
			}
			return string.Empty;
		}

		/// <summary>
		/// Get the url to be sent in clickHref property to the datalayer
		/// </summary>
		/// <param name="productCode">Product code</param>
		/// <param name="typeAction">Link, DataPass, Efulfillment, Comming soon, None(Anonymous)</param>
		/// <param name="renderingItem"></param>
		/// <param name="ctaLinkItemId"></param>
		/// <returns>Url string</returns>
		private string GetBaseUrlPartner(string productCode, ActionTypeEnum typeAction, Item renderingItem, ID ctaLinkItemId)
		{
			//Case Anonymous
			if (typeAction.Equals(ActionTypeEnum.None)) {
				var options = LinkManager.GetDefaultUrlOptions();
				options.AlwaysIncludeServerUrl = true;
				options.SiteResolving = true;
				return LinkManager.GetItemUrl(Sitecore.Context.Database.GetItem(Templates.LoginPage.ID),options);
			}
			//Case datapass and single sign on
			else if (typeAction.Equals(ActionTypeEnum.DataPass) || typeAction.Equals(ActionTypeEnum.SingleSignOn)) {
				var productCodeAmericanFidelity = _globalConfigurationManager.ProductCodeAmericanFidelity.Split('|');
				var productCodeClickAndSave = _globalConfigurationManager.ProductCodeClickAndSave.Split('|');
				var productCodeJeepZag = _globalConfigurationManager.ProductCodeJeepZag.Split('|');
				var productCodeMercer = _globalConfigurationManager.ProductCodeMercer.Split('|');

				if (productCodeAmericanFidelity.Contains(productCode)) {
					return _globalConfigurationManager.UrlAmericanFidelity;
				} else if (productCodeClickAndSave.Contains(productCode)) {
					return _globalConfigurationManager.UrlClickAndSave;
				} else if (productCodeJeepZag.Contains(productCode)) {
					return _globalConfigurationManager.UrlJeepZag;
				} else if (productCodeMercer.Contains(productCode)) {
					return _globalConfigurationManager.UrlMercer;
				} else {
					return "";
				}
			}
			//Case Link
			else if (typeAction.Equals(ActionTypeEnum.Link)) {
				var urlLink = renderingItem.LinkFieldUrl(ctaLinkItemId).Trim();
				urlLink = urlLink.Split(new[] { '?' })[0];
				return urlLink;
			}
			//Case efulfillment
			else if (typeAction.Equals(ActionTypeEnum.Efulfillment)) {
				return ConstantsNeamb.UrlApiProducts;
			}
			else if (typeAction.Equals(ActionTypeEnum.ComingSoon)) {
				if (HttpContext.Request.Url != null) {
					var urlLink = HttpContext.Request.Url.AbsoluteUri;
					urlLink = urlLink.Split(new[] { '?' })[0];
					return urlLink;
				}
				else return "";
			}
			else {
				return "";
			}
		}
	}
}