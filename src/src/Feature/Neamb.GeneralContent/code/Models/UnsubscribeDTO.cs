using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
	public class UnsubscribeDTO
	{
		public Item Item { get; set; }
		public bool IsSucess { get; set; }
		public void Initialize(Rendering rendering)
		{
			Item = rendering.Item;
			IsSucess = false;
		}
	}
}