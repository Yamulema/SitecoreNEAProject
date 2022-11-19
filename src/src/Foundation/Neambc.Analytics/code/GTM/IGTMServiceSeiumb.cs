using HtmlAgilityPack;

namespace Neambc.Seiumb.Foundation.Analytics.GTM
{
    public interface IGTMServiceSeiumb
    {
        string SerializeObject(object @object);
        string GetOnClickEvent(object @object);
        void AddOnClickEvent(ref HtmlNode anchorNode, object @object, bool overrideEvent = false);
        string GetGtmEvent(object @object);
    }
}