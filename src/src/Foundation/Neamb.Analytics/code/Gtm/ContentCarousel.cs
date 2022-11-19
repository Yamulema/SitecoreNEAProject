using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class ContentCarousel
	{
        public string Event { get; set; }
        public string ContentTitle { get; set; }
        public string ContentLocation { get; set; }
        public ContentCarousel() {
            Event = "content";
        }
    }
}