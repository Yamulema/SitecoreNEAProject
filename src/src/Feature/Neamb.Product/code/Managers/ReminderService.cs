using Neambc.Neamb.Feature.Product.Interfaces;
using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Membership.Managers;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neambc.Neamb.Feature.Product.Managers {
	[Service(typeof(IReminderService))]
	public class ReminderService : IReminderService {
		private readonly IOracleDatabase _oracleManager;
		private readonly ISessionAuthenticationManager _sessionAuthenticationManager;

		public ReminderService(IOracleDatabase oracleManager, ISessionAuthenticationManager sessionAuthenticationManager) {
			_oracleManager = oracleManager;
			_sessionAuthenticationManager = sessionAuthenticationManager;
		}

		public bool SetReminder(string id, string mdsid) {
			return HasReminder(id, mdsid) || _oracleManager.InsertReminder(id, mdsid);
		}

		public bool HasReminder(string id, string mdsId) {
			return _oracleManager.ReminderLogCount(id, mdsId) != 0;
		}
		public Reminder GetReminder(SweepstakesDTO model) {
			if (model == null) {
				return null;
			}
			var datasource = model.SweepstakesBase.Item ?? model.SweepstakesBase.PageItem;
			var result = new Reminder() {
				Datasource = datasource,
				Enabled = datasource.Fields[Templates.Reminder.Fields.ComingSoon].IsChecked(),
				Id = model.SweepstakesId,
                ComponentIdAuthentication = model.SweepstakesBase.ComponentIdAuthentication,
                HasResultAuthentication = model.SweepstakesBase.HasResultAuthentication
			};

			if (result.Enabled) {
				var accountMembership = _sessionAuthenticationManager.GetAccountMembership();
				result.Notified = HasReminder(result.Id, accountMembership.Mdsid);
			}
			return result;
		}
        
    }
}