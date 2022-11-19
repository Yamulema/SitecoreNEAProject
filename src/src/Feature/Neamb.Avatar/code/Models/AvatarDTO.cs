using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.Membership.Model;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Avatar.Models
{
	public class AvatarDTO : IRenderingModel
	{
		public Item Item { get; set; }
		public Item PageItem { get; set; }
		public Rendering Rendering { get; set; }
		public StatusEnum UserStatus { get; set; }
		public string UserImageUrl { get; set; }
		public string BackText { get; set; }
		public string BackUrl { get; set; }
		public bool IsProcessedSucessfully { get; set; }
		public bool IsInAvatarPage { get; set; }

		public void Initialize(Rendering rendering)
		{
			if (rendering != null) {
				Rendering = rendering;
				Item = rendering.Item;
				PageItem = PageContext.Current.Item;
				UserStatus = StatusEnum.Unknown;
				Sitecore.Data.Fields.LinkField backLink = Item.Fields[Templates.Avatar.Fields.BackLink];
				if (backLink != null && backLink.TargetItem != null) {
					BackUrl = Sitecore.Links.LinkManager.GetItemUrl(backLink.TargetItem);
					BackText = backLink.Text;
				}

				IsProcessedSucessfully = false;
				IsInAvatarPage = PageItem.IsDerived(Templates.PageAvatar.ID);
			}
		}
	}
}