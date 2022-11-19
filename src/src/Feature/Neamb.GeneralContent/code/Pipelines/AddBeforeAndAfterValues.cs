using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Pipelines;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Feature.GeneralContent.Pipelines
{
    public class AddBeforeAndAfterValues : IRenderFieldProcessor
    {
        private readonly IEnumerable<Tuple<IStringProcessor, bool>> _stringProcessors;
        public AddBeforeAndAfterValues(IDownloadProcessor downloadProcessor) {
            _stringProcessors = new[]
            {
                new Tuple<IStringProcessor,bool>(downloadProcessor, false)
            };
        }
        public void Process(RenderFieldArgs args) {
            Assert.ArgumentNotNull(args, "args");
            try
            {
                if (args.FieldTypeKey != "rich text")
                {
                    return;
                }
                foreach (var processor in _stringProcessors)
                {
                    args.Result.FirstPart = (args.Result.FirstPart == null) ? null : processor.Item1.Process(args.Result.FirstPart, processor.Item2);
                    args.Result.LastPart = (args.Result.LastPart == null) ? null : processor.Item1.Process(args.Result.LastPart, processor.Item2);
                }
            }
            catch (Exception e)
            {
                Log.Error("Error while running StringProcessor pipeline.", e, this);
            }
        }
    }
}