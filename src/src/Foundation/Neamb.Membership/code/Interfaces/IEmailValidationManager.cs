using Neambc.Neamb.Foundation.Membership.Model;

namespace Neambc.Neamb.Foundation.Membership.Interfaces
{
    public interface IEmailValidationManager
    {
        ResultEmailValidation IsValid(string email, bool? validateusername);
    }
}