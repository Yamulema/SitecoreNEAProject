using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Sitecore.Data.Items;

namespace Neambc.UnitTesting.Base.Fakes
{
    public class FakePageSitecoreContext: IPageSitecoreContext
    {
        public Item Current { get; set; }
    }
}
