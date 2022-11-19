using System.Collections.Generic;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Neambc.Seiumb.Feature.Carousel.Models;
using Neambc.Seiumb.Foundation.Analytics.GTM;
using Neambc.Seiumb.Foundation.Analytics.GTM.Models;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.Carousel.Controllers {
	public class CarouselController : BaseController {
        private readonly IGTMServiceSeiumb _gtmService;

        public CarouselController(IGTMServiceSeiumb gtmService)
        {
            _gtmService = gtmService;
        }

        public ActionResult GetCarouselSlides() {
			var carouselModel = new CarouselModel();
			carouselModel.Initialize(RenderingContext.Current.Rendering);
            carouselModel.Slides = AddOnClickGTMContent(carouselModel.Slides);
            return View("/Views/Carousel/Renderings/Carousel.cshtml", carouselModel);
		}

        private List<SlideModel> AddOnClickGTMContent(List<SlideModel> carouselItems) {
            var result = new List<SlideModel>();

            foreach (var item in carouselItems)
            {
                var onClickContent = _gtmService.GetOnClickEvent(new ModuleSeiumb
                {
                    Event = "homepage carousel",
                    ModuleTitle = item.Slide["Headline"],
                    CtaText = item.Slide["Subheadline"]
                });
                item.OnClickEventContent = onClickContent;
                result.Add(item);
            }
            return result;
        }
	}
}