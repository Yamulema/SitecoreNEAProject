using Neambc.Neamb.Feature.SchemaMarkup.Repository.Interfaces;
using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.SchemaMarkup.Repository
{
    [Service(typeof(ISchemaRepository))]
    public class SchemaRepository : ISchemaRepository
    {
        /// <summary>
        /// Get current sitecore items
        /// </summary>
        /// <returns></returns>
        public Item GetCurrentItem()
        {
            return Sitecore.Context.Item;
        }

        /// <summary>
        /// Get question and answer renderings for FAQ
        /// </summary>
        /// <param name="currentItem">Current sitecore item</param>
        /// <returns></returns>
        public List<Item> GetQuestionAnswersItems(Item currentItem)
        {
            if (currentItem == null)
            {
                throw new ArgumentNullException("curremtItem");
            }

            var result = new List<Item>();
            var renderings = GetFinalLayoutRenderings(currentItem);

            if (renderings == null)
            {
                return result;
            }

            foreach (var render in renderings)
            {
                var rendering = render as RenderingDefinition;

                if (!IsValidSmallStacketAccordionRendering(rendering))
                {
                    continue;
                }

                var dataSourceId = new ID(rendering.Datasource);
                var accordionItem = Sitecore.Context.Database.GetItem(dataSourceId);

                if (!IsValidAccordionItem(accordionItem))
                {
                    continue;
                }

                result.Add(accordionItem);
            }
            return result;
        }

        /// <summary>
        /// Get the bread crumbs of an item
        /// </summary>
        /// <param name="currentItem">Current sitecore item</param>
        /// <returns></returns>
        public List<Item> GetBreadCrumbsOfItem(Item currentItem)
        {
            if (currentItem == null)
            {
                throw new ArgumentNullException("curremtItem");
            }

            var result = new List<Item>();
            var site = Sitecore.Context.Site;
            var homeItem = Sitecore.Context.Database.GetItem(site.StartPath);

            while (currentItem.ID != homeItem.ID)
            {
                result.Add(currentItem);

                if (currentItem.TemplateID == Templates.Product.Id)
                {
                    var eyebrowField = currentItem.Fields[Templates.Product.Fields.EyeBrow];
                    if (eyebrowField != null)
                    {
                        var eyeBrow = (LinkField)eyebrowField;
                        if (eyeBrow.TargetItem != null)
                        {
                            if(eyeBrow.TargetItem.Paths.IsContentItem)
                            {
                                currentItem = eyeBrow.TargetItem;
                            }
                            else
                            {
                                currentItem = currentItem.Parent;
                            }
                        }
                        else
                        {
                            currentItem = currentItem.Parent;
                        }
                    }
                    else
                    {
                        currentItem = currentItem.Parent;
                    }
                }
                else
                {
                    currentItem = currentItem.Parent;
                }

                if (currentItem.TemplateID == Templates.Folder.Id)
                {
                    break;
                }
            }

            result.Reverse();

            return result;
        }

        /// <summary>
        /// Get final renderings
        /// </summary>
        /// <param name="currentItem">Current Item</param>
        /// <returns></returns>
        private ArrayList GetFinalLayoutRenderings(Item currentItem)
        {
            var field = currentItem.Fields[Sitecore.FieldIDs.FinalLayoutField];
            var layoutXml = Sitecore.Data.Fields.LayoutField.GetFieldValue(field);
            var layout = LayoutDefinition.Parse(layoutXml);
            var deviceLayout = layout.Devices[0] as DeviceDefinition;

            if (deviceLayout == null)
            {
                return null;
            }

            return deviceLayout.Renderings;
        }

        /// <summary>
        /// Validate rendering
        /// </summary>
        /// <param name="rendering">rendering definition</param>
        /// <returns></returns>
        private bool IsValidSmallStacketAccordionRendering(RenderingDefinition rendering)
        {
            if (rendering == null)
            {
                return false;
            }

            if (!rendering.Placeholder.Contains("small-stacked-accordion-item"))
            {
                return false;
            }

            if (string.IsNullOrEmpty(rendering.Datasource))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate Accordion Item
        /// </summary>
        /// <param name="accordionItem">Accordion Item</param>
        /// <returns></returns>
        private bool IsValidAccordionItem(Item accordionItem)
        {
            if (accordionItem == null)
            {
                return false;
            }

            if (accordionItem.TemplateID != Templates.SmallAccordionItem.Id)
            {
                return false;
            }

            return true;
        }

    }
}