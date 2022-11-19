using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class WizardHeader
    {
        public string StepText { get; set; }
        public string LogoUrl { get; set; }
        public WizardButton Back { get; set; }
        public WizardButton Next { get; set; }
        public WizardButton End { get; set; }
    }
}