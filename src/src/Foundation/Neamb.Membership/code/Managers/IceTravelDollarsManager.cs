using Neambc.Neamb.Foundation.DependencyInjection;
using Neambc.Neamb.Foundation.MBCData.Services.IceDollars;


namespace Neambc.Neamb.Foundation.Membership.Managers
{
    [Service(typeof(IIceTravelDollarsManager))]
    public class IceTravelDollarsManager : IIceTravelDollarsManager
    {
        private readonly IIceDollarsService _iceDollarsService;
        
        public IceTravelDollarsManager(IIceDollarsService iceDollarsService) {
            _iceDollarsService = iceDollarsService;            
        }

        public int GetBalance(string mdsId)
        {
            if (!string.IsNullOrEmpty(mdsId))
            {
                if (int.TryParse(mdsId, out int mdsIdNumber))
                {
                    var response = _iceDollarsService.GetBalance(mdsIdNumber);
                    if (response.Success && response.Data != null) return response.Data.PointsBalance;
                }
            }
            return 0;
        }
    }
}