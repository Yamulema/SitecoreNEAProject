using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.SchemaMarkup.Models
{
    public class SchemaMarkupModel
    {
        public List<string> ScriptContent { get; set; }

        public SchemaMarkupModel()
        {
            ScriptContent = new List<string>();
        }
    }
}