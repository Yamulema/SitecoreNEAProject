using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Neambc.Seiumb.Foundation.Analytics.GTM.Processors.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Neambc.Seiumb.Foundation.Analytics.GTM
{
    public class GTMServiceSeiumb: IGTMServiceSeiumb
    {
        private readonly IHtmlProcessor _htmlProcessor;
        private readonly string _dataLayerFunction;

        public GTMServiceSeiumb(IHtmlProcessor htmlProcessor)
        {
            _htmlProcessor = htmlProcessor;
            _dataLayerFunction = Configuration.DataLayerFunction;
        }

        public string GetOnClickEvent(object @object) {
            return @object == null ? string.Empty : $"onclick = \"{_dataLayerFunction}({SerializeObject(@object)},this);\"";
        }

        public void AddOnClickEvent(ref HtmlNode anchorNode, object @object, bool overrideEvent = false) {
            var onClickValue = $"{_dataLayerFunction}({SerializeObject(@object)},this);";
            var existingOnclick = anchorNode.Attributes["onclick"];

            //When onclick attribute is not present
            if (existingOnclick == null)
            {
                anchorNode.Attributes.Add("onclick", onClickValue);
                return;
            }

            //When onclick is empty
            if (string.IsNullOrEmpty(existingOnclick?.Value))
            {
                existingOnclick.Value += onClickValue;
                return;
            }

            var pattern = $@"((.+)|\s*)(s|dataLayerPush)([(])([^)]+)([)])(.+$)";
            var match = Regex.Match(existingOnclick.Value, pattern, RegexOptions.IgnoreCase);

            //When onclick has already a gtm function
            if (match.Success)
            {
                if (!overrideEvent) return;
                existingOnclick.Value = Regex.Replace(existingOnclick?.Value,
                    pattern,
                    x => string.Format("{0}{1}{2}{3}{4}{5}",
                        x.Groups[1].Value,
                        x.Groups[3].Value,
                        x.Groups[4].Value,
                        $"{SerializeObject(@object)},this",
                        x.Groups[6].Value,
                        x.Groups[7].Value));
                return;
            }

            //When existing onclick is closed
            if (existingOnclick.Value.Trim().EndsWith(";"))
            {
                existingOnclick.Value += onClickValue;
            }
            //When existing onclick is not closed
            else
            {
                existingOnclick.Value += $";{onClickValue}";
            }
        }

        public string GetGtmEvent(object @object) {
            if (@object == null)
            {
                return string.Empty;
            }
            return $"{_dataLayerFunction}({SerializeObject(@object)},this);";
        }

        public string SerializeObject(object @object)
        {
            if (@object == null) return null;

            @object = RemoveHtml(@object);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.QuoteChar = '\'';
                var ser = new JsonSerializer()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                ser.Serialize(writer, @object);
            }
            return sb.ToString();
        }

        private object RemoveHtml(object @object)
        {
            if (@object == null) return null;

            foreach (var prop in @object.GetType().GetProperties())
            {
                var oldValue = prop.GetValue(@object, null) as string;
                if (string.IsNullOrEmpty(oldValue)) continue;
                prop.SetValue(@object, _htmlProcessor.GetSuppresedText(_htmlProcessor.GetTextHtml(oldValue)));
            }
            return @object;
        }
    }
}