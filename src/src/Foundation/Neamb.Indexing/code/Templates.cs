using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Foundation.Indexing
{
    public struct Templates
    {
        public struct PageTypeTemplates
        {
            public static readonly ID Article = new ID("{15547760-E485-446C-B7C0-98A660FD577E}");
            public static readonly ID VideoPage = new ID("{F228472C-8A4D-4D12-8CC2-216138E6A43A}");
            public static readonly ID Product = new ID("{D1889EB8-BE95-4E99-B8E9-3A0AEB8F4800}");
            public static readonly ID ProductCategory = new ID("{EEA0A232-C12B-4422-99C4-216A4E16FCDF}");
        }
    }
}