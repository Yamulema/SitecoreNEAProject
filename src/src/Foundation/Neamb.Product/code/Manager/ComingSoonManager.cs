using System.Runtime.Remoting.Contexts;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.Eligibility.Interfaces;
using Neambc.Neamb.Foundation.Eligibility.Model;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Neambc.Neamb.Foundation.Product.Interfaces;
using Neambc.Neamb.Foundation.Product.Model;
using Sitecore.Data;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neambc.Neamb.Foundation.Product.Manager {
	[Service(typeof(IComingSoonManager))]
	public class ComingSoonManager : IComingSoonManager {
		private readonly IEligibilityManager _eligibilityManager;
		private readonly IOracleDatabase _oracleManager;

		public ComingSoonManager(IEligibilityManager eligibilityManager, IOracleDatabase oracleManager) {
			_eligibilityManager = eligibilityManager;
			_oracleManager = oracleManager;
		}

		public OperationResult ExecuteProcess(ComingSoonModel comingSoonModel) {
			var operationResult = new OperationResult();
            var db = Sitecore.Context.Database;
            var mdsid = comingSoonModel.AccountUser.Mdsid;
            var contextItem = db.GetItem(new ID(comingSoonModel.ContextItemId));

            var eligibility = contextItem.Fields[new ID(comingSoonModel.EligibilityItemId)].IsChecked();

            var resultEligibility = EligibilityResultEnum.None;

            if (eligibility)
            {
                resultEligibility = _eligibilityManager.IsMemberEligible(comingSoonModel.AccountUser.Mdsid, comingSoonModel.ReminderId);
            }

            if ((eligibility && resultEligibility == EligibilityResultEnum.Eligible) || !eligibility) {
				var logCount =
					_oracleManager.ReminderLogCount(comingSoonModel.ReminderId, comingSoonModel.AccountUser.Mdsid);
				if (logCount == 0) {
					_oracleManager.InsertReminder(comingSoonModel.ReminderId, mdsid);
				}

				operationResult.Url = comingSoonModel.UrlReturn;
			} else {
				operationResult.ResultUrl = ResultUrlEnum.UnForbidden;
			}


			return operationResult;
		}
	}
}
