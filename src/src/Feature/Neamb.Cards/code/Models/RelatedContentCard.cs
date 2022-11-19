using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.Cards.Repositories;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public class RelatedContentCard : PageCard
    {
        public string ThumbnailAlt { get; set; }
        public string ThumbnailSrc { get; set; }
        public string Genre { get; set; }
		public string GtmAction { get; set; }
	}
}