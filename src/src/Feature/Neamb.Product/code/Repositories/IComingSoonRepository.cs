using Neambc.Neamb.Feature.Product.Model;
using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Feature.Product.Repositories
{
	public interface IComingSoonRepository {
		ComingSoonResult GetPropertiesUser(StatusEnum statusUser, ComingSoonRequest comingSoon);
        bool VerifyAlreadyNotified(ComingSoonRequest comingSoon);
    }
}