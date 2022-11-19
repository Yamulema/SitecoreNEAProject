using Neambc.Neamb.Foundation.MBCData.Managers;
using Sitecore.Data.Items;

namespace Neambc.UnitTesting.Base.Fakes
{
    public class FakeRenderingSitecoreContext : IRenderingSitecoreContext
    {
        public Item Current { get; set; }
    }
}
