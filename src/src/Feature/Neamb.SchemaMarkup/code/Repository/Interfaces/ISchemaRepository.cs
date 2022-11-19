using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.SchemaMarkup.Repository.Interfaces
{
    public interface ISchemaRepository
    {
        /// <summary>
        /// Get current sitecore items
        /// </summary>
        /// <returns></returns>
        Item GetCurrentItem();

        /// <summary>
        /// Get question and answer renderings for FAQ
        /// </summary>
        /// <param name="currentItem">Current sitecore item</param>
        /// <returns></returns>
        List<Item> GetQuestionAnswersItems(Item currentItem);

        /// <summary>
        /// Get the bread crumbs of an item
        /// </summary>
        /// <param name="currentItem">Current sitecore item</param>
        /// <returns></returns>
        List<Item> GetBreadCrumbsOfItem(Item currentItem);
    }
}