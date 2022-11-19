using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Membership.Managers;
using Sitecore.Mvc.Presentation;

// Neambc.Neamb.Feature.GeneralContent.Controllers.WizardController
namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class WizardController : BaseController {
        private readonly IWizardService _wizardService;
		private readonly IRegistrationManager _registrationManager;

		public WizardController(IWizardService wizardService, IRegistrationManager registrationManager) {
            _wizardService = wizardService;
			_registrationManager = registrationManager;
		}
        public ActionResult Wizard() {
            var dataSource = RenderingContext.Current?.Rendering?.Item ?? PageContext.Current?.Item;
            var model = _wizardService.GetWizard(dataSource, Request);
			if (PageContext.Current != null) {
				model.GtmAction = _registrationManager.ExecuteGtmActionRegistrationRedirection(PageContext.Current.Item.ID.ToString());
			}
			return View("/Views/Neamb.GeneralContent/Wizard/Wizard.cshtml", model);
        }
        public ActionResult Step()
        {
            var dataSource = RenderingContext.Current?.Rendering?.Item ?? PageContext.Current?.Item;
            var model = _wizardService.GetStep(dataSource);
			if (PageContext.Current != null) {
				model.GtmAction = _registrationManager.ExecuteGtmActionRegistrationRedirection(PageContext.Current.Item.ID.ToString());
			}
			return View("/Views/Neamb.GeneralContent/Wizard/Step.cshtml", model);
        }
    }
}