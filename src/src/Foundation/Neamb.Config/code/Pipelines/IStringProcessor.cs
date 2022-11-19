using Sitecore.Pipelines.RenderField;

namespace Neambc.Neamb.Foundation.Configuration.Pipelines
{
    public interface IStringProcessor {
        string Process(string input, bool overrideEvents = false, RenderFieldArgs args = null);
    }
}