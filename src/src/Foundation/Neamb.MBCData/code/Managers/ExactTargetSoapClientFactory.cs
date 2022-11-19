using System;
using System.ServiceModel;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Manager;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;

namespace Neambc.Neamb.Foundation.MBCData.Managers {
	[Service(typeof(IExactTargetSoapClientFactory))]
	public class ExactTargetSoapClientFactory : IExactTargetSoapClientFactory {

		#region Fields
		private readonly IGlobalConfigurationManager _globalConfigurationManager;
		private readonly BasicHttpBinding _defaultBinding;
		private readonly EndpointAddress _endpoint;
		#endregion

		#region Constructor
		public ExactTargetSoapClientFactory(IGlobalConfigurationManager globalConfigurationManager) {
			_globalConfigurationManager = globalConfigurationManager ?? throw new ArgumentNullException(nameof(globalConfigurationManager));
			_defaultBinding = new BasicHttpBinding {
				Name = ConstantsNeamb.UserNameSoapBinding,
				Security = {
					Mode = BasicHttpSecurityMode.TransportWithMessageCredential,
					Message = {ClientCredentialType = BasicHttpMessageCredentialType.UserName}
				},
				ReceiveTimeout = new TimeSpan(0, 0, _globalConfigurationManager.ServiceTimeout),
				OpenTimeout = new TimeSpan(0, 0, _globalConfigurationManager.ServiceTimeout),
				CloseTimeout = new TimeSpan(0, 0, _globalConfigurationManager.ServiceTimeout),
				SendTimeout = new TimeSpan(0, 0, _globalConfigurationManager.ServiceTimeout)
			};
			_endpoint = new EndpointAddress(_globalConfigurationManager.ExacttargetEndPoint);
		}
		#endregion

		#region Public Methods
		public IExactTargetSoapClient CreateClient() {
			// this is disposable 
			var ret = new SoapClient(_defaultBinding, _endpoint) {
				ClientCredentials = {
					UserName = {
						UserName = _globalConfigurationManager.ExacttargetUsername,
						Password = _globalConfigurationManager.ExacttargetPassword
					},

				}
			};
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
			// Uncomment this line for SOAP message debugging.
			ret.Endpoint.Behaviors.Add(new EndpointInspectorBehavior());
			return (IExactTargetSoapClient)ret;
		}
		#endregion
	}

}