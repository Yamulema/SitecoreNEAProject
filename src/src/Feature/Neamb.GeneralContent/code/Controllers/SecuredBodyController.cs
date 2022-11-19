using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Pipelines;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Neamb.Foundation.Membership.Managers;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers {
	public class SecuredBodyController : BaseController {
		private readonly ISessionAuthenticationManager _sessionManager;
        private readonly IStringProcessor _stringProcessor;
        public SecuredBodyController(ISessionAuthenticationManager sessionManager, IEmbeddedProcessor embeddedProcessor) {
			_sessionManager = sessionManager;
            _stringProcessor = embeddedProcessor;
        }
		public ActionResult SecuredBody() {
			return View("/Views/Neamb.GeneralContent/Renderings/SecuredBody.cshtml", CreateModel());
		}
		public ActionResult FullWidthSecuredBody() {
			return View("/Views/Neamb.GeneralContent/Renderings/FullWidthSecuredBody.cshtml", CreateModel());
		}
		private SecuredBodyDTO CreateModel() {
			var securedBodyDTO = new SecuredBodyDTO(_sessionManager);
            securedBodyDTO.StringProcessor = _stringProcessor;
			securedBodyDTO.Initialize(RenderingContext.Current.Rendering);
			return securedBodyDTO;
		}
	}
}