using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;

namespace Neambc.Neamb.Feature.GeneralContent.Repositories
{
    [Service(typeof(ITestYourKnowledgeRepository))]
    public class TestYourKnowledgeRepository : ITestYourKnowledgeRepository
    {
        public TestYourKnowledge GetKnownledgeContent()
        {
            var model = new TestYourKnowledge();

            var item = RenderingContext.Current.Rendering.Item;

            model.Initialize(RenderingContext.Current.Rendering);

            model.HeadLine = GetSigletextValue(item, Templates.TestYourKnowledge.Fields.HeadLine);
            model.Question = GetSigletextValue(item, Templates.TestYourKnowledge.Fields.Question);
            model.SubmitButton = GetSigletextValue(item, Templates.TestYourKnowledge.Fields.SubmitButton);
            model.ConfirmationLink = GetGeneralFieldUrl(item, Templates.TestYourKnowledge.Fields.ConfirmationLink);

            model.Option1 = GetSigletextValue(item, Templates.TestYourKnowledge.Fields.Option1);
            model.Option2 = GetSigletextValue(item, Templates.TestYourKnowledge.Fields.Option2);
            model.Option3 = GetSigletextValue(item, Templates.TestYourKnowledge.Fields.Option3);
            model.Option4 = GetSigletextValue(item, Templates.TestYourKnowledge.Fields.Option4);
            model.Option5 = GetSigletextValue(item, Templates.TestYourKnowledge.Fields.Option5);
            model.Option6 = GetSigletextValue(item, Templates.TestYourKnowledge.Fields.Option6);
            model.Option7 = GetSigletextValue(item, Templates.TestYourKnowledge.Fields.Option7);
            model.Option8 = GetSigletextValue(item, Templates.TestYourKnowledge.Fields.Option8);
            model.Option9 = GetSigletextValue(item, Templates.TestYourKnowledge.Fields.Option9);

            return model;
        }



        /// <summary>
        /// Get Sigle text value from a item
        /// </summary>
        /// <param name="currentItem">Sitecore Item</param>
        /// <param name="fieldId">Id of the field</param>
        /// <returns></returns>
        private string GetSigletextValue(Item currentItem, ID fieldId)
        {
            var field = currentItem.Fields[fieldId];
            if (field != null)
            {
                return field.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get LinkField Url
        /// </summary>
        /// <param name="currentItem">Sitecore Item</param>
        /// <param name="fieldId">Id of the field</param>
        /// <returns></returns>
        private LinkContentField GetGeneralFieldUrl(Item currentItem, ID fieldId)
        {
            var field = currentItem.Fields[fieldId];
            var model = new LinkContentField();
            if (field != null)
            {
                var linkField = (LinkField)field;

                if ((linkField.LinkType == "external" || linkField.LinkType == "javascript") && !string.IsNullOrEmpty(linkField.Url))
                {
                    model.LinkType = linkField.LinkType;
                    model.Value = linkField.Url;
                    return model;
                }

                if (linkField.TargetItem == null)
                {
                    return model;
                }

                var itemOptions = new ItemUrlBuilderOptions
                {
                    AlwaysIncludeServerUrl = false
                };

                var url = LinkManager.GetItemUrl(linkField.TargetItem, itemOptions);
                model.LinkType = "internal";
                model.Value = url;

            }
            return model;
        }
    }
}