namespace Neambc.Seiumb.Foundation.WebServices {
    public interface IWebServicesConfiguration
    {
        int ConfiguredTimeout { get; set; }
        string Partner { get; set; }
        string Key { get; set; }
        int ServiceTimeout { get; set; }
        string UnionId { get; set; }
        string AccountEndpoint { get; set; }
        string EligibilityEndpoint { get; set; }
        string MatchRoutineIdentifierSeium { get; set; }
		string AfiniumEncryptDecryptPasswordSeiumb { get; set; }
		string AfiniumEncryptDecryptSaltSeiumb { get; set; }
		int AfiniumEncryptDecryptPasswordInteractionsSeiumb { get; set; }
		int AfiniumEncryptDecryptKeySizeSeiumb { get; set; }
		string Webusersource { get; set; }
	}
}