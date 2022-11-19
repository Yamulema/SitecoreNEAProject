using Neambc.Neamb.Feature.SchemaMarkup.Factory;
using Neambc.Neamb.Feature.SchemaMarkup.Models;
using Neambc.Neamb.Feature.SchemaMarkup.Repository.Interfaces;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Neambc.Neamb.Feature.SchemaMarkup.Controllers
{
    public class SchemaMarkupController : BaseController
    {
        private readonly ISchemaRepository _schemaRepository;

        public SchemaMarkupController(ISchemaRepository schemaRepository)
        {
            _schemaRepository = schemaRepository;
        }

        /// <summary>
        /// Execute the schema generation
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateSchemaMarkup()
        {
            var model = new SchemaMarkupModel();
            try
            {
                var currentItem = _schemaRepository.GetCurrentItem();

                if (currentItem.TemplateID == Templates.Articles.Id)
                {
                    model.ScriptContent.Add(GetArticleSchema(currentItem));
                }

                if (ValidateIsFAQ(currentItem))
                {
                    var questionAnswers = _schemaRepository.GetQuestionAnswersItems(currentItem);

                    model.ScriptContent.Add(GetFAQSchemaMarkupModel(currentItem, questionAnswers));
                }

                if (ValidateCurrentItemForBreadcrumbs(currentItem))
                {
                    var breadcrumbs = _schemaRepository.GetBreadCrumbsOfItem(currentItem);
                    model.ScriptContent.Add(GetBreadCrumbsSchema(currentItem, breadcrumbs));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, this);
            }
            
            return View(Views.SchemaMarkupView, model);
        }

        /// <summary>
        /// Validate if the page or template has FAQ
        /// </summary>
        /// <param name="currentItem">Current Sitecore Item</param>
        /// <returns></returns>
        private bool ValidateIsFAQ(Item currentItem)
        {
            return currentItem.ID == Templates.LandingPage.FAQPageId
                || currentItem.TemplateID == Templates.Product.Id
                || currentItem.TemplateID == Templates.MarketPlace.Id;
        }

        /// <summary>
        /// Validate if the current item needs to display bread crumbs schema markup
        /// </summary>
        /// <param name="currentItem">Current Sitecore Item</param>
        /// <returns></returns>
        private bool ValidateCurrentItemForBreadcrumbs(Item currentItem)
        {
            return currentItem.TemplateID == Templates.Articles.Id
                || currentItem.TemplateID == Templates.GoalTopic.Id
                || currentItem.TemplateID == Templates.Product.Id
                || currentItem.TemplateID == Templates.ProductCategory.Id;
        }

        /// <summary>
        /// Generate the article schema
        /// </summary>
        /// <param name="currentItem">Current Sitecore item</param>
        /// <returns></returns>
        private string GetArticleSchema(Item currentItem)
        {
            var articleFactory = new ArticleSchemaFactory();

            return articleFactory.GenerateSchema(currentItem);
        }

        /// <summary>
        /// Generate FAQ schema
        /// </summary>
        /// <param name="currentItem">Current Sitecore item</param>
        /// <param name="questionAnswers">List of question and answers</param>
        /// <returns></returns>
        private string GetFAQSchemaMarkupModel(Item currentItem, List<Item> questionAnswers)
        {
            var articleFactory = new FAQSchemaFactory();

            return articleFactory.GenerateSchema(currentItem, questionAnswers);
        }

        /// <summary>
        /// Generate Breadcrumb schema
        /// </summary>
        /// <param name="currentItem">Current Sitecore item</param>
        /// <param name="breadcrumbs">List of bread crumbs</param>
        /// <returns></returns>
        private string GetBreadCrumbsSchema(Item currentItem, List<Item> breadcrumbs)
        {
            var breadcrumbFactory = new BreadCrumbSchemaFactory();
            return breadcrumbFactory.GenerateSchema(currentItem, breadcrumbs);
        }

        private struct Views
        {
            public const string SchemaMarkupView = "~/Views/Neamb.SchemaMarkup/SchemaMarkup.cshtml";
        }


    }

    
}