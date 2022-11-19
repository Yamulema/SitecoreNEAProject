using System;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Seiumb.Feature.GeneralContent.Models
{
    public class Chat : IRenderingModel
    {
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public HtmlString ChatHtml { get; private set; }

        public Func<string, string> DetokenizeChat;


        public void Initialize(Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;

            var chatItemSelected = Item[Templates.DefaultChat.Fields.SnippetItem];

            if (!string.IsNullOrEmpty(chatItemSelected)) 
            {
                ChatHtml = GetChatHtml(Item);
            } else 
            {
                var setting = Sitecore.Context.Database.GetItem(Items.SiteSettings);
                ChatHtml = GetChatHtml(setting);
            }
        }

        private HtmlString GetChatHtml(Item item) 
        {
            var chatId = item.Fields[Templates.DefaultChat.Fields.SnippetItem].Value;

            if (!string.IsNullOrEmpty(chatId)) 
            {
                var snippetItem = Sitecore.Context.Database.GetItem(new ID(chatId));
                var chatSnipped = snippetItem[Templates.ChatSnippetItem.Fields.Snippet];
                var detokenizedSnipped = DetokenizeChat(chatSnipped);
                return new HtmlString(detokenizedSnipped);
            }
            return new HtmlString("");
        }
    }
}