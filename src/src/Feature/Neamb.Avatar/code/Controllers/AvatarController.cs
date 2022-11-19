using System.Web.Mvc;
using Neambc.Neamb.Feature.Avatar.Interfaces;
using Neambc.Neamb.Feature.Avatar.Models;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Avatar.Controllers {
	public class AvatarController : BaseController {
		private readonly IAvatarManager _avatarManager;
		
		public AvatarController(IAvatarManager avatarManager) {
			_avatarManager = avatarManager;
		}

		public ActionResult Avatar() {
			var model= _avatarManager.GetAvatarModel(Request.QueryString[ConstantsNeamb.UploadResult], RenderingContext.Current.Rendering);
			return View("/Views/Neamb.Avatar/Avatar.cshtml", model);
		}

		[HttpPost]
		
		public ActionResult UploadAvatar(string data) {
			var result= _avatarManager.SaveAvatar(data);
			switch (result) {
				case AvatarResultOperation.ErrorSize: {
					return Json(new { results = "errorsize" }, JsonRequestBehavior.AllowGet);
				}
				case AvatarResultOperation.Ok: {
					return Json(new { results = "OK" }, JsonRequestBehavior.AllowGet);
				}
				case AvatarResultOperation.GeneralError:
				default:
				{
					return Json(new {
							results = "generalerror"
						},
						JsonRequestBehavior.AllowGet);
				}
			}
		}
	}
}