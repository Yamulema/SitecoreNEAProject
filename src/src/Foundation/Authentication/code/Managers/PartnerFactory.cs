using System;
using System.Linq;
using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Seiumb.Foundation.Authentication.Enums;
using Neambc.Seiumb.Foundation.Authentication.Interfaces;
using Sitecore.Configuration;

namespace Neambc.Seiumb.Foundation.Authentication.Managers {
	[Service(typeof(IPartnerFactory))]
	public class PartnerFactory : IPartnerFactory {
		private readonly IPartnerFactoryConfiguration _partnerFactoryConfiguration;
		public PartnerFactory(IEncryptionService encryptionService) {
			var xmlNode = Factory.GetConfigNode("seiumb/partnerConfiguration", true);
			_partnerFactoryConfiguration = Factory.CreateObject(xmlNode, true) as PartnerFactoryConfiguration;
		}

		public IPartner GetPartner(string productCode) {
			var partnerSettings = _partnerFactoryConfiguration.Partners
				.FirstOrDefault(x => x.Value.ProgramCodes
					.Any(y => y.Equals(productCode, StringComparison.InvariantCultureIgnoreCase)));

			switch (partnerSettings.Key) {
				case PartnerType.None:
					return null;
				case PartnerType.GreenDot:
					return new GreenDot(partnerSettings.Value);
				case PartnerType.SeiuAutoAndHomeInsurance:
					return new SeiuAutoAndHomeInsurance(partnerSettings.Value);
				default:
					return null;
			}
		}
	}
}