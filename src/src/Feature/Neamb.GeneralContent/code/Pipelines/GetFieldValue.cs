using System;
using System.Collections.Generic;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Pipelines;
using Neambc.Neamb.Foundation.MBCData.Repositories;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Feature.GeneralContent.Pipelines {
	public class GetFieldValue : IRenderFieldProcessor
    {
        private readonly IEnumerable<Tuple<IStringProcessor,bool>> _stringProcessors;

        public GetFieldValue(
            ITokenizationService tokenizationService, 
            IFooterProcessor footerProcessor, 
            IContactUsProcessor contactUsProcessor, 
            INavigationProcessor navigationProcessor,
            IProductNavigationProcessor productNavigationProcessor,
            ISocialProcessor socialProcessor,
            IAccountProductsProcessor accountProductsProcessor,
			IContentCarouselProcessor contentCarouselProcessor
            )
		{
            _stringProcessors = new[]
            {
                new Tuple<IStringProcessor,bool>(tokenizationService, true),
                new Tuple<IStringProcessor,bool>(footerProcessor, false),
                new Tuple<IStringProcessor,bool>(contactUsProcessor, true),
                new Tuple<IStringProcessor,bool>(navigationProcessor, true),
                new Tuple<IStringProcessor,bool>(productNavigationProcessor, true),
                new Tuple<IStringProcessor,bool>(socialProcessor, true),
                new Tuple<IStringProcessor,bool>(accountProductsProcessor, true),
				new Tuple<IStringProcessor,bool>(contentCarouselProcessor, true)
            };
        }

		public void Process(RenderFieldArgs args) {
            if (!Context.Site.Name.Equals("neamb"))
                return;

            Assert.ArgumentNotNull(args, "args");
			try {
				if (args.FieldTypeKey != "rich text") {
					return;
				}
                // ----- DISPLAYING THE TOKEN TAG IN EE PAGEMODE -----
                if (Sitecore.Context.PageMode.IsExperienceEditor)
                {
                    return;
                }
                foreach (var processor in _stringProcessors) {
                    args.Result.FirstPart = (args.Result.FirstPart == null) ? null : processor.Item1.Process(args.Result.FirstPart, processor.Item2, args);
                    args.Result.LastPart = (args.Result.LastPart == null) ? null : processor.Item1.Process(args.Result.LastPart, processor.Item2, args);
                }
            } catch (Exception e) {
				Log.Error("Error while running StringProcessor pipeline.", e, this);
			}
		}
	}
}