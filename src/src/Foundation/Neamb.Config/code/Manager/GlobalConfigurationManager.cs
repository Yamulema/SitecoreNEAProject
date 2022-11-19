using System;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Data;
using config = Sitecore.Configuration;

namespace Neambc.Neamb.Foundation.Configuration.Manager {
	[Service(typeof(IGlobalConfigurationManager))]
	public class GlobalConfigurationManager : IGlobalConfigurationManager {
        
        private string _neambcPartner;
		private string _neambcKey;
		private string _neambcUnionId;

        #region Unclassified ReadOnly Prop
        private readonly int _expirationMinutesCacheEligibilityMarketplace;
        private readonly string _neambcKeyMatchRoutineIdentifier;
		private readonly string _exacttargetUsername;
		private readonly string _exacttargetPassword;
		private readonly string _cellCodeResetPasswordLockedOut;
		private readonly string _campaignResetPassword;
		private readonly string _customerDefinitionResetPassword;
		private readonly string _exacttargetEndPoint;
		private readonly int _exacttargetClientId;
		private int _serviceTimeout;
		private readonly TimeSpan _timeWarmCookie;
		private readonly TimeSpan _timeremembermeCookie;
		private readonly string _webusersource;
		private readonly bool _exactTargetUpdateSubscriberKey;
        private readonly string _unionid;
		private readonly string _customerDefinitionDuplicateRegistration;
		private readonly string _cellCodeDuplicateRegistration;
		private readonly string _campaignCodeDuplicateRegistration;
		private readonly int _attemptZipCodeValidation;
		private readonly string _enrollmentServiceUser;
		private readonly string _enrollmentServicePassword;
		private readonly int _expirationRedisPdf;
		private readonly string _customerDefinitionChangeUsername;
		private readonly string _cellcodeChangeUsernameNewLogin;
		private readonly string _cellcodeChangeUsernameOldLogin;
		private readonly string _campaignCodeChangeUsername;
		private readonly string _urlJeepZag;
		private readonly string _customerDefinitionSweepstake;
        private readonly string _customerDefinitionSeminarForm;
		private readonly string _cellCodeResetPasswordRequestedReset;
        private readonly string _cellCodeResetPasswordRequestedResetOldToken;
        private readonly string _afiniumEncryptDecryptPassword;
		private readonly string _mercerEncryptDecryptPassword;
		private readonly string _mercerEncryptDecryptSalt;
		private readonly int _AESPasswordInteractions;
		private readonly int _AESKeySize;
		private readonly string _bucketNameAvatarImages;
		private readonly int _maxImageSizeAvatar;
		private readonly string _customerDefinitionAddFamilyMember;
		private readonly string _cellcodeAddFamilyMember;
		private readonly string _campaignAddFamilyMember;
		private readonly string _catpchaUrl;
		private readonly string _catpchaSecret;
		private readonly string _bucketNameContestImages;
		private readonly int _maxFileSizeFileContest;
		private readonly string _s3SubmissionFolder;
		private readonly string _complimentaryLifeProductCode;
		private readonly string _introLifeProductCode;
		private readonly string _itemErrorMscEfulfillment;
		private readonly string _productCodeAmericanFidelity;
		private readonly string _urlAmericanFidelity;
		private readonly string _productCodeClickAndSave;
        private readonly string _urlClickAndSave;
		private readonly string _productCodeJeepZag;
		private readonly string _productCodeMercer;
		private readonly string _urlMercer;
		private readonly int _maxTabbedHeroItems;
		private readonly string _termsAndConditionsControlIdr;
		private readonly int _maxCardCount;
		private readonly string _pathFileProcessUpdatePublishDate;
		private readonly bool _executeProcessUpdatePublishDate;
		private readonly int _expirationHoursTokenRedis;
        private readonly int _expirationHoursSeminaries;
        private readonly int _expirationCacheOmniEligibility;
        private readonly int _expirationCacheLocalCodesHours;
        private readonly bool _enableCustomFacets;
        private readonly string _urlEfulfillmentS3;
        private readonly string _urlEfulfillmentS3External;
        private readonly string _urlEfulfillmentS3Seiumb;
        private readonly string _urlEfulfillmentS3SeiumbExternal;
        private readonly string _restUrl;
        private readonly string _restUserAuthentication;
        private readonly string _restPasswordAuthentication;
        private readonly string _restUrlProductEligibility;
        private readonly string _restUrlProductEligibilitySeiumb;
        private readonly string _restUrlCompIntroEligibility;
        private readonly string _restUrlUpdateUserName;
        private readonly string _restUrlForgotUserName;
        private readonly string _restUrlValidateEmailDomain;
        private readonly string _restUrlDeleteUser;
        private readonly string _restUrlUpdateUser;
        private readonly string _restUrlAuthenticatePassword;
        private readonly string _compLifeEmailCellCodeToteBag;
        private readonly string _restUrlSearchUserName;
        private readonly string _restUrlRetrieveUser;
        private readonly string _afiniumAESPassword;
        private readonly string _mercerAESPassword;
        private readonly string _afiniumAESSalt;
        private readonly string _mercerAESSalt;
        private readonly string _restUrlLogin;
        private readonly string _restUrlUpdatePassword;
        private readonly string _restUrlUpdateUserStatus;
        private readonly string _restUrlResetPassword;
        private readonly string _referrerIdJeepZag;
        private readonly string _headerTrueCarToken;
        private readonly string _urlTrueCarSeiumb;
        private readonly string _referrerIdSeiumb;
        private readonly string _headerTrueCarTokenSeiumb;
        private readonly string _restUrlRegisterUser;
        private readonly string _restUrlCreateResetToken;
        private readonly string _restUrlValidateResetToken;
        private readonly string _restUrlCancelResetToken;
        private readonly string _restUrlEfulfillment;
        private readonly string _restUrlAESEncrypt;
        private readonly string _restUrlAESDecrypt;
        private readonly string _restUrlICEGetBalance;
        private readonly string _productCodeStoresSeiumb;
        private readonly string _timeZoneRangeTimePersonalizationRule;
        private readonly string _specialCharacterSolr;
        private readonly bool _excludeCommaSpecialCharacterSolr;
        private readonly bool _excludeAmpSpecialCharacterSolr;

        #endregion

        #region Rakuten ReadOnly Prop
        private readonly string _rakutenServerApiUrl;
        private readonly string _rakutenStoreApiUrl;
        private readonly string _rakutenStoreDetailApiUrl;
        private readonly string _rakutenStoreChannel;
        private readonly string _rakutenSeiumbStoreChannel;
        private readonly int _expirationRedisEtag;
        private readonly string _rakutenMediaServerUrl;
        private readonly string _rakutenShoppingBaseUrl;
        private readonly string _rakutenPartnerId;
        private readonly string _rakutenSeiumbPartnerId;
        private readonly string _rakutenCategoriesParentID;
        #endregion
        private readonly string _productCodeStores;
        private readonly string _storesSkipImportProcess;
        private readonly string _neambIndex;
        private readonly string _seiumbIndex;
        private readonly string _storeIndex;
        private readonly string _indicatorNoPermissionEmail;
        private readonly string _indicatorYesPermissionEmail;
        private readonly string _typePermissionEmail;
        private readonly string _businessUnitPermissionEmail;
        private readonly string _captchaSecretSeiumb;
        private readonly string _captchaKeySeiumb;
        #region Properties
        public int ExpirationMinutesCacheEligibilityMarketplace => (_expirationMinutesCacheEligibilityMarketplace);
        public string StoresSkipImportProcess => (_storesSkipImportProcess);
        public string TimeZoneRangeTimePersonalizationRule => (_timeZoneRangeTimePersonalizationRule);
        public string ProductCodeStoresSeiumb => (_productCodeStoresSeiumb);
        public string ProductCodeStores => (_productCodeStores);
        public string UrlEfulfillmentS3Seiumb => (_urlEfulfillmentS3Seiumb);
        public string UrlEfulfillmentS3SeiumbExternal => (_urlEfulfillmentS3SeiumbExternal);
        public string RakutenStoreChannel => (_rakutenStoreChannel);
        public string RakutenSeiumbStoreChannel => (_rakutenSeiumbStoreChannel);
        public int ExpirationRedisEtag => (_expirationRedisEtag);
        public string RakutenServerApiUrl => (_rakutenServerApiUrl);
        public string RakutenStoreApiUrl => (_rakutenStoreApiUrl);
        public string RakutenStoreDetailApiUrl => (_rakutenStoreDetailApiUrl);
        public string UrlEfulfillmentS3 => (_urlEfulfillmentS3);
        public string UrlEfulfillmentS3External => (_urlEfulfillmentS3External);
        public int ExpirationCacheLocalCodesHours => (_expirationCacheLocalCodesHours);
        public bool EnableCustomFacets => (_enableCustomFacets);
        public int ExpirationCacheOmniEligibility => (_expirationCacheOmniEligibility);
        public int ExpirationHoursTokenRedis => (_expirationHoursTokenRedis);
		public bool ExecuteProcessUpdatePublishDate => (_executeProcessUpdatePublishDate);
		public string PathFileProcessUpdatePublishDate => (_pathFileProcessUpdatePublishDate);
		public int MaxCardCount => (_maxCardCount);
		public string TermsAndConditionsControlId => (_termsAndConditionsControlIdr);
		public int MaxTabbedHeroItems => (_maxTabbedHeroItems);
		public string ProductCodeAmericanFidelity => (_productCodeAmericanFidelity);
		public string UrlAmericanFidelity => (_urlAmericanFidelity);
		public string ProductCodeClickAndSave => (_productCodeClickAndSave);
		public string UrlClickAndSave => (_urlClickAndSave);
		public string ProductCodeJeepZag => (_productCodeJeepZag);
		public string ProductCodeMercer => (_productCodeMercer);
		public string UrlMercer => (_urlMercer);
		public string ItemErrorMscEfulfillment => (_itemErrorMscEfulfillment);
		public string ComplimentaryLifeProductCode => (_complimentaryLifeProductCode);
		public string IntroLifeProductCode => (_introLifeProductCode);

		public string S3SubmissionFolder => (_s3SubmissionFolder);
		public int MaxFileSizeFileContest => (_maxFileSizeFileContest);

		public int MaxImageSizeAvatar => (_maxImageSizeAvatar);
		public string BucketNameAvatarImages => (_bucketNameAvatarImages);
		public string CatpchaUrl => (_catpchaUrl);
		public string CatpchaSecret => (_catpchaSecret);
		/// <summary>AfiniumEncryptDecryptPassword</summary>
		public string AfiniumEncryptDecryptPassword => (_afiniumEncryptDecryptPassword);
		/// <summary>MercerEncryptDecryptPassword</summary>
		public string MercerEncryptDecryptPassword => (_mercerEncryptDecryptPassword);
		/// <summary>MercerEncryptDecryptSalt</summary>
		public string MercerEncryptDecryptSalt => (_mercerEncryptDecryptSalt);
        /// <summary>AESPasswordInteractions</summary>
        public int AESPasswordInteractions => (_AESPasswordInteractions);
        /// <summary>AESKeySize</summary>
        public int AESKeySize => (_AESKeySize);
		/// <summary>NEMBC partner</summary>
		public string NeambcPartner {
			get => (_neambcPartner);
			set => _neambcPartner = value;
		}

		/// <summary>NEMBC key</summary>
		public string NeambcKey {
			get => (_neambcKey);
			set => _neambcKey = value;
		}

		/// <summary>NEMBC Union ID</summary>
		public string NeambcUnionId {
			get => (_neambcUnionId);
			set => _neambcUnionId = value;
		}

		/// <summary>NEMBC Matchroutineidentifier</summary>
		public string NeambcKeyMatchRoutineIdentifier => (_neambcKeyMatchRoutineIdentifier);

		/// <summary>Service timeout</summary>
		public int ServiceTimeout {
			get => (_serviceTimeout);
			set => _serviceTimeout = value;
		}

		/// <summary>Exact target user name for authenticate</summary>
		public string ExacttargetUsername => (_exacttargetUsername);

		/// <summary>Exact target password for authenticate</summary>
		public string ExacttargetPassword => (_exacttargetPassword);

		/// <summary>Cellcode value for Exact target in reset password feature</summary>
		public string CellCodeResetPasswordLockedOut => (_cellCodeResetPasswordLockedOut);

		/// <summary>Campaign value for Exact target in reset password feature</summary>
		public string CampaignResetPassword => (_campaignResetPassword);

		/// <summary>Customer definition value for Exact target in reset password feature</summary>
		public string CustomerDefinitionResetPassword => (_customerDefinitionResetPassword);

		/// <summary>Exact target endpoint</summary>
		public string ExacttargetEndPoint => (_exacttargetEndPoint);

		/// <summary>Client id in exact target</summary>
		public int ExacttargetClientId => (_exacttargetClientId);

		/// <summary>Time in days for the cookie warm</summary>
		public TimeSpan TimeWarmCookie => (_timeWarmCookie);

		/// <summary>Time in days for the cookie remember me</summary>
		public TimeSpan TimeRemembermeCookie => (_timeremembermeCookie);

		/// <summary>
		/// Registration Webusersource parameter to send the webservice
		/// </summary>
		public string Webusersource => (_webusersource);
        /// <summary>
        /// Flag to enable or disable change of subscriber key for test users
        /// </summary>
        public bool ExactTargetUpdateSubscriberKey => (_exactTargetUpdateSubscriberKey);

        /// <summary>
        /// Registration Unionid parameter to send the webservice
        /// </summary>
        public string Unionid => (_unionid);

		/// <summary>
		/// Customer key Duplicate registration email
		/// </summary>
		public string CustomerDefinitionDuplicateRegistration => (_customerDefinitionDuplicateRegistration);

		/// <summary>
		/// Cellcode duplicate registration email
		/// </summary>
		public string CellCodeDuplicateRegistration => (_cellCodeDuplicateRegistration);

		/// <summary>
		/// Campaign code duplicate registration email
		/// </summary>
		public string CampaignCodeDuplicateRegistration => (_campaignCodeDuplicateRegistration);

		/// <summary>
		/// AttemptZipCodeValidation
		/// </summary>
		public int AttemptZipCodeValidation => (_attemptZipCodeValidation);
		/// <summary>
		/// Enrollment ServiceUser used in American Fidelity partner
		/// </summary>
		public string EnrollmentServiceUser => (_enrollmentServiceUser);
		/// <summary>
		/// Enrollment Service Password used in American Fidelity partner
		/// </summary>
		public string EnrollmentServicePassword => (_enrollmentServicePassword);
		public int ExpirationRedisPdf => (_expirationRedisPdf);

		public string CustomerDefinitionChangeUsername => (_customerDefinitionChangeUsername);

		public string CellcodeChangeUsernameNewLogin => (_cellcodeChangeUsernameNewLogin);

		public string CellcodeChangeUsernameOldLogin => (_cellcodeChangeUsernameOldLogin);

		public string CampaignCodeChangeUsername => (_campaignCodeChangeUsername);

		/// <summary>
		/// Url to call the partner jeep and zag
		/// </summary>
		public string UrlJeepZag => (_urlJeepZag);

		/// <summary>
		/// Customer Definition Sweep stake email
		/// </summary>
		public string CustomerDefinitionSweepstake => (_customerDefinitionSweepstake);
        /// <summary>
        /// Customer Definition Seminar
        /// </summary>
        public string CustomerDefinitionSeminarForm => (_customerDefinitionSeminarForm);
		/// <summary>NEMBC partner</summary>
        public string CellCodeResetPasswordRequestedReset => (_cellCodeResetPasswordRequestedReset);
        public string CellCodeResetPasswordRequestedResetOldToken => (_cellCodeResetPasswordRequestedResetOldToken);
        /// <summary>CustomerDefinitionAddFamilyMember</summary>
        public string CustomerDefinitionAddFamilyMember => (_customerDefinitionAddFamilyMember);
		/// <summary>CellcodeAddFamilyMember</summary>
		public string CellcodeAddFamilyMember => (_cellcodeAddFamilyMember);
		/// <summary>CampaignAddFamilyMember</summary>
		public string CampaignAddFamilyMember => (_campaignAddFamilyMember);
		public string BucketNameContestImages => (_bucketNameContestImages);
        public int ExpirationHoursSeminaries => (_expirationHoursSeminaries);
        /// <summary>RestUrl</summary>
        public string RestUrl => (_restUrl);
        /// <summary>RestUserAuthentication</summary>
        public string RestUserAuthentication => (_restUserAuthentication);
        /// <summary>RestPasswordAuthentication</summary>
        public string RestPasswordAuthentication => (_restPasswordAuthentication);
        /// <summary>RestPasswordAuthentication</summary>
        public string RestUrlProductEligibility => (_restUrlProductEligibility);
        /// <summary>LifetimeAccessTokenMinutes</summary>
        public double LifetimeAccessTokenMinutes { get; }
        /// <summary>RestUrlProductEligibilitySeiumb</summary>
        public string RestUrlProductEligibilitySeiumb => (_restUrlProductEligibilitySeiumb);
        /// <summary>RestUrlProductCompIntro</summary>
        public string RestUrlCompIntroEligibility => (_restUrlCompIntroEligibility);
        public string RestUrlUpdateUserName => (_restUrlUpdateUserName);
        public string RestUrlForgotUserName => (_restUrlForgotUserName);
        public string RestUrlValidateEmailDomain => (_restUrlValidateEmailDomain);
        public string RestUrlDeleteUser => (_restUrlDeleteUser);
        public string RestUrlUpdateUser => (_restUrlUpdateUser);
        public string RestUrlAuthenticatePassword => (_restUrlAuthenticatePassword);
        public string RestUrlRakutenMemberCreation { get; }
        public string RestUrlRakutenMemberCreationSEIU { get; }
        public string CompLifeEmailCellCodeToteBag => (_compLifeEmailCellCodeToteBag);
        public string RestUrlSearchUserName => (_restUrlSearchUserName);
        public string RestUrlRetrieveUser => (_restUrlRetrieveUser);
        public string AfiniumAESPassword => (_afiniumAESPassword);
        public string MercerAESPassword => (_mercerAESPassword);
        public string MercerAESSalt => (_mercerAESSalt);
        public string AfiniumAESSalt => (_afiniumAESSalt);
        public string RestUrlLogin => (_restUrlLogin);
        public string RestUrlUpdatePassword => (_restUrlUpdatePassword);
        public string RestUrlUpdateUserStatus => (_restUrlUpdateUserStatus);
        public string RestUrlResetPassword => (_restUrlResetPassword);
        public string ReferrerIdJeepZag => (_referrerIdJeepZag);
        public string HeaderTrueCarToken => (_headerTrueCarToken);
        public string UrlTrueCarSeiumb => (_urlTrueCarSeiumb);
        public string ReferrerIdSeiumb => (_referrerIdSeiumb);
        public string HeaderTrueCarTokenSeiumb => (_headerTrueCarTokenSeiumb);
        public string RestUrlRegisterUser => (_restUrlRegisterUser);
        public string RestUrlCreateResetToken => (_restUrlCreateResetToken);
        public string RestUrlValidateResetToken => (_restUrlValidateResetToken);
        public string RestUrlCancelResetToken => (_restUrlCancelResetToken);
        public string RestUrlEfulfillment => (_restUrlEfulfillment);
        public string RestUrlAESEncrypt => (_restUrlAESEncrypt);
        public string RestUrlAESDecrypt => (_restUrlAESDecrypt);
        public string SpecialCharacterSolr => (_specialCharacterSolr);
        public bool ExcludeCommaSpecialCharacterSolr => (_excludeCommaSpecialCharacterSolr);
        public bool ExcludeAmpSpecialCharacterSolr => (_excludeAmpSpecialCharacterSolr);
        public string RestUrlICEGetBalance => (_restUrlICEGetBalance);
        public string NeambIndex => (_neambIndex);
        public string SeiumbIndex => (_seiumbIndex);
        public string StoreIndex => (_storeIndex);
        public string IndicatorNoPermissionEmail => (_indicatorNoPermissionEmail);
        public string IndicatorYesPermissionEmail => (_indicatorYesPermissionEmail);
        public string TypePermissionEmail => (_typePermissionEmail);

        public string BusinessUnitPermissionEmail => (_businessUnitPermissionEmail);
        public string CaptchaSecretSeiumb => (_captchaSecretSeiumb);
        public string CaptchaKeySeiumb => (_captchaKeySeiumb);
        #endregion

        #region Rakuten Properties
        public string RakutenLocale { get; }
        public string RakutenSignature { get; }
        public string RakutenSEIUSignature { get; }
        public string RakutenMemberSource { get; }
        public string RakutenSEIUMemberSource { get; }
        public string RakutenMediaServerUrl => (_rakutenMediaServerUrl);
        public string RakutenShoppingBaseUrl => (_rakutenShoppingBaseUrl);
        public string RakutenPartnerId => (_rakutenPartnerId);
        public string RakutenSeiumbPartnerId => (_rakutenSeiumbPartnerId);
        public string RakutenCategoriesParentID => (_rakutenCategoriesParentID);
        #endregion        

        public GlobalConfigurationManager() {
            #region Unclassified Constructors
            _urlEfulfillmentS3 = GetOptionalString(ConstantsNeamb.UrlEfulfillmentS3, string.Empty);
            _urlEfulfillmentS3External = GetOptionalString(ConstantsNeamb.UrlEfulfillmentS3External, string.Empty);
            _neambcPartner = GetOptionalString(ConstantsNeamb.NeambcPartner, string.Empty);
			_neambcKey = GetOptionalString(ConstantsNeamb.NeambcKey, string.Empty);
			_neambcUnionId = GetOptionalString(ConstantsNeamb.NeambcUnionId, string.Empty);
			_neambcKeyMatchRoutineIdentifier = GetOptionalString(ConstantsNeamb.NeambcKeyMatchroutineidentifier, string.Empty);
			_serviceTimeout = GetOptionalInt(ConstantsNeamb.ServiceTimeout, 180);
			_exacttargetPassword = GetOptionalString(ConstantsNeamb.ExacttargetPassword, string.Empty);
			_exacttargetUsername = GetOptionalString(ConstantsNeamb.ExacttargetUsername, string.Empty);
			_neambcUnionId = GetOptionalString(ConstantsNeamb.NeambcUnionId, string.Empty);
			_cellCodeResetPasswordLockedOut = GetOptionalString(ConstantsNeamb.CellCodeResetPasswordLockedOut, string.Empty);
			_campaignResetPassword = GetOptionalString(ConstantsNeamb.CampaignResetPassword, string.Empty);
			_customerDefinitionResetPassword = GetOptionalString(ConstantsNeamb.CustomerDefinitionResetPassword, string.Empty);
			_exacttargetEndPoint = GetOptionalString(ConstantsNeamb.ExacttargetEndPoint, string.Empty);
			_exacttargetClientId = GetOptionalInt(ConstantsNeamb.ExacttargetClientId,0);
			_timeWarmCookie = GetTimeSpan(ConstantsNeamb.TimeWarmCookie);
			_timeremembermeCookie = GetTimeSpan(ConstantsNeamb.TimeRemembermeCookie);
			_webusersource = GetOptionalString(ConstantsNeamb.Webusersource, string.Empty);
            _exactTargetUpdateSubscriberKey = GetBool(ConstantsNeamb.ExactTargetUpdateSubscriberKey);
            _unionid = GetOptionalString(ConstantsNeamb.Unionid, string.Empty);
			_customerDefinitionDuplicateRegistration =
				GetOptionalString(ConstantsNeamb.CustomerDefinitionDuplicateRegistration, string.Empty);
			_cellCodeDuplicateRegistration = GetOptionalString(ConstantsNeamb.CellCodeDuplicateRegistration, string.Empty);
			_campaignCodeDuplicateRegistration = GetOptionalString(ConstantsNeamb.CampaignCodeDuplicateRegistration, string.Empty);
			_attemptZipCodeValidation = GetOptionalInt(ConstantsNeamb.AttemptZipCodeValidation, 3);
			_enrollmentServiceUser = GetOptionalString(ConstantsNeamb.EnrollmentServiceUser, string.Empty);
			_enrollmentServicePassword = GetOptionalString(ConstantsNeamb.EnrollmentServicePassword, string.Empty);
			_expirationRedisPdf = GetOptionalInt(ConstantsNeamb.ExpirationRedisPdf, 24);
			_customerDefinitionChangeUsername = GetOptionalString(ConstantsNeamb.CustomerDefinitionChangeUsername, string.Empty);
			_cellcodeChangeUsernameNewLogin = GetOptionalString(ConstantsNeamb.CellcodeChangeUsernameNewLogin, string.Empty);
			_cellcodeChangeUsernameOldLogin = GetOptionalString(ConstantsNeamb.CellcodeChangeUsernameOldLogin, string.Empty);
			_campaignCodeChangeUsername = GetOptionalString(ConstantsNeamb.CampaignCodeChangeUsername, string.Empty);
			_urlJeepZag = GetOptionalString(ConstantsNeamb.UrlJeepZag, string.Empty);
			_customerDefinitionSweepstake = GetOptionalString(ConstantsNeamb.CustomerDefinitionSweepstake, string.Empty);
            _customerDefinitionSeminarForm = GetOptionalString(ConstantsNeamb.CustomerDefinitionSeminarForm, string.Empty);
			_cellCodeResetPasswordRequestedReset = GetOptionalString(ConstantsNeamb.CellCodeResetPasswordRequestedReset, string.Empty);
            _cellCodeResetPasswordRequestedResetOldToken = GetOptionalString(ConstantsNeamb.CellCodeResetPasswordRequestedResetOldToken, string.Empty);
            _afiniumEncryptDecryptPassword = GetOptionalString(ConstantsNeamb.AfiniumEncryptDecryptPassword, string.Empty);
			_mercerEncryptDecryptPassword = GetOptionalString(ConstantsNeamb.MercerEncryptDecryptPassword, string.Empty);
			_mercerEncryptDecryptSalt = GetOptionalString(ConstantsNeamb.MercerEncryptDecryptSalt, string.Empty);
            _AESPasswordInteractions = GetOptionalInt(ConstantsNeamb.AESPasswordInteractions, 65536);
            _AESKeySize = GetOptionalInt(ConstantsNeamb.AESKeySize, 256);
			_bucketNameAvatarImages = GetOptionalString(ConstantsNeamb.BucketNameAvatarImages, string.Empty);
			_maxImageSizeAvatar = GetOptionalInt(ConstantsNeamb.MaxImageSizeAvatar, 1);
			_customerDefinitionAddFamilyMember = GetOptionalString(ConstantsNeamb.CustomerDefinitionAddFamilyMember, string.Empty);
			_cellcodeAddFamilyMember = GetOptionalString(ConstantsNeamb.CellcodeAddFamilyMember, string.Empty);
			_campaignAddFamilyMember = GetOptionalString(ConstantsNeamb.CampaignAddFamilyMember, string.Empty);
			_catpchaUrl = GetOptionalString(ConstantsNeamb.CatpchaUrl, string.Empty);
			_catpchaSecret = GetOptionalString(ConstantsNeamb.CatpchaSecret, string.Empty);
			_bucketNameContestImages = GetOptionalString(ConstantsNeamb.BucketNameContestImages, string.Empty);
			_maxFileSizeFileContest = GetOptionalInt(ConstantsNeamb.MaxFileSizeFileContest, 1);
			_s3SubmissionFolder = GetOptionalString(ConstantsNeamb.S3SubmissionFolder, string.Empty);
			_complimentaryLifeProductCode = GetOptionalString(ConstantsNeamb.ComplimentaryLifeProductCode, string.Empty);
			_introLifeProductCode = GetOptionalString(ConstantsNeamb.IntroLifeProductCode, string.Empty);
			_itemErrorMscEfulfillment = GetOptionalString(ConstantsNeamb.ItemErrorMscEfulfillment, string.Empty);
			_productCodeAmericanFidelity = GetOptionalString(ConstantsNeamb.ProductCodeAmericanFidelity, string.Empty);
			_urlAmericanFidelity = GetOptionalString(ConstantsNeamb.UrlAmericanFidelity, string.Empty);
			_productCodeClickAndSave = GetOptionalString(ConstantsNeamb.ProductCodeClickAndSave, string.Empty);
            _urlClickAndSave = GetOptionalString(ConstantsNeamb.UrlClickAndSave, string.Empty);
            _productCodeJeepZag = GetOptionalString(ConstantsNeamb.ProductCodeJeepZag, string.Empty);
			_productCodeMercer = GetOptionalString(ConstantsNeamb.ProductCodeMercer, string.Empty);
			_urlMercer = GetOptionalString(ConstantsNeamb.UrlMercer, string.Empty);
			_maxTabbedHeroItems= GetOptionalInt(ConstantsNeamb.MaxFileSizeFileContest, 0);
			_termsAndConditionsControlIdr = GetOptionalString(ConstantsNeamb.TermsAndConditionsControlId, string.Empty);
			_maxCardCount = GetOptionalInt(ConstantsNeamb.MaxCardCount, 0);
			_pathFileProcessUpdatePublishDate = GetOptionalString(ConstantsNeamb.PathFileProcessUpdatePublishDate, string.Empty);
			_executeProcessUpdatePublishDate = GetBool(ConstantsNeamb.ExecuteProcessUpdatePublishDate);
			_expirationHoursTokenRedis = GetOptionalInt(ConstantsNeamb.ExpirationHoursTokenRedis, 24);
            _expirationHoursSeminaries = GetOptionalInt(ConstantsNeamb.ExpirationHoursSeminaries, 1);
            _expirationCacheOmniEligibility = GetOptionalInt(ConstantsNeamb.ExpirationCacheOmniEligibility, 24);
            _expirationCacheLocalCodesHours = GetOptionalInt(ConstantsNeamb.ExpirationCacheLocalCodesHours, 1);
            _enableCustomFacets = GetBool(ConstantsNeamb.EnableCustomFacets);
            _restUrl = GetOptionalString(ConstantsNeamb.RestUrl, string.Empty);
            _restUserAuthentication = GetOptionalString(ConstantsNeamb.RestUserAuthentication, string.Empty);
            _restPasswordAuthentication = GetOptionalString(ConstantsNeamb.RestPasswordAuthentication, string.Empty);
            _restUrlProductEligibility = GetOptionalString(ConstantsNeamb.RestUrlProductEligibility, string.Empty);
            LifetimeAccessTokenMinutes = GetDouble(ConstantsNeamb.LifetimeAccessTokenMinutes);
            _restUrlProductEligibilitySeiumb = GetOptionalString(ConstantsNeamb.RestUrlProductEligibilitySeiumb, string.Empty);
            _restUrlCompIntroEligibility = GetOptionalString(ConstantsNeamb.RestUrlCompIntroEligibility, string.Empty);
            _restUrlUpdateUserName = GetOptionalString(ConstantsNeamb.RestUrlUpdateUserName, string.Empty);
            _restUrlForgotUserName = GetOptionalString(ConstantsNeamb.RestUrlForgotUserName, string.Empty);
            _restUrlValidateEmailDomain = GetOptionalString(ConstantsNeamb.RestUrlValidateEmailDomain, string.Empty);
            _restUrlDeleteUser = GetOptionalString(ConstantsNeamb.RestUrlDeleteUser, string.Empty);
            _restUrlUpdateUser = GetOptionalString(ConstantsNeamb.RestUrlUpdateUser, string.Empty);
            _restUrlAuthenticatePassword = GetOptionalString(ConstantsNeamb.RestUrlAuthenticatePassword, string.Empty);
            RestUrlRakutenMemberCreation = GetOptionalString(ConstantsNeamb.RestUrlRakutenMemberCreation, string.Empty);
            RestUrlRakutenMemberCreationSEIU = GetOptionalString(ConstantsNeamb.RestUrlRakutenMemberCreationSEIU, string.Empty);
            _afiniumAESPassword = GetOptionalString(ConstantsNeamb.AfiniumAESPassword, string.Empty);
            _mercerAESPassword = GetOptionalString(ConstantsNeamb.MercerAESPassword, string.Empty);
            _afiniumAESSalt = GetOptionalString(ConstantsNeamb.AfiniumAESSalt, string.Empty);
            _mercerAESSalt = GetOptionalString(ConstantsNeamb.MercerAESSalt, string.Empty);
            _restUrlICEGetBalance = GetOptionalString(ConstantsNeamb.RestUrlICEGetBalance, string.Empty);
            #endregion
            #region Rakuten Constructors
            RakutenMemberSource = GetOptionalString(ConstantsNeamb.RakutenMemberSource, string.Empty);
            RakutenSEIUMemberSource = GetOptionalString(ConstantsNeamb.RakutenSEIUMemberSource, string.Empty);
            RakutenLocale = GetOptionalString(ConstantsNeamb.RakutenLocale, string.Empty);
            RakutenSignature = GetOptionalString(ConstantsNeamb.RakutenSignature, string.Empty);
            RakutenSEIUSignature = GetOptionalString(ConstantsNeamb.RakutenSEIUSignature, string.Empty);
            _rakutenServerApiUrl = GetOptionalString(ConstantsNeamb.RakutenServerApiUrl, string.Empty);
            _rakutenStoreApiUrl = GetOptionalString(ConstantsNeamb.RakutenStoreApiUrl, string.Empty);
            _rakutenStoreDetailApiUrl = GetOptionalString(ConstantsNeamb.RakutenStoreDetailApiUrl, string.Empty);
            _rakutenStoreChannel = GetOptionalString(ConstantsNeamb.RakutenStoreChannel, string.Empty);
            _rakutenSeiumbStoreChannel = GetOptionalString(ConstantsNeamb.RakutenSeiumbStoreChannel, string.Empty);
            _expirationRedisEtag = GetOptionalInt(ConstantsNeamb.ExpirationRedisEtag, 24);
            _rakutenMediaServerUrl = GetOptionalString(ConstantsNeamb.RakutenMediaServerUrl, string.Empty);
            _rakutenShoppingBaseUrl = GetOptionalString(ConstantsNeamb.RakutenShoppingBaseUrl, string.Empty);
            _rakutenPartnerId = GetOptionalString(ConstantsNeamb.RakutenPartnerId, string.Empty);
            _rakutenSeiumbPartnerId = GetOptionalString(ConstantsNeamb.RakutenSeiumbPartnerId, string.Empty);
            #endregion
            _urlEfulfillmentS3Seiumb = GetOptionalString(ConstantsNeamb.UrlEfulfillmentS3Seiumb, string.Empty);
            _urlEfulfillmentS3SeiumbExternal = GetOptionalString(ConstantsNeamb.UrlEfulfillmentS3SeiumbExternal, string.Empty);
            _productCodeStores = GetOptionalString(ConstantsNeamb.ProductCodeStores, string.Empty);
            _compLifeEmailCellCodeToteBag = GetOptionalString(ConstantsNeamb.CompLifeEmailCellCodeToteBag, string.Empty);
            _rakutenCategoriesParentID = GetOptionalString(ConstantsNeamb.RakutenCategoriesParentID, string.Empty);
            _restUrlSearchUserName = GetOptionalString(ConstantsNeamb.RestUrlSearchUserName, string.Empty);
            _restUrlRetrieveUser = GetOptionalString(ConstantsNeamb.RestUrlRetrieveUser, string.Empty);
            _storesSkipImportProcess = GetOptionalString(ConstantsNeamb.StoresSkipImportProcess, string.Empty);
            _restUrlLogin = GetOptionalString(ConstantsNeamb.RestUrlLogin, string.Empty);
            _restUrlUpdatePassword = GetOptionalString(ConstantsNeamb.RestUrlUpdatePassword, string.Empty);
            _restUrlUpdateUserStatus = GetOptionalString(ConstantsNeamb.RestUrlUpdateUserStatus, string.Empty);
            _restUrlResetPassword = GetOptionalString(ConstantsNeamb.RestUrlResetPassword, string.Empty);
            _referrerIdJeepZag = GetOptionalString(ConstantsNeamb.ReferrerId, string.Empty);
            _headerTrueCarToken = GetOptionalString(ConstantsNeamb.HeaderTrueCarToken, string.Empty);
            _urlTrueCarSeiumb = GetOptionalString(ConstantsNeamb.UrlTrueCarSeiumb, string.Empty);
            _referrerIdSeiumb = GetOptionalString(ConstantsNeamb.ReferrerIdSeiumb, string.Empty);
            _headerTrueCarTokenSeiumb = GetOptionalString(ConstantsNeamb.HeaderTrueCarTokenSeiumb, string.Empty);
            _restUrlRegisterUser = GetOptionalString(ConstantsNeamb.RestUrlRegisterUser, string.Empty);
            _restUrlCreateResetToken = GetOptionalString(ConstantsNeamb.RestUrlCreateResetToken, string.Empty);
            _restUrlValidateResetToken = GetOptionalString(ConstantsNeamb.RestUrlValidateResetToken, string.Empty);
            _restUrlCancelResetToken = GetOptionalString(ConstantsNeamb.RestUrlCancelResetToken, string.Empty);
            _restUrlEfulfillment = GetOptionalString(ConstantsNeamb.RestUrlEfulfillment, string.Empty);
            _restUrlAESEncrypt = GetOptionalString(ConstantsNeamb.RestUrlAESEncrypt, string.Empty);
            _restUrlAESDecrypt = GetOptionalString(ConstantsNeamb.RestUrlAESDecrypt, string.Empty);
            _productCodeStoresSeiumb = GetOptionalString(ConstantsNeamb.ProductCodeStoresSeiumb, string.Empty);
            _timeZoneRangeTimePersonalizationRule = GetOptionalString(ConstantsNeamb.TimeZoneRangeTimePersonalizationRule, string.Empty);
            _specialCharacterSolr = GetOptionalString(ConstantsNeamb.SpecialCharacterSolr, string.Empty);
            _excludeCommaSpecialCharacterSolr = GetBool(ConstantsNeamb.ExcludeCommaSpecialCharacterSolr);
            _excludeAmpSpecialCharacterSolr = GetBool(ConstantsNeamb.ExcludeAmpSpecialCharacterSolr);
            _expirationMinutesCacheEligibilityMarketplace = GetOptionalInt(ConstantsNeamb.ExpirationMinutesCacheEligibilityMarketplace, 30);
            _neambIndex = GetOptionalString(ConstantsNeamb.NeambIndex, string.Empty);
            _seiumbIndex = GetOptionalString(ConstantsNeamb.SeiumbIndex, string.Empty);
            _storeIndex = GetOptionalString(ConstantsNeamb.StoreIndex, string.Empty);
            _indicatorNoPermissionEmail = GetOptionalString(ConstantsNeamb.IndicatorNoPermissionEmail, string.Empty);
            _indicatorYesPermissionEmail = GetOptionalString(ConstantsNeamb.IndicatorYesPermissionEmail, string.Empty);
            _typePermissionEmail = GetOptionalString(ConstantsNeamb.TypePermissionEmail, string.Empty);
            _businessUnitPermissionEmail = GetOptionalString(ConstantsNeamb.BusinessUnitPermissionEmail, string.Empty);
            _captchaKeySeiumb = GetOptionalString(ConstantsNeamb.CaptchaKeySeiumb, string.Empty);
            _captchaSecretSeiumb = GetOptionalString(ConstantsNeamb.CaptchaSecretSeiumb, string.Empty);
        }

        #region Retrieve Options
        private string GetOptionalString(string name, string defaultValue) {
			// Try to get the value from the Sitecore configuration file.
			var value = config.Settings.GetSetting(name);
			if (value == null) // Not found ? Return the default value
                return (defaultValue);
            // Return the value
			return (value);
		}

		protected int GetOptionalInt(string name, int defaultValue) {
			// Try to get the value from the configuration file.
			var value = config.Settings.GetSetting(name);
			if (value == null) // Not found ? Return the default value
                return (defaultValue);

            try {
				// Try to convert it to an integer
				return (Convert.ToInt32(value));
			} catch (Exception e) {
				throw e;
			}
		}

		protected TimeSpan GetTimeSpan(string name) {
			var value = config.Settings.GetSetting(name);
			if (!string.IsNullOrEmpty(value)) // Not found ? Return the default value
                return TimeSpan.FromDays(Convert.ToInt32(value));
            else
                return TimeSpan.Zero;
        }

		protected ID GetId(string name)
		{
			var value = config.Settings.GetSetting(name);
			if (!String.IsNullOrEmpty(value)) // Not found ? Return the default value
                return new ID(value);
            else
                return ID.Null;
        }

		protected bool GetBool(string name)
		{
			var value = config.Settings.GetSetting(name);
			if (!string.IsNullOrEmpty(value)) // Not found ? Return the default value
                return Convert.ToBoolean(value);
            else
                return false;
        }

        protected double GetDouble(string name)
        {
            var value = config.Settings.GetSetting(name);
            if (!string.IsNullOrEmpty(value)) // Not found ? Return the default value
                return Convert.ToDouble(value);
            return 0;
        }
        #endregion
    }
}