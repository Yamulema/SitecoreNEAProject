using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Cards.Models
{
    public class TabbedHeroItem
    {
        public string Text { get; set; }
        public string ImageSrc { get; set; }
        public string ImageAlt { get; set; }
        public string Cta { get; set; }
        public string Target { get; set; }
		public string GtmAction { get; set; }
	}
}