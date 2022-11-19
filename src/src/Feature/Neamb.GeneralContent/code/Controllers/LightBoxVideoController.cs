using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class LightBoxVideoController : BaseController
    {
        public ActionResult LightBoxVideo() {
            return View("/Views/Neamb.GeneralContent/Modals/LightBoxVideo.cshtml", CreateModel());
        }

        #region Static Methods
        private static VideoDto CreateModel()
        {
            var result = new VideoDto();
            result.Initialize(RenderingContext.Current.Rendering);

            result.SourceUrl = VideoDto.GetVideoUrl(result.Item);
            result.Type = VideoDto.GetVideoType(result.Item);

            return result;
        }
        #endregion
    }
}