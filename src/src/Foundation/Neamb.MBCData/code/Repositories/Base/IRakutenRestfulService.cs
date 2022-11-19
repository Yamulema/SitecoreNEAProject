using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Repositories.Base {
    public interface IRakutenRestfulService
    {
        RestResultDto<T> Post<T>(RestRequestDto restRequestDto);
        RestResultDto<T> Get<T>(RestRequestDto restRequestDto);
    }
}
