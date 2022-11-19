using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.GeneralContent.Extensions;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Feature.GeneralContent.Services
{
    [Service(typeof(IWizardEventHandler))]
    public class WizardEventService : IWizardEventHandler {
        private readonly IComplimentaryLifeWizardService _complimentaryLifeWizardService;
        public WizardEventService(IComplimentaryLifeWizardService complimentaryLifeWizardService) {
            _complimentaryLifeWizardService = complimentaryLifeWizardService;
        }
        public string GetOnClickNextEvent() {
            var currentItem = Sitecore.Context.Item;
            if (currentItem.HasRendering(Renderings.ComplimentaryLifeFA005)) {
                return _complimentaryLifeWizardService.GetOnClickNextEvent();
            }
            return string.Empty;
        }
        public string GetOnClickEndEvent() {
            var currentItem = Sitecore.Context.Item;
            if (currentItem.HasRendering(Renderings.ComplimentaryLifeFA005))
            {
                return _complimentaryLifeWizardService.GetOnClickEndEvent();
            }
            return string.Empty;
        }
    }
}