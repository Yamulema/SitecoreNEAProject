using System;
using System.Collections.Generic;
using System.Xml;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Foundation.Product.Pipelines
{
    public class PartnerBase
    {
        #region properties
        protected List<string> programCodes;
        protected Dictionary<string, string> Configuration { get; set; }
        #endregion
        public PartnerBase()
        {
            programCodes = new List<string>();
            Configuration = new Dictionary<string, string>();
        }
        
        public void AddProgramCode(string programCode)
        {
            Assert.ArgumentNotNullOrEmpty(programCode, "programCode");
            programCodes.Add(programCode);
        }

        public void Config(XmlNode node)
        {
            Configuration.Add(node.Name, node.InnerText);
        }
        public void SetAction(string actionUrl, ProductPipelineArgs args)
        {
            if (string.IsNullOrEmpty(args.Result))
            {
                args.Result = actionUrl;
            }
            else if (string.IsNullOrEmpty(args.SecondaryUrl))
            {
                args.SecondaryUrl = actionUrl;
            }
        }
    }
}