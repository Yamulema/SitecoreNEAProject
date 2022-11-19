using Neambc.Neamb.Feature.SchemaMarkup.Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

namespace Neambc.Neamb.Feature.SchemaMarkup.Factory
{
    public class ArticleSchemaFactory : SchemaFactory
    {
        /// <summary>
        /// Generate Schema
        /// </summary>
        /// <param name="currentItem">Current Sitecore Item</param>
        /// <param name="subItems">List of sitecore content items</param>
        /// <returns></returns>
        public override string GenerateSchema(Item currentItem, List<Item> subItems = null)
        {
            if (currentItem == null)
            {
                throw new ArgumentNullException("currentItem");
            }

            if (currentItem.TemplateID != Templates.Articles.Id)
            {
                throw new InvalidOperationException("Not valid template");
            }

            var builder = new StringBuilder();

            builder.AppendLine("<script type=\"application/ld+json\">");
            builder.AppendLine(GetJsonContent(currentItem));
            builder.AppendLine("</script>");

            return builder.ToString(); 
        }

        /// <summary>
        /// Generate Json string object
        /// </summary>
        /// <param name="currentItem">Current Sitecore Item</param>
        /// <returns></returns>
        private string GetJsonContent(Item currentItem)
        {
            var model = new ArticleModel()
            {
                Context = "https://schema.org",
                Type = "NewsArticle",
                Headline = GetSigletextValue(currentItem, Templates.Articles.Fields.PageTitle),
                Author = AuthorContent(currentItem)
            };

            var publishedDate = GetDateTimeValue(currentItem, Templates.Articles.Fields.PublishDate);
            var lastUpdatedDate = GetDateTimeValue(currentItem, Templates.Articles.Fields.LastUpdatedDate);

            model.DatePublished = publishedDate != DateTime.MinValue ? publishedDate : (lastUpdatedDate != DateTime.MinValue ? lastUpdatedDate : currentItem.Statistics.Created);
            model.DateModified = lastUpdatedDate != DateTime.MinValue ? lastUpdatedDate : currentItem.Statistics.Updated;

            model.MainEntityOfPage.Type = "WebPage";
            model.MainEntityOfPage.Id = "https://google.com/article";

            var images = GetArticleImage(currentItem);
            if(images != null && images.Count > 0)
            {
                model.Image = new List<string>();
                model.Image.AddRange(images);
            }
            

            var jsonResult = CreateJson(model);

            return jsonResult;
        }

        /// <summary>
        /// Get Article Images
        /// </summary>
        /// <param name="currentItem">Current Sitecore Item</param>
        /// <returns></returns>
        public List<string> GetArticleImage(Item currentItem)
        {
            var images = new List<string>();

            var heroImageField = (ImageField)currentItem.Fields[Templates.Articles.Fields.HeroImage];

            if(heroImageField == null || heroImageField?.MediaItem == null)
            {
                return null;
            }
            
            var urlImage = GetImageUrl(heroImageField.MediaItem);

            images.Add(urlImage);

            return images;
        }

        /// <summary>
        /// Get Author Content
        /// </summary>
        /// <param name="currentItem">Current Sitecore Item</param>
        /// <returns></returns>
        private AuthorModel AuthorContent(Item currentItem)
        {
            var model = new AuthorModel();

            model.Type = "Organization";
            var authorName = GetSigletextValue(currentItem, Templates.Articles.Fields.Author);
            model.Name = !string.IsNullOrEmpty(authorName)? authorName : "NEA Member Benefits";

            var url = GetGeneralFieldUrl(currentItem, Templates.Articles.Fields.AuthorUrl);
            model.Url = !string.IsNullOrEmpty(url)? url: "sameas";

            return model;
        }
    }
}