using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class LightBoxController : BaseController
    {
        private readonly ISessionManager _sessionManager;
        public LightBoxController(ISessionManager sessionManager) {
            _sessionManager = sessionManager;
        }
        public ActionResult ShowModal() {
            var returnUrl = string.IsNullOrEmpty(_sessionManager.RetrieveFromSession<string>(Configuration.ReturnUrlArg))
                ? string.Empty
                :_sessionManager.RetrieveFromSession<string>(Configuration.ReturnUrlArg);
            var model = new LightBox() {
                Item = RenderingContext.Current.Rendering.Item ?? PageContext.Current.Item,
                RedirectUrl = returnUrl
            };
            return View("/Views/Neamb.GeneralContent/Modals/LightBox.cshtml", model);
        }
    }
}