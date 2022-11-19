using System.Collections.Generic;

namespace Neambc.Neamb.Foundation.MBCData.Model.Rest.Requests
{
    public class RakutenRestRequestDto : RestRequestDto
    {
        public string MdsId { get; set; }
        public string UnionId { get; set; }
        public string CellCode { get; set; }

        public RakutenRestRequestDto(List<KeyValuePair<string,string>> headers) {
            Headers = headers;
        }
    }
}