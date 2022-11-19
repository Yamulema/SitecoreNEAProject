using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Model;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Neambc.Seiumb.Foundation.Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Product.Repositories
{
	[Service(typeof(IComingSoonRepository))]
	public class ComingSoonRepository: IComingSoonRepository
	{
		private readonly IOracleDatabase _oracleManager;
		private readonly IProductGtmManager _productGtmManager;
		private readonly IBaseUrlColdUser _baseUrlColdUser;
		private readonly IBaseUrlComingSoon _baseUrlComingSoon;
		private readonly ISessionManager _sessionManager;
		private readonly ILog _log;

		public ComingSoonRepository(IOracleDatabase oracleManager, IProductGtmManager productGtmManager, IBaseUrlColdUser baseUrlColdUser, ISessionManager sessionManager, IBaseUrlComingSoon baseUrlComingSoon, ILog log) {
			_oracleManager = oracleManager;
			_productGtmManager = productGtmManager;
			_baseUrlColdUser = baseUrlColdUser;
			_sessionManager = sessionManager;
			_baseUrlComingSoon = baseUrlComingSoon;
			_log = log;
		}
		public ComingSoonResult GetPropertiesUser(StatusEnum statusUser,  ComingSoonRequest comingSoon) {
			ComingSoonResult comingSoonResult = new ComingSoonResult();

			if (statusUser == StatusEnum.Cold || statusUser== StatusEnum.Unknown) {
				comingSoonResult= SetPropertiesColdUser(comingSoon, statusUser);
			}
			else if (statusUser == StatusEnum.Hot) {
				comingSoonResult =SetPropertiesHotUser(comingSoon, statusUser);
			}
			else if (statusUser == StatusEnum.WarmHot || statusUser== StatusEnum.WarmCold)
			{
				comingSoonResult = SetPropertiesWarmUser(comingSoon, statusUser);
			} else {
				comingSoonResult.HasError = true;
				_log.Warn("Coming soon status user unknown", this);
			}
			return comingSoonResult;
		}

        public bool VerifyAlreadyNotified(ComingSoonRequest comingSoon) {
			var logCount = _oracleManager.ReminderLogCount(comingSoon.ProductCode, comingSoon.Mdsid);
			if (logCount > 0)
			{
				return true;
			} else {
				return false;
			}
		}

        private ComingSoonResult SetPropertiesColdUser(ComingSoonRequest comingSoon, StatusEnum userStatus)
        {
            ComingSoonResult comingSoonResult = new ComingSoonResult();
            comingSoon.UserName = ConstantsNeamb.UserCold;
            SaveSessionVariablesColdWarm(comingSoon);
            comingSoonResult.Action = string.Format("executeloginwarm('{0}{1}');", ConstantsNeamb.Primary, comingSoon.ComponentId);
            string actionHref = _baseUrlColdUser.GetBaseUrlPartner();
            var clickGtmAction = GetGtmAction(comingSoon.ProductName, comingSoon.ComponentType, comingSoon.RenderingItem, comingSoon.ReminderCtaId, actionHref, userStatus);
            if (!string.IsNullOrEmpty(clickGtmAction)) comingSoonResult.Action += $"{clickGtmAction}";
            return comingSoonResult;
        }

        private ComingSoonResult SetPropertiesHotUser(ComingSoonRequest comingSoon, StatusEnum userStatus) {
			ComingSoonResult comingSoonResult=new ComingSoonResult();
			comingSoonResult.Action = $"notifyproductavailable{comingSoon.ComponentId}('{comingSoon.ProductCode}','{comingSoon.RenderingItem.ID}','{comingSoon.EligibilityItemId}');return false;";
			string actionHref = _baseUrlComingSoon.GetBaseUrlPartner(comingSoon.UrlActionHref);
            var clickGtmAction = GetGtmAction(comingSoon.ProductName, comingSoon.ComponentType, comingSoon.RenderingItem, comingSoon.ReminderCtaId, actionHref, userStatus);
            if (!string.IsNullOrEmpty(clickGtmAction)) comingSoonResult.Action += $"{clickGtmAction}";
            return comingSoonResult;
		}

		private ComingSoonResult SetPropertiesWarmUser(ComingSoonRequest comingSoon, StatusEnum userStatus) {
			ComingSoonResult comingSoonResult= new ComingSoonResult();
			SaveSessionVariablesColdWarm(comingSoon);
            comingSoonResult.Action = string.Format("executeloginwarm('{0}{1}');", ConstantsNeamb.Primary, comingSoon.ComponentId);
            string actionHref = _baseUrlComingSoon.GetBaseUrlPartner(comingSoon.UrlActionHref);
            var clickGtmActionWarm = GetGtmAction(comingSoon.ProductName, comingSoon.ComponentType, comingSoon.RenderingItem, comingSoon.ReminderCtaId, actionHref, userStatus);
            if (!string.IsNullOrEmpty(clickGtmActionWarm)) comingSoonResult.Action += $"{clickGtmActionWarm}";
            if (userStatus == StatusEnum.WarmHot) {
                var clickGtmActionHot = GetGtmAction(comingSoon.ProductName, comingSoon.ComponentType, comingSoon.RenderingItem, comingSoon.ReminderCtaId, actionHref, StatusEnum.Hot);
                _sessionManager.StoreInSession($"{ConstantsNeamb.ProductGtmActionPrimary}{comingSoon.ComponentId}", clickGtmActionHot);
            }
            return comingSoonResult;
		}

        private string GetGtmAction(string productName, ComponentTypeEnum componentType, Item renderingItem, ID reminderCtaId, string actionHref, StatusEnum statusUser) {
            string reminderCta = reminderCtaId != ID.Null && reminderCtaId != ID.Undefined ? renderingItem[reminderCtaId] : "";
            ProductCtaBase productGtm = new ProductCtaBase { ProductName = productName, CtaText = reminderCta };
            return _productGtmManager.GetGtmFunction(componentType, renderingItem, actionHref, productGtm, statusUser);
        }

        private void SaveSessionVariablesColdWarm(ComingSoonRequest comingSoon) {
            var notifyAction = $"notifyproductavailablewarm('{comingSoon.ProductCode}','{comingSoon.RenderingItem.ID}','{comingSoon.EligibilityItemId}');return false;";
            _sessionManager.StoreInSession<string>($"{ConstantsNeamb.CtaActionPrimary}{comingSoon.ComponentId}", string.Empty);
            _sessionManager.StoreInSession<string>(
                $"{ConstantsNeamb.CtaActionPrimaryOnclick}{comingSoon.ComponentId}",
                !string.IsNullOrEmpty(notifyAction) ? notifyAction.Replace(";return false", string.Empty) : string.Empty);
            _sessionManager.StoreInSession<string>(
                $"{ConstantsNeamb.CtaActionPrimaryTargetBlank}{comingSoon.ComponentId}",
                "_blank");
            _sessionManager.StoreInSession($"{ConstantsNeamb.ProductComponent}{comingSoon.ComponentId}", comingSoon.ProductCode);

            //Execute login when the user is warm hot
            if (!string.IsNullOrEmpty(comingSoon.UserName)) {
                _sessionManager.StoreInSession<string>(ConstantsNeamb.CtaActionWarmUser, comingSoon.UserName);
            }
            _sessionManager.StoreInSession<bool>($"{ConstantsNeamb.ProductHasCheckEligibility}{comingSoon.ComponentId}", comingSoon.HasCheckEligibility);
        }
    }
}