using System.Collections.Generic;
using System.Threading.Tasks;
using Neambc.Neamb.Foundation.MBCData.ExactTargetService;

namespace Neambc.Neamb.Foundation.MBCData.Managers {
	/// <summary>
	/// Responsible for communications with ExactTarget 
	/// </summary>
	public interface IExactTargetProxy {
		bool Send(string customerKey, Subscriber subscriber);
		IEnumerable<APIObject> Retrieve(RetrieveRequest retrieveRequest);
		bool Update(UpdateRequest updateRequest);
		bool Create(CreateRequest createRequest);
		Task<bool> CreateAsync(CreateRequest createRequest);
        CreateResult[] CreateCall(CreateRequest createRequest);
    }
}