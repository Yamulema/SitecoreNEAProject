using Sitecore.Data.Items;

namespace Neambc.Seiumb.Feature.Product.Models
{
    public class UserStateProduct
    {
        public string ProductCode { get; set; }
        public Item ContextItem { get; set; }
		public Item PageItem { get; set; }
		public string Action { get; set; }
        public UserData UserData { get; set; }
		public string Action1Text { get; set; }
		public string Action2Text { get; set; }
		public string Action3Text { get; set; }
        public string Token { get; set; }
		public ActionTypeEnum ActionType1 { get; set; }
        public ActionTypeEnum ActionType2 { get; set; }
        public ActionTypeEnum ActionType3 { get; set; }
        public string MaterialId { get; set; }
        public string RegistrationLinkUrl { get; set; }
        public string RegistrationText { get; set; }
        public bool HasCheckEligibility { get; set; }
		public bool HasEligibility { get; set; }
        public string ActionFirstUrl { get; set; }
        public string ActionFirstClick { get; set; }
        public string ActionColdClick { get; set; }
        public string ActionColdClickRegister { get; set; }
        public string ActionColdClickMobile { get; set; }
        public string ActionFirstText { get; set; }
		public string ActionFirstTitle { get; set; }
        public string ActionFirstTarget { get; set; }
        public string ActionSecondUrl { get; set; }
        public string ActionSecondClick { get; set; }
        public string ActionSecondText { get; set; }
		public string ActionSecondTitle { get; set; }
        public string ActionSecondTarget { get; set; }
		public string ActionThirdUrl { get; set; }
        public string ActionThirdClick { get; set; }
        public string ActionThirdText { get; set; }
		public string ActionThirdTitle { get; set; }
        public string ActionThirdTarget { get; set; }
        public string LoginDesktopText { get; set; }
        public string LoginMobileText { get; set; }
        public string LoginMobileLinkUrl { get; set; }
        public bool HasAction1 { get; set; }
        public bool HasAction2 { get; set; }
        public bool HasAction3 { get; set; }
        public string AuthenticationStatus { get; set; }
		public string LoginModalUrlFirst { get; set; }
        public string LoginModalOnClickFirst { get; set; }
        public string LoginModalOnClickFirstV2 { get; set; }
        public string LoginModalUrlSecond { get; set; }
        public string LoginModalOnClickSecond { get; set; }
        public string LoginModalOnClickSecondV2 { get; set; }
        public string LoginModalUrlThird { get; set; }
        public string LoginModalOnClickThird { get; set; }
        public string LoginModalOnClickThirdV3 { get; set; }

    }
}

