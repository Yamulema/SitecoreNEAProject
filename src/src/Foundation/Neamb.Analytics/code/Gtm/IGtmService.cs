using HtmlAgilityPack;
using Neambc.Neamb.Foundation.Configuration.Pipelines;

namespace Neambc.Neamb.Foundation.Analytics.Gtm
{
    public interface IGtmService {
        string SerializeObject(object @object);
        string GetOnClickEvent(object @object);
        void AddOnClickEvent(ref HtmlNode anchorNode, object @object, bool overrideEvent = false);
		string GetGtmEvent(object @object);
        string GetParameterGtmEvent(object @object);
    }
}