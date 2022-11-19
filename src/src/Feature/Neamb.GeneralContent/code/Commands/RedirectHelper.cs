using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Commands
{
	public class RedirectHelper
	{
		public string RequestedUrl { get; set; }
		public Item RedirectItem { get; set; }
		public string RedirectUrl { get; set; }

	}
}