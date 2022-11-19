using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Repositories.Base {
    public interface IMBCRestfulService {
        RestResultDto<T> Post<T>(RestRequestDto restRequestDto);
    }
}
