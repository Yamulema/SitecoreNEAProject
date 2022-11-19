using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class AttributionController : BaseController
    {
        public ActionResult Attribution()
        {
            var model = new AttributionDto();
            model.Initialize(RenderingContext.Current.Rendering);

            //Fills the model.
            FillModel(ref model);

            return View("/Views/Neamb.GeneralContent/Attribution/Attribution.cshtml", model);
        }

        private static void FillModel(ref AttributionDto model)
        {
            var publishedDate = GetPublishedDate(model.PageItem);
            model.Author = GetAuthor(model.PageItem) ?? string.Empty;
            model.PublishedDate = publishedDate != null && publishedDate != DateTime.MinValue 
                                    ? string.Format("{0:MMM dd, yyyy}", publishedDate) 
                                    : string.Empty;
            model.CssStyle = GetCssStyle(model.PageItem);
            model.SocialShare = new SocialShareModel(model.PageItem);
        }

        private static string GetCssStyle(Item pageItem)
        {
            var result = string.Empty;
            var color   = pageItem.Fields[Templates.PageBody.Fields.PageBodyBodyBackgroundColor]?.Value;
            switch (color)
            {
                case "Gray":
                    result = "bg-gray";
                    break;
                case "Dark Blue":
                    result = "bg-dark-blue";
                    break;
                default:
                    result = string.Empty;
                    break;
            }
            return result;
        }

        private static DateTime? GetPublishedDate(Item pageItem)
        {
            DateTime? result = null;
            if (pageItem.Template.BaseTemplates.Any(x => x.ID == Templates.Attribution.ID))
            {
                result = ((DateField)pageItem.Fields[Templates.Attribution.Fields.LastUpdated])?.DateTime != DateTime.MinValue
                    ? ((DateField)pageItem.Fields[Templates.Attribution.Fields.LastUpdated]).DateTime :
                    ((DateField)pageItem.Fields[Templates.Attribution.Fields.PublishDate])?.DateTime;
            }
            return result;
        }

        private static string GetAuthor(Item pageItem)
        {
            var result = string.Empty;
            if (pageItem.Template.BaseTemplates.Any(x => x.ID == Templates.Attribution.ID))
            {
                result = pageItem.Fields[Templates.Attribution.Fields.Authors]?.Value;
            }
            return result;
        }
    }
}