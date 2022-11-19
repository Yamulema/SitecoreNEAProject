using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Configuration;

namespace Neambc.Seiumb.Foundation.WebServices
{
    [Service(typeof(IWebServicesConfiguration))]
    public class WebServicesConfiguration : IWebServicesConfiguration
    {
        public int ConfiguredTimeout { get; set; }
        public string Partner { get; set; }
        public string Key { get; set; }
        public int ServiceTimeout { get; set; }
        public string UnionId { get; set; }
        public string AccountEndpoint { get; set; }
        public string EligibilityEndpoint { get; set; }
        public string MatchRoutineIdentifierSeium { get; set; }
		public string AfiniumEncryptDecryptPasswordSeiumb { get; set; }
		public string AfiniumEncryptDecryptSaltSeiumb { get; set; }
		public int AfiniumEncryptDecryptPasswordInteractionsSeiumb { get; set; }
		public int AfiniumEncryptDecryptKeySizeSeiumb { get; set; }
		public string Webusersource { get; set; }

		public WebServicesConfiguration()
        {
            ConfiguredTimeout = int.Parse(Settings.GetSetting("ServiceTimeOut"));
            Partner = Settings.GetSetting("NeambcPartner");
            Key = Settings.GetSetting("NeambcKey");
            ServiceTimeout = ConfiguredTimeout > 0 ? ConfiguredTimeout * 1000 : 180000;
            UnionId = Settings.GetSetting("UnionIDSeiu");
			Webusersource = Settings.GetSetting("WebusersourceSeiu");
			AccountEndpoint = Settings.GetSetting("NeambcEndpoint");
            EligibilityEndpoint = Settings.GetSetting("EligibilityEndPoint");
            MatchRoutineIdentifierSeium = Settings.GetSetting("MatchroutineidentifierSeiu");
			AfiniumEncryptDecryptPasswordSeiumb = Settings.GetSetting("AfiniumEncryptDecryptPasswordSeiumb");
			AfiniumEncryptDecryptSaltSeiumb = Settings.GetSetting("AfiniumEncryptDecryptSaltSeiumb");
			AfiniumEncryptDecryptPasswordInteractionsSeiumb = int.Parse(Settings.GetSetting("AfiniumEncryptDecryptPasswordInteractionsSeiumb"));
			AfiniumEncryptDecryptKeySizeSeiumb = int.Parse(Settings.GetSetting("AfiniumEncryptDecryptKeySizeSeiumb"));
        }
	}
}