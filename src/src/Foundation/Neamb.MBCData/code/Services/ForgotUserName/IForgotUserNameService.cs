
using Neambc.Neamb.Foundation.MBCData.Model.ForgotUserName;

namespace Neambc.Neamb.Foundation.MBCData.Services.ForgotUserName
{
    public interface IForgotUserNameService
    {
        ForgotUserNameResponse ForgotUserNameStatus(string firstName, string lastName, string zipCode, string dob, int unionId);
    }
}