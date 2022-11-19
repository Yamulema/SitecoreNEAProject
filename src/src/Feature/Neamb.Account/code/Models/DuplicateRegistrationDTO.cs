using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.Account.Models
{
	public class DuplicateRegistrationDTO
	{
		public string CurrentEmail { get; set; }
		public List<EmailDuplicate> EmailList { get; set; }
		public Item Item { get; set; }
		public string RedirectAction { get; set; }
		public string MessageSelectedEmailPart1 { get; set; }
		public string MessageSelectedEmailPart2 { get; set; }
		public string MessageUnSelectedEmailPart1 { get; set; }
		public string MessageUnSelectedEmailPart2 { get; set; }
		public string ThankYouUrl { get; set; }
		public string ThankYouUrlText { get; set; }
		public bool HasGeneralError { get; set; }

		public void Initialize(Rendering rendering)
		{
			Item = rendering.Item;
			EmailList = new List<EmailDuplicate>();
		}
	}

	public class EmailDuplicate
	{
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Dob { get; set; }
		public string Webuserid { get; set; }
	}
}