using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Feature.GeneralContent.Interfaces
{
    public interface IRenderFieldProcessor {
        void Process(RenderFieldArgs args);
    }
}