using Neambc.Neamb.Feature.GeneralContent.Models;
using Neambc.Neamb.Foundation.MBCData.Utilities;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System.Web;
using Sitecore.Data;
using Neambc.Neamb.Foundation.MBCData.Repositories;
using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Feature.GeneralContent.Repositories
{
    [Service(typeof(IChatRepository))]
    public class ChatRepository : IChatRepository
    {
        private readonly ITokenizationService _TokenizationService;

        public ChatRepository(ITokenizationService tokenizationService)
        {
            _TokenizationService = tokenizationService;
        }

        public ChatDTO GetChatContent()
        {
            var model = new ChatDTO();
            var rendering = RenderingContext.Current.Rendering;
            var item = rendering.Item;
            var siteSettings = Sitecore.Context.Database.GetItem(Items.SiteSettings);
            string productCode = "";


            if (item.TemplateID == Templates.Product.ID)
            {
                if (item != null && ItemUtility.HasField(item, Templates.ProductCTA.Fields.ProductCodeDroplink))
                {
                    string dropDownItemId = item[Templates.ProductCTA.Fields.ProductCodeDroplink];
                    var globalItem = Sitecore.Context.Database.GetItem(dropDownItemId);
                    if (globalItem != null)
                    {
                        productCode = globalItem[Templates.ProductCode.Fields.ProductCode];
                    }

                }
                GetChatSnippetProducts(siteSettings, item, productCode, ref model);
                model.DefaultChat = GetDefaultChatProducts(siteSettings, ref model);
            }
            else
            {
                GetChatSnippetComponent(item, ref model);
            }


            return model;
        }

        private void GetChatSnippetComponent(Item item, ref ChatDTO model)
        {
            var dropDownItemId = item[Templates.ChatSnippetTemplate.Fields.ChatSnippet];
            if (string.IsNullOrEmpty(dropDownItemId))
            {
                return;
            }

            var snippetItem = Sitecore.Context.Database.GetItem(new ID(dropDownItemId));
            if (snippetItem == null)
            {
                return;
            }

            var chatSnipped = snippetItem[Templates.ChatSnippet.Fields.Snippet];
            var snippedDetokenized = DeTokenizeChat(chatSnipped);
            model.Chat = new HtmlString(snippedDetokenized);
            model.DefaultChat = new HtmlString(snippedDetokenized);
            model.IsDefaultChat = false;
        }


        private void GetChatSnippetProducts(Item siteSettings, Item item, string productCode, ref ChatDTO model)
        {
            model.Chat = GetDefaultChatProducts(siteSettings, ref model);
            if (!string.IsNullOrEmpty(productCode))
            {
                var chatSnippet = item[Templates.ProductCTA.Fields.ChatSnippet];
                if (!string.IsNullOrEmpty(chatSnippet))
                {
                    var snippetItem = Sitecore.Context.Database.GetItem(new ID(chatSnippet));
                    if (snippetItem == null)
                    {
                        return;
                    }
                    var chatSnipped = snippetItem[Templates.ChatSnippet.Fields.Snippet];
                    var snippedDetokenized = DeTokenizeChat(chatSnipped);
                    model.Chat = new HtmlString(snippedDetokenized);
                    model.IsDefaultChat = false;
                }
            }
        }

        private HtmlString GetDefaultChatProducts(Item siteSettingsArg, ref ChatDTO model)
        {
            model.IsDefaultChat = true;
            var chatSnippet = siteSettingsArg.Fields[Templates.SiteSettings.Fields.Snippet1].Value;
            var snippetDetokenized = DeTokenizeChat(chatSnippet);
            return new HtmlString(snippetDetokenized);
        }

        private string DeTokenizeChat(string rawText)
        {
            return _TokenizationService.DeTokenize(rawText);
        }
    }
}