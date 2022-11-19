using Neambc.Neamb.Feature.SchemaMarkup.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neambc.Neamb.Feature.SchemaMarkup.Factory
{
    public class BreadCrumbSchemaFactory : SchemaFactory
    {
        /// <summary>
        /// Generate the schema markup for bread crumbs
        /// </summary>
        /// <param name="currentItem">Current sitecore item</param>
        /// <param name="subItems">List of sitecore content items</param>
        /// <returns></returns>
        public override string GenerateSchema(Item currentItem, List<Item> subItems = null)
        {
            if (currentItem == null)
            {
                throw new ArgumentNullException("currentItem");
            }

            if (subItems == null || subItems?.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            builder.AppendLine("<script type=\"application/ld+json\">");
            builder.AppendLine(GetJsonContent(subItems));
            builder.AppendLine("</script>");

            return builder.ToString(); 
        }

        /// <summary>
        /// Creates the json content
        /// </summary>
        /// <param name="breadCrumbs">List of sitecore content items</param>
        /// <returns></returns>
        private string GetJsonContent(List<Item> breadCrumbs)
        {
            var model = GetBreadCrumbModel(breadCrumbs);

            var jsonModel = CreateJson(model);

            return jsonModel;
        }

        /// <summary>
        /// Creates the bread crumbs model
        /// </summary>
        /// <param name="breadCrumbs">List of sitecore content items</param>
        /// <returns></returns>
        private BreadCrumbModel GetBreadCrumbModel(List<Item> breadCrumbs)
        {
            var model = new BreadCrumbModel();

            model.Context = "https://schema.org";
            model.Type = "BreadcrumbList";

            for (int i = 0; i < breadCrumbs.Count; i++)
            {
                var itemModel = new BreadCrumbItemModel();
                var position = i + 1;

                itemModel.Type = "ListItem";
                itemModel.Position = position;
                itemModel.Name = GetSigletextValue(breadCrumbs[i], Templates.CommonFields.MetaTitle);
                itemModel.Item = GetItemUrl(breadCrumbs[i]);

                model.ItemElementList.Add(itemModel);
            }

            return model;
        }
    }
}