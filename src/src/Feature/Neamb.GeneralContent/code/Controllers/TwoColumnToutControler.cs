using Neambc.Neamb.Feature.GeneralContent.Models;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class TwoColumnToutController : BaseController {

        public ActionResult TwoColumnTout()
        {
            return View("/Views/Neamb.GeneralContent/Renderings/TwoColumnTout.cshtml", CreateModel());
        }

        private TwoColumnTout CreateModel()
        {
            var renderingItem = RenderingContext.Current.Rendering.Item;
            var model = new TwoColumnTout();

            model.Initialize(RenderingContext.Current.Rendering);
            model.BackgroundColor = GetBackgroundColor(renderingItem[Templates.TwoColumnTout.Fields.BackgroundColor]);
            model.AlignmentRight = ((CheckboxField) renderingItem.Fields[Templates.TwoColumnTout.Fields.ImageAligmentRight]).Checked;
            model.ImageUrl = renderingItem.ImageUrl(Templates.TwoColumnTout.Fields.Image);
            model.ImageAlt = GetImageAltField(renderingItem);
            return model;
        }

        private string GetBackgroundColor(string color)
        {
            var result = string.IsNullOrEmpty(color) ? "cccccc" : color;
            return result.Contains("#") ? result : $"#{result}";
        }

        private string GetImageAltField(Item item)
        {
            var imgField = (ImageField)item.Fields[Templates.TwoColumnTout.Fields.Image];
            return imgField.Alt;
        }
    }
}