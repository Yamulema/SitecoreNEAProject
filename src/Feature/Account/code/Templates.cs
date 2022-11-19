using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Account
{
    public struct Templates
    {
        public struct LocalCode {
            public static readonly ID ID = new ID("{3BFC374A-C171-4271-B411-E103AD370DC2}");

            public struct Fields {
                public static readonly ID Id = new ID("{4E5AF70D-E2A1-48A8-B230-DA5839BD4FC3}");
                public static readonly ID Name = new ID("{A5A6D562-BA94-453A-BCA5-4F888D9D6DDD}");
                public static readonly ID LocalDivision = new ID("{89A284C6-DD26-4562-9910-CE199C236C54}");
            }
        }

        public struct NameValueItem
        {
            public static readonly ID ID = new ID("{71BE9887-E50B-426E-A7EF-18F27844BA0C}");

            public struct Fields
            {
                public static readonly ID ItemName = new ID("{73494340-4626-4D41-A5E7-F0EC2664C0B0}");
                public static readonly ID ItemValue = new ID("{6E092B84-F2CE-477A-AF26-DB4C0BF85AD0}");
            }
        }
    }
}