using System;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;
using Neambc.Neamb.Foundation.MBCData.Managers;

namespace Neambc.Neamb.Foundation.MBCData.UnitTest.Fakes {
	public class ExactTargetClientFactoryFake : IExactTargetSoapClientFactory {
		#region Implementation of IExactTargetClientFactory
		public IExactTargetSoapClient CreateClient() {
			throw new NotImplementedException();
		}
		#endregion
	}
}
