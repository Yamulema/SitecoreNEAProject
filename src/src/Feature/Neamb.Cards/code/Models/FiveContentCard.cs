using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public class FiveContentCard : PageCard
    {
        public string ThumbnailSrc { get; set; }
        public string ThumbnailAlt { get; set; }
        public string Genre { get; set; }
		public string GtmAction { get; set; }
	}
}