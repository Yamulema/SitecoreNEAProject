using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class Step
    {
        public bool IsAnonymous { get; set; }
        public bool IsExperienceEditor { get; set; }
        public WizardHeader Header { get; set; }
        public Item Datasource { get; set; }
        public bool IsInnerStep { get; set; }
		public string GtmAction { get; set; }
	}
}