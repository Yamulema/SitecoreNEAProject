using Neambc.Neamb.Feature.SchemaMarkup.Models;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;

namespace Neambc.Neamb.Feature.SchemaMarkup.Factory
{
    public abstract class SchemaFactory
    {
        /// <summary>
        /// Generate Schema
        /// </summary>
        /// <param name="currentItem">Current Sitecore Item</param>
        /// <returns></returns>
        public abstract string GenerateSchema(Item currentItem, List<Item> subItems = null);

        /// <summary>
        /// Get image url
        /// </summary>
        /// <param name="item">sitecore item</param>
        /// <returns></returns>
        protected string GetImageUrl(Item item)
        {
            var mediaItem = new MediaItem(item);
            return GetImageUrl(mediaItem);
        }

        /// <summary>
        /// Get Image Url
        /// </summary>
        /// <param name="mediaItem">Media Item</param>
        /// <returns></returns>
        protected string GetImageUrl(MediaItem mediaItem)
        {
            var mediaOptions = new MediaUrlBuilderOptions
            {
                AlwaysIncludeServerUrl = true
            };

            string url = MediaManager.GetMediaUrl(mediaItem, mediaOptions);

            return url;
        }

        /// <summary>
        /// Get Sigle text value from a item
        /// </summary>
        /// <param name="currentItem">Sitecore Item</param>
        /// <param name="fieldId">Id of the field</param>
        /// <returns></returns>
        protected string GetSigletextValue(Item currentItem, ID fieldId)
        {
            var field = currentItem.Fields[fieldId];
            if (field != null)
            {
                return field.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get a date and time value
        /// </summary>
        /// <param name="currentItem">Sitecore Item</param>
        /// <param name="fieldId">Id of the field</param>
        /// <returns></returns>
        protected DateTime GetDateTimeValue(Item currentItem, ID fieldId)
        {
            var field = currentItem.Fields[fieldId];
            if (field != null)
            {
                var dateField = (DateField)field;
                return dateField.DateTime;
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// Get LinkField Url
        /// </summary>
        /// <param name="currentItem">Sitecore Item</param>
        /// <param name="fieldId">Id of the field</param>
        /// <returns></returns>
        protected string GetGeneralFieldUrl(Item currentItem, ID fieldId)
        {
            var field = currentItem.Fields[fieldId];
            if (field != null)
            {
                var linkField = (LinkField)field;

                if (linkField.LinkType == "external" && !string.IsNullOrEmpty(linkField.Url))
                {
                    return linkField.Url;
                }

                var itemOptions = new ItemUrlBuilderOptions
                {
                    AlwaysIncludeServerUrl = true
                };

                if (linkField.TargetItem == null)
                {
                    return string.Empty;
                }

                var url = LinkManager.GetItemUrl(linkField.TargetItem, itemOptions);

                return url;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get the url of an item with the server url
        /// </summary>
        /// <param name="item">Sitecore content Item</param>
        /// <returns></returns>
        protected string GetItemUrl(Item item)
        {
            var linkOptions = new ItemUrlBuilderOptions
            {
                AlwaysIncludeServerUrl = true
            };

            string url = LinkManager.GetItemUrl(item, linkOptions);

            return url;
        }

        /// <summary>
        /// Serialize an objet into Json
        /// </summary>
        /// <param name="objectContent">Object to serialize</param>
        /// <returns></returns>
        protected string CreateJson(object objectContent)
        {
            return JsonConvert.SerializeObject(objectContent, Formatting.Indented);
        }
    }
}