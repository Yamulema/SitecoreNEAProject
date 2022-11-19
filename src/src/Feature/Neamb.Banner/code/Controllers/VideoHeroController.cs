using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Mvc.Presentation;
using Neambc.Neamb.Feature.GeneralContent.Models;

namespace Neambc.Neamb.Feature.Banner.Controllers
{
    public class VideoHeroController : BaseController
    {
        #region ActionResult Methods
        public ActionResult HeadlineHero()
        {
            return View("/Views/Neamb.Banner/VideoHero.cshtml", CreateModel());
        }
        #endregion

        #region Static Methods
        private static VideoDto CreateModel()
        {
            var result = new VideoDto();
            result.Initialize(RenderingContext.Current.Rendering);

            result.SourceUrl = VideoDto.GetVideoUrl(result.PageItem);
            result.Type = VideoDto.GetVideoType(result.PageItem);

            return result;
        }
        #endregion
    }
}