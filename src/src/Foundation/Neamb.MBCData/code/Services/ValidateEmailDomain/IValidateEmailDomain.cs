using Neambc.Neamb.Foundation.MBCData.Model.ValidateEmailDomain;

namespace Neambc.Neamb.Foundation.MBCData.Services.ValidateEmailDomain
{
    public interface IValidateEmailDomain
    {
        ValidateEmailDomainResponse ValidateEmailDomainStatus(string Email);
    }
}