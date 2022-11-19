using Neambc.Neamb.Foundation.MBCData.Model.IceDollars;

namespace Neambc.Neamb.Foundation.MBCData.Services.IceDollars
{
    public interface IIceDollarsService
    {
        IceDollarsResponse GetBalance(int mdsId);
    }
}
