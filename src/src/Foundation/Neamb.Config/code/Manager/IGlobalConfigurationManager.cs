using System;

namespace Neambc.Neamb.Foundation.Configuration.Manager {
    public interface IGlobalConfigurationManager {

        #region Unclassified
        /// <summary>NEMBC partner</summary>
        string NeambcPartner { get; set; }

        /// <summary>NEMBC key</summary>
        string NeambcKey { get; set; }

        /// <summary>NEMBC Union ID</summary>
        string NeambcUnionId { get; set; }

        /// <summary>NEMBC Matchroutineidentifier</summary>
        string NeambcKeyMatchRoutineIdentifier { get; }

        /// <summary>Service timeout</summary>
        int ServiceTimeout { get; set; }

        /// <summary>Exact target user name for authenticate</summary>
        string ExacttargetUsername { get; }

        /// <summary>Exact target password for authenticate</summary>
        string ExacttargetPassword { get; }

        /// <summary>Cellcode value for Exact target in reset password feature</summary>
        string CellCodeResetPasswordLockedOut { get; }

        /// <summary>Campaign value for Exact target in reset password feature</summary>
        string CampaignResetPassword { get; }

        /// <summary>Customer definition value for Exact target in reset password feature</summary>
        string CustomerDefinitionResetPassword { get; }

        /// <summary>Exact target endpoint</summary>
        string ExacttargetEndPoint { get; }

        /// <summary>Client id in exact target</summary>
        int ExacttargetClientId { get; }

        /// <summary>Time in days for the cookie warm</summary>
        TimeSpan TimeWarmCookie { get; }

        /// <summary>Time in days for the cookie remember me</summary>
        TimeSpan TimeRemembermeCookie { get; }

        /// <summary>
        /// Registration Webusersource parameter to send the webservice
        /// </summary>
        string Webusersource { get; }

        /// <summary>
        /// Flag to enable or disable change of subscriber key for test users
        /// </summary>
        bool ExactTargetUpdateSubscriberKey { get; }

        /// <summary>
        /// Registration Unionid parameter to send the webservice
        /// </summary>
        string Unionid { get; }

        /// <summary>
        /// Customer key Duplicate registration email
        /// </summary>
        string CustomerDefinitionDuplicateRegistration { get; }

        /// <summary>
        /// Cellcode duplicate registration email
        /// </summary>
        string CellCodeDuplicateRegistration { get; }

        /// <summary>
        /// Campaign code duplicate registration email
        /// </summary>
        string CampaignCodeDuplicateRegistration { get; }

        /// <summary>
        /// AttemptZipCodeValidation
        /// </summary>
        int AttemptZipCodeValidation { get; }

        /// <summary>
        /// Enrollment ServiceUser used in American Fidelity partner
        /// </summary>
        string EnrollmentServiceUser { get; }

        /// <summary>
        /// Enrollment Service Password used in American Fidelity partner
        /// </summary>
        string EnrollmentServicePassword { get; }

        int ExpirationRedisPdf { get; }
        /// <summary>
        /// Customer key Change username email
        /// </summary>
        string CustomerDefinitionChangeUsername { get; }

        /// <summary>
        /// Cell code Change username email. New login
        /// </summary>
        string CellcodeChangeUsernameNewLogin { get; }
        /// <summary>
        /// Cell code Change username email. Old login
        /// </summary>
        string CellcodeChangeUsernameOldLogin { get; }

        /// <summary>
        /// Campaign code Change username email
        /// </summary>
        string CampaignCodeChangeUsername { get; }
        /// <summary>
        /// Url to call the partner jeep and zag
        /// </summary>
        string UrlJeepZag { get; }

        /// <summary>
        /// Customer Definition Sweep stake email
        /// </summary>
        string CustomerDefinitionSweepstake { get; }

        /// <summary>
        /// Cell code reset password requested by the user
        /// </summary>
        string CellCodeResetPasswordRequestedReset { get; }
        /// <summary>
        /// Cell code reset password requested by the user for old tokens
        /// </summary>
        string CellCodeResetPasswordRequestedResetOldToken { get; }

        /// <summary>
		/// MercerEncryptDecryptPassword
		/// </summary>
		string AfiniumEncryptDecryptPassword { get; }

        /// <summary>
        /// MercerEncryptDecryptPassword
        /// </summary>
        string MercerEncryptDecryptPassword { get; }

        /// <summary>
        /// MercerEncryptDecryptSalt
        /// </summary>
        string MercerEncryptDecryptSalt { get; }

        /// <summary>
        /// AESPasswordInteractions
        /// </summary>
        int AESPasswordInteractions { get; }

        /// <summary>
        /// AESKeySize
        /// </summary>
        int AESKeySize { get; }
        /// <summary>
        /// BucketNameAvatarImages
        /// </summary>
        string BucketNameAvatarImages { get; }
        /// <summary>
        /// MaxImageSizeAvatar
        /// </summary>
        int MaxImageSizeAvatar { get; }
        /// <summary>
        /// MaxImageSizeAvatar
        /// </summary>
        int MaxFileSizeFileContest { get; }
        /// <summary>
        /// CustomerDefinitionAddFamilyMember
        /// </summary>
        string CustomerDefinitionAddFamilyMember { get; }

        /// <summary>
        /// CellcodeAddFamilyMember
        /// </summary>
        string CellcodeAddFamilyMember { get; }
        /// <summary>
        /// CampaignAddFamilyMember
        /// </summary>
        string CampaignAddFamilyMember { get; }
        string CatpchaUrl { get; }
        string CatpchaSecret { get; }
        /// <summary>
        /// BucketNameAvatarImages
        /// </summary>
        string BucketNameContestImages { get; }
        /// <summary>
        /// S3SubmissionFolder
        /// </summary>
        string S3SubmissionFolder { get; }
        /// <summary>
        /// ComplimentaryLifeProductCode
        /// </summary>
        string ComplimentaryLifeProductCode { get; }
        /// <summary>
        /// IntroLifeProductCode
        /// </summary>
        string IntroLifeProductCode { get; }
        /// <summary>
        /// ItemErrorMscEfulfillment
        /// </summary>
        string ItemErrorMscEfulfillment { get; }
        string ProductCodeAmericanFidelity { get; }
        string UrlAmericanFidelity { get; }
        string ProductCodeClickAndSave { get; }
        string UrlClickAndSave { get; }
        string ProductCodeJeepZag { get; }
        string ProductCodeMercer { get; }
        string UrlMercer { get; }
        int MaxTabbedHeroItems { get; }
        string TermsAndConditionsControlId { get; }
        int MaxCardCount { get; }
        string PathFileProcessUpdatePublishDate { get; }
        bool ExecuteProcessUpdatePublishDate { get; }
        /// <summary>ExpirationHoursTokenRedis</summary>
        int ExpirationHoursTokenRedis { get; }
        int ExpirationHoursSeminaries { get; }
        /// <summary>
        /// CustomerDefinitionSeminarForm
        /// </summary>
        string CustomerDefinitionSeminarForm { get; }
        int ExpirationCacheOmniEligibility { get; }
        int ExpirationCacheLocalCodesHours { get; }
        bool EnableCustomFacets { get; }
        string UrlEfulfillmentS3 { get; }
        string UrlEfulfillmentS3External { get; }
        /// <summary>
		/// URL to get the Rest services
		/// </summary>
		string RestUrl { get; }
        /// <summary>
        /// User to get the Rest services
        /// </summary>
        string RestUserAuthentication { get; }
        /// <summary>
        /// Pwd to get the Rest services
        /// </summary>
        string RestPasswordAuthentication { get; }
        /// <summary>
        /// Url of the rest services for products
        /// </summary>
        string RestUrlProductEligibility { get; }
        /// <summary>
        /// Url of the rest services for products Seiumb
        /// </summary>
        string RestUrlProductEligibilitySeiumb { get; }

        /// <summary>
        /// Minutes substracted to set lifetime of Access Token in Redis
        /// </summary>
        double LifetimeAccessTokenMinutes { get; }

        /// <summary>
        /// Url of the rest services for comp/intro life products
        /// </summary>
        string RestUrlCompIntroEligibility { get; }

        string RestUrlUpdateUserName { get; }
        string RestUrlForgotUserName { get; }
        string RestUrlValidateEmailDomain { get; }
        string RestUrlDeleteUser { get; }
        string RestUrlUpdateUser { get; }
        string RestUrlAuthenticatePassword { get; }
        string RestUrlSearchUserName { get; }
        string RestUrlRetrieveUser { get; }
        /// <summary>
        /// CompLifeEmailCellCodeToteBag
        /// </summary>
        string CompLifeEmailCellCodeToteBag { get; }
        /// <summary>
        /// UrlEfulfillmentS3Seiumb
        /// </summary>
        string UrlEfulfillmentS3Seiumb { get; }
        /// <summary>
        /// UrlEfulfillmentS3SeiumbExternal
        /// </summary>
        string UrlEfulfillmentS3SeiumbExternal { get; }
        /// <summary>
        /// AfiniumAESPassword
        /// </summary>
        string AfiniumAESPassword { get; }
        /// <summary>
        /// MercerAESPassword
        /// </summary>
        string MercerAESPassword { get; }
        /// <summary>
        /// AfiniumAESSalt
        /// </summary>
        string AfiniumAESSalt { get; }
        /// <summary>
        /// MercerAESSalt
        /// </summary>
        string MercerAESSalt { get; }
        /// <summary>
        /// StoresSkipImportProcess
        /// </summary>
        string StoresSkipImportProcess { get; }
        /// <summary>
        /// RestUrlLogin
        /// </summary>
        string RestUrlLogin { get; }
        /// <summary>
        /// RestUrlUpdatePassword
        /// </summary>
        string RestUrlUpdatePassword { get; }
        /// <summary>
        /// RestUrlUpdateUserStatus
        /// </summary>
        string RestUrlUpdateUserStatus { get; }
        /// <summary>
        /// RestUrlResetPassword
        /// </summary>
        string RestUrlResetPassword { get; }
        /// <summary>
        /// ReferrerIdJeepZag
        /// </summary>
        string ReferrerIdJeepZag { get; }
        /// <summary>
        /// HeaderTrueCarToken
        /// </summary>
        string HeaderTrueCarToken { get; }
        /// <summary>
        /// UrlTrueCarSeiumb
        /// </summary>
        string UrlTrueCarSeiumb { get; }
        /// <summary>
        /// ReferrerIdSeiumb
        /// </summary>
        string ReferrerIdSeiumb { get; }
        /// <summary>
        /// HeaderTrueCarTokenSeiumb
        /// </summary>
        string HeaderTrueCarTokenSeiumb { get; }
        /// <summary>
        /// RestUrlRegisterUser
        /// </summary>
        string RestUrlRegisterUser { get; }
        /// <summary>
        /// RestUrlCreateResetToken
        /// </summary>
        string RestUrlCreateResetToken { get; }
        /// <summary>
        /// RestUrlValidateResetToken
        /// </summary>
        string RestUrlValidateResetToken { get; }
        /// <summary>
        /// RestUrlCancelResetToken
        /// </summary>
        string RestUrlCancelResetToken { get; }
        /// <summary>
        /// RestUrlEfulfillment
        /// </summary>
        string RestUrlEfulfillment { get; }
        /// <summary>
        /// RestUrlAESEncrypt
        /// </summary>
        string RestUrlAESEncrypt { get; }
        /// <summary>
        /// RestUrlAESDecrypt
        /// </summary>
        string RestUrlAESDecrypt { get; }
        /// <summary>
        /// ProductCodeStoresSeiumb
        /// </summary>
        string ProductCodeStoresSeiumb { get; }
        /// <summary>
        /// RestUrlAESDecrypt
        /// </summary>
        string RestUrlICEGetBalance { get; }
        /// <summary>
        /// TimeZoneRangeTimePersonalizationRule
        /// </summary>
        string TimeZoneRangeTimePersonalizationRule { get; }
        /// <summary>
        /// SpecialCharacterSolr
        /// </summary>
        string SpecialCharacterSolr { get; }
        /// <summary>
        /// ExcludeCommaSpecialCharacterSolr
        /// </summary>
        bool ExcludeCommaSpecialCharacterSolr { get; }
        /// <summary>
        /// ExcludeAmpSpecialCharacterSolr
        /// </summary>
        bool ExcludeAmpSpecialCharacterSolr { get; }
        /// <summary>
        /// ExpirationMinutesCacheEligibilityMarketplace
        /// </summary>
        int ExpirationMinutesCacheEligibilityMarketplace { get; }
        /// <summary>
        /// Indicator to prevent to send emails to the user
        /// </summary>
        string IndicatorNoPermissionEmail { get; }
        /// <summary>
        /// Indicator to allow to send emails to the user
        /// </summary>
        string IndicatorYesPermissionEmail { get; }
        /// <summary>
        /// Permission type "MAIL"
        /// </summary>
        string TypePermissionEmail { get; }
        /// <summary>
        /// Business unit for permission email. Example : "MB"
        /// </summary>
        string BusinessUnitPermissionEmail { get; }
        /// <summary>
        /// Captcha secret for seiumb domain
        /// </summary>
        string CaptchaSecretSeiumb { get; }
        /// <summary>
        /// Captcha key for seiumb domain
        /// </summary>
        string CaptchaKeySeiumb { get; }
        #endregion

        #region Rakuten
        /// <summary>
        /// Url of the Rakuten Creation Member API
        /// </summary>
        string RestUrlRakutenMemberCreation { get; }
        string RestUrlRakutenMemberCreationSEIU { get; }
        /// <summary>
        /// Url of the Rakuten Request Header Locale property
        /// </summary>
        string RakutenLocale { get; }
        /// <summary>
        /// Url of the Rakuten Request Header Signature property
        /// </summary>
        string RakutenSignature { get; }
        string RakutenSEIUSignature { get; }
        /// <summary>
        /// Url of the Rakuten Request Header MemberSource property
        /// </summary>
        string RakutenMemberSource { get; }
        string RakutenSEIUMemberSource { get; }
        /// <summary>
        /// RakutenServerApiUrl
        /// </summary>
        string RakutenServerApiUrl { get; }
        /// <summary>
        /// RakutenStoreApiUrl
        /// </summary>
        string RakutenStoreApiUrl { get; }
        /// <summary>
        /// RakutenStoreDetailApiUrl
        /// </summary>
        string RakutenStoreDetailApiUrl { get; }
        /// <summary>
        /// RakutenStoreChannel
        /// </summary>
        string RakutenStoreChannel { get; }
        /// <summary>
        /// RakutenStoreChannel
        /// </summary>
        string RakutenSeiumbStoreChannel { get; }
        /// <summary>
        /// ExpirationRedisEtag
        /// </summary>
        int ExpirationRedisEtag { get; }
        string RakutenMediaServerUrl { get; }
        string RakutenShoppingBaseUrl { get; }
        string RakutenPartnerId { get; }
        string RakutenSeiumbPartnerId { get; }
        
        /// <summary>
        /// ProductCodeStores
        /// </summary>
        string ProductCodeStores { get; }
        string RakutenCategoriesParentID { get; }
        #endregion
        #region Indexes
        string NeambIndex { get; }
        string SeiumbIndex { get; }
        string StoreIndex { get;}
        #endregion
    }
}