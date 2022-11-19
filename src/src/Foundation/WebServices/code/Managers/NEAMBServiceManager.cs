using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Foundation.WebServices.org.neambc.elegibility;
using Neambc.Seiumb.Foundation.WebServices.org.neambc.encryptafinium;
using System;
using System.ServiceModel;

namespace Neambc.Seiumb.Foundation.WebServices.Managers {
    [Service(typeof(INeambServiceManager))]
	public class NeambServiceManager : INeambServiceManager {

		#region Fields
		private readonly IWebServicesConfiguration _webServicesConfiguration;
		#endregion

		#region Constructors
		public NeambServiceManager(IWebServicesConfiguration webServicesConfiguration) {
			_webServicesConfiguration = webServicesConfiguration;
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Gets products 
		/// </summary>
		/// <param name="mdsid"></param>
		/// <returns></returns>
		//public string GetEligibilityService(string mdsid) {
		//	seiumb client = null;

		//	try {
		//		client = new seiumb { Timeout = _webServicesConfiguration.ServiceTimeout, Url = _webServicesConfiguration.EligibilityEndpoint };
		//		var response = client.productEligibility(mdsid, _webServicesConfiguration.Partner, _webServicesConfiguration.Key);

		//		return response;
		//	} catch (Exception ex) {
		//		//abort connection for all exceptions
		//		client?.Abort();
		//		Sitecore.Diagnostics.Log.Error(ex.Message, ex, nameof(NeambServiceManager));

		//		if (ex is TimeoutException || ex is CommunicationException) {
		//			return "2"; //TODO : replace for variable with explanation
		//		}

		//		throw;
		//	} finally {
		//		client?.Dispose();
		//	}
		//}

		/// <summary>
		/// Gets the mdsid encrypted for Afenium partner
		/// </summary>
		/// <param name="mdsid">User mdsid</param>
		/// <returns>Mdsid encrypted</returns>
		public string EncryptPartner(string mdsid)
		{
			mdsid = mdsid ?? throw new ArgumentNullException(nameof(mdsid));
			var password = _webServicesConfiguration.AfiniumEncryptDecryptPasswordSeiumb;
			using (var client = new aes256EncryptDecrypt { Timeout = _webServicesConfiguration.ServiceTimeout })
			{
				return client.encrypt(mdsid, password,
					_webServicesConfiguration.AfiniumEncryptDecryptSaltSeiumb,
					_webServicesConfiguration.AfiniumEncryptDecryptPasswordInteractionsSeiumb , true,
					_webServicesConfiguration.AfiniumEncryptDecryptKeySizeSeiumb, true
				);
			}
		}
		#endregion
	}
}