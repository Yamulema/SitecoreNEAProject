using Neambc.Neamb.Foundation.Membership.Model;


namespace Neambc.Neamb.Foundation.Membership.Interfaces
{
    public interface ILoginGtmManager
    {
        string GetGtmFunction(LoginGtmStatus loginGtmStatus);
    }
}