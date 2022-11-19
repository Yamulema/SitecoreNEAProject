using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Forms.Models;
using Neambc.Seiumb.Feature.Forms.Repositories;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.Forms.Controllers {
	public class UnsubscribeController : BaseController {
		private readonly IFormsRepository _formsRepository;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="formsRepository"></param>
		public UnsubscribeController(IFormsRepository formsRepository) {
			_formsRepository = formsRepository;
		}

		public ActionResult ExecuteUnsubscribe(string listid, string mdsid, string cellcode) {
			var unsubscribeModel = new UnsubscribeModel();
			unsubscribeModel.Initialize(RenderingContext.Current.Rendering);
			if (int.TryParse(listid, out var listIdAsInt)) {
				_formsRepository.UnsubscribeList(listIdAsInt, mdsid, unsubscribeModel);
			}
			return View("/Views/Forms/Unsubscribe.cshtml", unsubscribeModel);
		}
	}
}