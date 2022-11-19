using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.ComponentModel.DataAnnotations;
using Neambc.Neamb.Feature.Account.Enums;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Feature.Account.Models
{
	public class AccountDTO : IRenderingModel
	{
		[Required] public string Email { get; set; }
		[Required] public string Password { get; set; }
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }
		public Item PageItem { get; set; }
		public bool HasErrorInvalidCredentials { get; set; }
		public bool HasAlreadyLockedErrorTokenValid { get; set; }
		public bool HasLockedError { get; set; }
		public bool HasErrorUserName { get; set; }
		public bool HasErrorPassword { get; set; }
		public bool HasErrorTimeout { get; set; }
        public bool IsRememberMe { get; set; }
        public bool IsValid { get; set; }
		public string LoginText { get; set; }
		public bool IsAlreadyRegistered { get; set; }
		public bool FromProduct { get; set; }
		public string CtaAction { get; set; }
		public string CtaActionClick { get; set; }
		public string CtaActionTargetBlank { get; set; }
        public string ProductCode { get; set; }
        public string GtmAction { get; set; }
        public bool HasCheckEligibility { get; set; }
        public string CtaFirstSecondAction { get; set; }
        public LoginAjaxEnum LoginAjaxProcess { get; set; }
        public string StoreId { get; set; }
        public string RegistrationUrl { get; set; }
        public string RegistrationText { get; set; }
		public string GtmLoadAction { get; set; }
		public string GtmLoginFailed { get; set; }
		public bool IsPost { get; set; }

		public StatusEnum Status
		{
			get; set;
		}
		public void Initialize(Rendering rendering)
		{
			IsValid = false;
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
			IsAlreadyRegistered = false;
            HasCheckEligibility = true;
            LoginAjaxProcess = LoginAjaxEnum.None;
        }
	}
}