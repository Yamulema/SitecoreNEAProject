using Neambc.Neamb.Foundation.MBCData.Model.Login;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.Managers
{
	public interface IIceTravelDollarsManager
	{
        int GetBalance(string mdsId);
    }
}