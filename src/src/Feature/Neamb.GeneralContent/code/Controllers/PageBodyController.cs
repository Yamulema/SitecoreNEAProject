using Neambc.Neamb.Feature.GeneralContent.Models;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Configuration.Extensions;
using Neambc.Neamb.Foundation.Configuration.Pipelines;
using Neambc.Neamb.Foundation.Configuration.Utility;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class PageBodyController : BaseController {
        private readonly IStringProcessor _stringProcessor;
        public PageBodyController(IEmbeddedProcessor embeddedProcessor) {
            _stringProcessor = embeddedProcessor;
        }
        public ActionResult FullWidthBodyCopy()
        {
            return View("/Views/Neamb.GeneralContent/Renderings/FullWidthBodyCopy.cshtml", CreateModel());
        }

        private PageBodyDTO CreateModel()
        {
            var pageBodyDTO = new PageBodyDTO()
            {
                BodyBackgroundColorClass = GetBackgroundColorClass(),
                StringProcessor = _stringProcessor
            };
            pageBodyDTO.Initialize(RenderingContext.Current.Rendering);
            return pageBodyDTO;
        }

        private string GetBackgroundColorClass()
        {
            var backgroundColor = PageContext.Current.Item.Fields[Templates.BasePageTemplates.Article.Fields.PageBodyBodyBackgroundColor];
            var backgroundColorClass = ConstantsNeamb.GrayBackgroundColorClass;
            if (backgroundColor.Value.ToUpper() == ConstantsNeamb.WhiteBackgroundColor.ToUpper())
            {
                backgroundColorClass = ConstantsNeamb.WhiteBackgroundColorClass;
            }
            else if  (backgroundColor.Value.ToUpper() == ConstantsNeamb.DarkBlueBackgroundColor.ToUpper())
            {
                backgroundColorClass = ConstantsNeamb.DarkBlueBackgroundColorClass;
            }
            return backgroundColorClass;
        }

        private PageBodyDTO CreateModelBodyCopy()
        {
            var hasBodyHeight =
                int.TryParse(
                    PageContext.Current.Item.Fields[Templates.BasePageTemplates.Article.Fields.BodyCopyBodyHeightLimit]?.Value, 
                    out var bodyHeight) 
                && bodyHeight > 0;

            var pageBodyDto = new PageBodyDTO()
            {
                BodyBackgroundColorClass = GetBackgroundColorClass(),
                BodyHeightLimit = PageContext.Current.Item.Fields[Templates.BasePageTemplates.Article.Fields.BodyCopyBodyHeightLimit].Value,
                HasBodyHeight = hasBodyHeight,
                StringProcessor = _stringProcessor
            };
            pageBodyDto.Initialize(RenderingContext.Current.Rendering);
            return pageBodyDto;
        }
        public ActionResult BodyCopy()
        {
            return View("/Views/Neamb.GeneralContent/Renderings/BodyCopy.cshtml", CreateModelBodyCopy());
        }
    }
}