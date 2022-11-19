using Neambc.Neamb.Feature.SchemaMarkup.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neambc.Neamb.Feature.SchemaMarkup.Factory
{
    public class FAQSchemaFactory : SchemaFactory
    {
        /// <summary>
        /// Get FAQ Schema
        /// </summary>
        /// <param name="currentItem">Current Sitecore item</param>
        /// <param name="questionAnswers">FAQ Questions item</param>
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
        /// Create Json Content
        /// </summary>
        /// <param name="questionAnswers">FAQ Questions item</param>
        /// <returns></returns>
        private string GetJsonContent(List<Item> questionAnswers)
        {
            var model = GetFAQModel(questionAnswers);

            var jsonModel = CreateJson(model);

            return jsonModel;
        }

        /// <summary>
        /// Generate FAQ Model
        /// </summary>
        /// <param name="questionAnswers">FAQ Questions item</param>
        /// <returns></returns>
        private FAQModel GetFAQModel(List<Item> questionAnswers)
        {
            var faqModel = new FAQModel();
            faqModel.Context = "https://schema.org";
            faqModel.Type = "FAQPage";

            faqModel.MainEntity.AddRange(GetQuestionAnswres(questionAnswers));

            return faqModel;
        }

        /// <summary>
        /// Generate questions and answers
        /// </summary>
        /// <param name="questionAnswers">FAQ Questions item</param>
        /// <returns></returns>
        private List<QuestionFAQModel> GetQuestionAnswres(List<Item> questionAnswers)
        {
            var result = new List<QuestionFAQModel>();
            QuestionFAQModel questionModel = null;
            foreach (var item in questionAnswers)
            {
                questionModel = new QuestionFAQModel();
                var question = GetSigletextValue(item, Templates.SmallAccordionItem.Fields.Header);
                var answer = GetSigletextValue(item, Templates.SmallAccordionItem.Fields.Subhead);

                questionModel.Type = "Question";
                questionModel.Name = question;
                questionModel.AcceptedAnswer.Type = "Answer";
                questionModel.AcceptedAnswer.Text = answer;

                result.Add(questionModel);
            }

            return result;
        }
    }
}