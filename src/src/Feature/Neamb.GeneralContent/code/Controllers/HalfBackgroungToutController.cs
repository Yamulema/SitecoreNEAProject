using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using HtmlAgilityPack;
using Neambc.Neamb.Feature.GeneralContent.Extensions;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.GeneralContent.Controllers
{
    public class HalfBackgroundToutController : BaseController {

        public ActionResult HalfBackgroundTout() {
            var model = GetModel();
            return View("/Views/Neamb.GeneralContent/HalfBackgroundTout/HalfBackgroundTout.cshtml", model);
        }

        private HalfBackgroundTout GetModel() {
            var datasource = RenderingContext.Current.Rendering.Item ?? PageContext.Current.Item;

            var linkField = ((LinkField) datasource.Fields[Templates.HalfBackgroundTout.Fields.Link])?.GetFriendlyUrl();
            if (string.IsNullOrEmpty(linkField)) linkField = "/";

            var colorHex = datasource.Fields[Templates.HalfBackgroundTout.Fields.BackgroundColor];
            var colorD = ColorTranslator.FromHtml("#" + colorHex);
            var colorL = ColorExtension.ChangeColorBrightness(colorD, 0.4F);

            var attributeList = datasource.Fields[Templates.HalfBackgroundTout.Fields.LinkHtmlProperties].Value;
            var attrDictionary = new Dictionary<string, string>();
            var attributeListRaw = "";

            if (!string.IsNullOrEmpty(attributeList)) {
                var attributeListClean = attributeList.Replace("\"", "");
                attributeListRaw = attributeList.Replace(",", " ");

                attrDictionary = attributeListClean.Split(',')
                    .Select(x => x.Split('='))
                    .ToDictionary(x => x[0], x => x[1]);
            }

            var model = new HalfBackgroundTout {
                Link = linkField,
                LinkHtmlProperties = attributeListRaw,
                RGBDark = colorD.R + ", " + colorD.G + ", " + colorD.B,
                RGBLight = colorL.R + ", " + colorL.G + ", " + colorL.B,
                Item = datasource
            };

            var bodyText = datasource.Fields[Templates.HalfBackgroundTout.Fields.Body].Value;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(bodyText);

            var anchorNodes = htmlDoc.DocumentNode.SelectNodes("//a");
            if (anchorNodes != null)
                foreach (var anchorNode in anchorNodes) {
                    var href = anchorNode.Attributes["href"].Value;
                    if (href.Contains("[ToutHref]")) {
                        anchorNode.Attributes["href"].Value = linkField;
                        foreach (var attribute in attrDictionary)
                            anchorNode.Attributes.Append(attribute.Key, attribute.Value);
                    }
                }
            model.Body = htmlDoc.DocumentNode.OuterHtml;
            return model;
        }
    }
}