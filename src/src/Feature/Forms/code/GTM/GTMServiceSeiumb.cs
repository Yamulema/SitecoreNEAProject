using System;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using Neambc.Seiumb.Feature.Forms.GTM.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Neambc.Seiumb.Feature.Forms.GTM
{
    public class GTMServiceSeiumb
    {
        private readonly string _dataLayerFunction = "dataLayerPush";

        public GTMServiceSeiumb()
        {
        }

        public string GetOnClickEvent(object @object) {
            return @object == null ? string.Empty : $"onclick = \"{_dataLayerFunction}({SerializeObject(@object)},this);\"";
        }

        public void AddOnClickEvent(ref HtmlNode anchorNode, object @object, bool overrideEvent = false) {
            throw new NotImplementedException();
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
                prop.SetValue(@object, HtmlProcessor.GetSuppresedText(HtmlProcessor.GetTextHtml(oldValue)));
            }
            return @object;
        }
    }
}