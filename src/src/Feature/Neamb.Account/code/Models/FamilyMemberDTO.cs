using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Config.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Sitecore.Mvc.Presentation;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Feature.Account.Models
{
	public class FamilyMemberDTO : ProfileBasicDTO, IRenderingModel
	{
		public StatusEnum UserStatus { get; set; }
		public List<SelectListItem> RelationshipList { get; set; }

		[Required(ErrorMessage = ConstantsNeamb.ValidationRequired)]
		public string Relationship { get; set; }
		public bool HasTooltipRelationship { get; set; }
		public string BackText { get; set; }
		public string BackUrl { get; set; }
		public void Initialize(Rendering rendering)
		{
			Rendering = rendering;
			Item = rendering.Item;
			UserStatus = StatusEnum.Unknown;
			//RelationshipList = GetRelationshipItems();
			HasTooltipRelationship = false;
			ErrorsBirthDate = new List<ErrorStatusEnum>();
			ErrorsEmail = new List<ErrorStatusEnum>();
			ErrorsFirstName = new List<ErrorStatusEnum>();
			ErrorsLastName = new List<ErrorStatusEnum>();
			HasTooltipBirthDate = false;
			HasTooltipEmail = false;
			HasTooltipFirstName = false;
			HasTooltipLastName = false;
			ProcessedSucessfully = false;
			Sitecore.Data.Fields.LinkField backLink = Item.Fields[Templates.FamilyMember.Fields.Back];
			if (backLink != null && backLink.TargetItem != null)
			{
				BackUrl = Sitecore.Links.LinkManager.GetItemUrl(backLink.TargetItem);
				BackText = backLink.Text;
			}
		}
	}
}