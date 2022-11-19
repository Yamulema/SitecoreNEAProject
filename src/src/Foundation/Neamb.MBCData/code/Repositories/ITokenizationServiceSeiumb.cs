using Neambc.Neamb.Foundation.Configuration.Pipelines;

namespace Neambc.Neamb.Foundation.MBCData.Repositories
{
    public interface ITokenizationServiceSeiumb : IStringProcessor
    {
        string DeTokenize(string rawText);
    }
}