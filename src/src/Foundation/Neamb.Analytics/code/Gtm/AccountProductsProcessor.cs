using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using Neambc.Neamb.Foundation.Analytics.Extensions;
using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public class AccountProductsProcessor : IAccountProductsProcessor
    {
        private readonly IGtmService _gtmService;
        public AccountProductsProcessor(IGtmService gtmService) {
            _gtmService = gtmService;
        }
        public string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null) {
            if (string.IsNullOrEmpty(input)) return input;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            var nodes = htmlDoc.DocumentNode.SelectNodes($"//div[contains(@class,'{Configuration.AccountProductsClass}')]");
            if (nodes == null) return input;
            foreach (var node in nodes) {
                var accountAction = node.SelectSingleNode(".//h3")?.InnerText;
                foreach (var anchorNode in node.SelectNodes(".//a")) {
                    var anchor = anchorNode;
                    AddOnClickEvent(ref anchor, accountAction, overrideEvents);
                }
            }
            return htmlDoc.DocumentNode.OuterHtml;
        }
        private void AddOnClickEvent(ref HtmlNode anchor, string accountAction, bool overrideEvent = false) {

            if (anchor == null) {
                return;
            }
            if (anchor.HasClass(new List<string>() {"btn"}))
            {
                _gtmService.AddOnClickEvent(ref anchor, new Account()
                {
                    Event = "account",
                    AccountSection = "manage products and services",
                    AccountAction = accountAction,
                    CtaText = anchor.InnerText
                }, overrideEvent);
            } else {
                _gtmService.AddOnClickEvent(ref anchor, new Account()
                {
                    Event = "account",
                    AccountSection = "manage products and services",
                    AccountAction = accountAction,
                    CtaText = "product link"
                }, overrideEvent);
            }
        }
    }
}