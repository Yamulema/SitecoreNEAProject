using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.GeneralContent.Models;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.GeneralContent.Controllers
{
    public class FourStepsCarouselController : BaseController
    {
        public ActionResult FourStepsCarousel()
        {
            var contextItem = RenderingContext.Current.Rendering.Item;
            var model = new FourStepsCarousel();
            model.Initialize(RenderingContext.Current.Rendering);
            model.HasStep1 = !string.IsNullOrEmpty(contextItem[Templates.FourStepsCarousel.Fields.Step1]);
            model.HasStep2 = !string.IsNullOrEmpty(contextItem[Templates.FourStepsCarousel.Fields.Step2]);
            model.HasStep3 = !string.IsNullOrEmpty(contextItem[Templates.FourStepsCarousel.Fields.Step3]);
            model.HasStep4 = !string.IsNullOrEmpty(contextItem[Templates.FourStepsCarousel.Fields.Step4]);

            return View("/Views/Seiumb.GeneralContent/Renderings/4StepsCarousel.cshtml", model);
        }
    }
}