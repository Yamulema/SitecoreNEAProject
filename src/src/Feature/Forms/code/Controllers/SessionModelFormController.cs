using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Forms.Models;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Neambc.Seiumb.Feature.Forms.Controllers
{
    public class SessionModalFormController : BaseController
	{
		public ActionResult SessionModal()
		{
			var model = new SessionFormModel();
			model.Initialize(RenderingContext.Current.Rendering);
			return View("/Views/Forms/SessionModal.cshtml", model);
		}
	}
}