using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neambc.Neamb.Foundation.MBCData.Services {
    public interface ICdnService {
        Task<bool> InvalidateAsync(IEnumerable<string> paths);
    }
}