using Neambc.Neamb.Foundation.DependencyInjection;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    [Service(typeof(IPageSitecoreContext))]
    public class PageSitecoreContext: IPageSitecoreContext
    {
        public Item Current {
            get
            {
                return PageContext.Current.Item;
            }
        }
    }
}