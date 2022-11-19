using Neambc.Neamb.Foundation.MBCData.Model.Rakuten;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests;
using Neambc.Neamb.Foundation.MBCData.Model.Rest.Responses;

namespace Neambc.Neamb.Foundation.MBCData.Services.Rakuten
{
    public interface IRakutenStoreRestRepository {
        RestResultDto<StoreResponse> GetStore(string etagStored);
        RestResultDto<StoreDetailResponseTop> GetStoreDetail(RestRequestDto restRequestDto);
    }
}
