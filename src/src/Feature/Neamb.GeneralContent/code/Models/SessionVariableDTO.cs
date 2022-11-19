using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.GeneralContent.Models
{
    public class SessionVariableDTO : IRenderingModel
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public String SessionVariable { get; set; }
        public Dictionary<string, string> SessionVariableDictionary{ get; set; }
        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            SessionVariable = Item[Templates.SessionVariable.Fields.SessionVariables];
        }
    }
}