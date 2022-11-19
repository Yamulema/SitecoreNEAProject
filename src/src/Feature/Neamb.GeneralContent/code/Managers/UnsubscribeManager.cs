using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Managers;

namespace Neambc.Neamb.Feature.GeneralContent.Managers {
	[Service(typeof(IUnsubscribeManager))]
	public class UnsubscribeManager : IUnsubscribeManager {

		#region Fields
		private readonly IExactTargetClient _exactTargetClient;
		#endregion

		#region Constructors
		public UnsubscribeManager(IExactTargetClient exactTargetClient) {
			_exactTargetClient = exactTargetClient;
		}
		#endregion

		#region Public Methods
		public void UnsubscribeList(int listid, string mdsid, UnsubscribeDTO unsubscribeModel) {
			unsubscribeModel.IsSucess = _exactTargetClient.UnsubscribeListMail(mdsid, listid);
		}
		#endregion
	}
}