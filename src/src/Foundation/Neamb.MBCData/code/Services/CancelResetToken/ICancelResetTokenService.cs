using Neambc.Neamb.Foundation.MBCData.Model.CancelResetToken;

namespace Neambc.Neamb.Foundation.MBCData.Services.CancelResetToken
{
    public interface ICancelResetTokenService
    {
        bool CancelResetToken(string username, int unionId);
    }
}
