using Neambc.Seiumb.Foundation.Analytics.GTM.Processors.Interfaces;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.RenderField;
using System;
using System.Collections.Generic;
using Neambc.Neamb.Foundation.Configuration.Pipelines;
using Neambc.Neamb.Foundation.MBCData.Repositories;
using Sitecore;

namespace Neambc.Seiumb.Foundation.Analytics.Pipelines
{
    public class GetFieldValue
    {
        private readonly IEnumerable<Tuple<IStringProcessor, bool>> _stringProcessors;

        public GetFieldValue(
            IProductDetailsProcessor productDetailsProcessor,
            IMiscellaneousPagesProcessor miscellaneousPagesProcessor,
            ILandingPageProcessor landingPageProcessor,
            IErrorPageProcessor errorPageProcessor,
            IRailCardProcessor railCardProcessor,
            IHomePageProcessor homePageProcessor,
            ITokenizationServiceSeiumb tokenizationServiceSeiumb
            )
        {
            _stringProcessors = new[]
            {
                new Tuple<IStringProcessor,bool>(productDetailsProcessor, true),
                new Tuple<IStringProcessor,bool>(miscellaneousPagesProcessor, true),
                new Tuple<IStringProcessor,bool>(landingPageProcessor, true),
                new Tuple<IStringProcessor,bool>(errorPageProcessor, true),
                new Tuple<IStringProcessor,bool>(railCardProcessor, true),
                new Tuple<IStringProcessor,bool>(homePageProcessor, true),
                new Tuple<IStringProcessor,bool>(tokenizationServiceSeiumb, true)
            };
        }
        public void Process(RenderFieldArgs args) {
            if (!Context.Site.Name.Equals("seiumb")) 
                return;

            Assert.ArgumentNotNull(args, "args");
            try
            {
                if (args.FieldTypeKey != "rich text")
                {
                    return;
                }
                // ----- DISPLAYING THE TOKEN TAG IN EE PAGEMODE -----
                if (Sitecore.Context.PageMode.IsExperienceEditor)
                {
                    return;
                }
                foreach (var processor in _stringProcessors)
                {
                    args.Result.FirstPart = (args.Result.FirstPart == null) ? null : processor.Item1.Process(args.Result.FirstPart, processor.Item2, args);
                    args.Result.LastPart = (args.Result.LastPart == null) ? null : processor.Item1.Process(args.Result.LastPart, processor.Item2, args);
                }
            }
            catch (Exception e)
            {
                Log.Error("Error while running StringProcessor pipeline.", e, this);
            }
        }
    }
}