using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.ComponentModel.DataAnnotations;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Feature.Account.Models
{
	public class InviteFamilyMemberDTO : IRenderingModel
	{
		public Rendering Rendering { get; set; }
		public Item Item { get; set; }
		public Item PageItem { get; set; }
		public StatusEnum UserStatus { get; set; }
		public List<FamilyMemberItemList> FamilyMemberList { get; set; }
		public bool IsRedirectionAdd { get; set; }
		public bool HasGeneralError { get; set; }
		public int LimitRecords { get; set; }
		public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			PageItem = PageContext.Current.Item;
			UserStatus = StatusEnum.Unknown;
			FamilyMemberList= new List<FamilyMemberItemList>();
			IsRedirectionAdd = false;
			HasGeneralError = false;
			LimitRecords = 0;
		}
	}
}