using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    [Service(typeof(IRenderingSitecoreContext))]
    public class RenderingSitecoreContext : IRenderingSitecoreContext
    {
        public Item Current {
            get
            {
                return RenderingContext.Current.Rendering.Item;
            }
        }
    }
}