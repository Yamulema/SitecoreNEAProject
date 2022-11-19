using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.ClickAndSave
{
    public struct Templates
    {
        public struct LoginFormClickAndSave
        {
            public static readonly ID ID = new ID("{016AA7A8-5AE6-44A1-9693-10F24E062F90}");

            public struct Fields
            {
                public static readonly ID Headline = new ID("{200F8366-40C3-4075-855B-4B34D45DBFA8}");
                public static readonly ID Subheadline = new ID("{13D1E6D8-FE7B-489C-8622-835EF410B77B}");
                public static readonly ID Logo = new ID("{B94B4FF0-D291-464B-BAA2-983397943271}");
                public static readonly ID Password = new ID("{DC171172-C4D8-47C6-9634-D625ACA90889}");
                public static readonly ID Tooltip = new ID("{99598970-9AA4-42EF-BA69-DF222506D949}");
                public static readonly ID Show = new ID("{058809CB-3B63-405F-86D7-3F4219AE21B2}");
                public static readonly ID LoginButton = new ID("{556F6859-59EC-4A5F-8DD8-932012369210}");
                public static readonly ID EmptyPassword = new ID("{E1DE101A-98D9-4005-8CAD-5ED7430F2B75}");
                public static readonly ID InvalidCredentials = new ID("{35700334-A5BD-4AD3-8440-161165CA9ACB}");
                public static readonly ID AccountLocked = new ID("{8F4981DD-A3E1-4C15-9E01-D011E508A950}");
                public static readonly ID AccountAlreadyLockedValidToken = new ID("{63317379-9029-4A56-ABA8-82504E2DABDA}");
				public static readonly ID TimeOut = new ID("{859F19C5-8D62-4818-86A4-076FB7D14B68}");
                public static readonly ID NotEligible = new ID("{4AEAF65B-B117-4FAB-8C73-0A3BA1B75A5D}");
	            public static readonly ID DefaultClickSaveUrl = new ID("{348700E7-C735-4086-9DC5-AFEE7C117D01}");
	            public static readonly ID PasswordReset = new ID("{0ED33F4D-F131-4CFC-80CC-0BE360783D90}");
            }
		}
    }
}