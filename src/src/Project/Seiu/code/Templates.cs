using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Project.Web
{
    using Sitecore.Data;
    public struct Templates
    {
        public struct SitecoreExtensions
        {
            public struct SiteSettingsGlobal
            {
                public static readonly ID ID = new ID("{46D06DD7-8928-43F3-AC91-64FFC27778DC}");
            }
            public struct SiteSettings
            {
                public static readonly ID ID = new ID("{BDB7DE45-9794-4B0E-AADE-2D9A8D227FCB}");
                public struct Fields
                {
                    public static readonly ID GeneralJavaScriptCodeTop = new ID("{DF6F8F62-64DB-440F-9055-D7480BE9C139}");
                    public static readonly ID GeneralJavaScriptCodeBottom = new ID("{DAAC8CF8-C59D-484B-ADC9-12F9E65BE30B}");
                    public static readonly ID GeneralMetadata = new ID("{18B72E62-5F91-4115-AB12-9C38D4607049}");
                    public static readonly ID SEIUMBLogo = new ID("{3195DF63-AC35-41CB-B2E9-F637BFF61811}");
                    public static readonly ID MobileLogin = new ID("{1C983212-48B6-4FBE-B1BB-541121E980C1}");
                    public static readonly ID Registration = new ID("{39CB2088-E064-41DB-A2F8-38D83F03BA9B}");
                    public static readonly ID Welcome = new ID("{5A54342D-A276-49AA-ABA2-61167E49AAB0}");
                    public static readonly ID NotYou = new ID("{12454F06-278D-400F-90A3-C737A0E6B013}");
                    public static readonly ID Logout = new ID("{2F93A0CF-1044-46BB-95C1-134093A015A5}");
                    public static readonly ID FooterContent = new ID("{28DA50CB-FD52-4203-9AB2-2069BB4974A9}");
                    public static readonly ID SearchText = new ID("{AE3535A3-87A7-4A45-8146-81901DBCA0C7}");
                    public static readonly ID Profile = new ID("{39A6155D-C445-441E-B693-C4BACEBDF4AD}");
                    public static readonly ID Username = new ID("{3F9D1016-499F-4985-A263-0C76C264C582}");
                    public static readonly ID Password = new ID("{5A8F02C1-6C91-4CD8-B767-94728C49B6BE}");
                }
            }
            public struct Help
            {
                public static readonly ID ID = new ID("{0726C648-5FCB-4614-A50D-3A5F6410D5E8}");
                public struct Fields
                {
                    public static readonly ID HelpSectionContent = new ID("{5FC898AF-82C4-4C90-B42C-CF3FC98163D3}");
                }
            }                   
        }
        public struct HasPageContent
        {
            public static readonly ID ID = new ID("{4C30E129-CB3B-4445-84ED-219CB46B7D7E}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{FE806046-C528-4C76-9AC8-E1E31A4742B4}");
                public static readonly ID Summary = new ID("{6091DE65-8191-4B69-8193-E4D5D21F5D0D}");
                public static readonly ID Body = new ID("{FC7C8C2A-1003-4734-BE98-35024110C2F5}");
                public static readonly ID Image = new ID("{CFCC6ACC-1678-4A6C-BA8E-0881625B8DEF}");
                public static readonly ID Author = new ID("{DE9C1567-49EB-4A93-B137-30AE22DECB87}");
            }
        }
        public struct Home
        {
            public static readonly ID ID = new ID("{72051F7F-87F7-4279-B455-5A7EACA902F5}");
        }

        public struct ProfilePage
        {
            public static readonly ID ID = new ID("{C544123B-E481-4A1F-AD74-8EF77D48DDC7}");
        }

        public struct LoginMobilePage
        {
            public static readonly ID ID = new ID("{9EE7F15D-D997-4C88-8FCE-1C44E2C078FB}");
        }
        public struct RailCard
        {
            public static readonly ID ID = new ID("{AF95CCC9-5805-4C53-9DEF-EB6362539B1F}");
            public struct Fields
            {
                public static readonly ID Icon = new ID("{47F80D79-005E-4BD5-83E4-E203022DC1AE}");
                public static readonly ID Title = new ID("{9E215C0A-7E77-45DA-887A-73729E24302F}");
                public static readonly ID Image = new ID("{F4A03B66-8C14-4C94-A560-4F74C9E62973}");
                public static readonly ID Body = new ID("{91E9A811-1D9D-4226-AEBE-6E4C888B0CD1}");
            }
        }
        public struct OpenContent
        {
            public static readonly ID ID = new ID("{DF575C0B-D846-4852-B345-015376438C7E}");
            public struct Fields
            {
                public static readonly ID OpenContent = new ID("{71CD0221-254F-42A0-B08A-E790E9362B2D}");
            }
        }

        public struct ErrorTemplate
        {
            public static readonly ID ID = new ID("{C5F4B6B8-A53A-4148-903D-94A0C08894F6}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{4E0CCD80-281F-4BBD-B288-63F810623C2C}");
                public static readonly ID Body = new ID("{25DF7E83-4554-4B36-98D2-6359D8C7DF59}");
                public static readonly ID ContinueButton = new ID("{595B93C5-514A-410C-95E2-C597E1189FAF}");
            }
        }

        public struct ThankYouTemplate
        {
            public static readonly ID ID = new ID("{A7FD5976-7636-4EB2-A6F2-2270CFFBEB04}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{40B3FB3F-B0F2-46C0-8B56-4C570F6A3508}");
                public static readonly ID Body = new ID("{8B8DBBBF-4A2E-4B79-9154-738CB28D2C17}");
                public static readonly ID ContinueButton = new ID("{DE0333AF-6DB9-4C01-9D29-9D19435EF24E}");
            }
        }

        public struct ResitrationThankYouTemplate
        {
            public static readonly ID ID = new ID("{DFBCCCC4-891E-4677-B499-A0B0739482C9}");
            public struct Fields
            {
                public static readonly ID UnverifiedMemeber = new ID("{7191CB07-19D5-4574-A5D2-CEE11318614B}");
            }
        }

        public struct OneColumnMiscellaneousPageType
        {
            public static readonly ID ID = new ID("{2D96EC72-CD15-45E1-BFFB-06AF9804BB68}");
        }
        public struct TwoColumnMiscellaneousPageType
        {
            public static readonly ID ID = new ID("{EF07CBF6-E44B-45CE-B502-9A8592266BA1}");
        }
        public struct TwoColumnListPageType
        {
            public static readonly ID ID = new ID("{AD0853BA-639C-44BA-8EF9-EF4AE713B631}");
        }

        public struct LandingPageTemplate
        {
            public static readonly ID ID = new ID("{FBFA82BD-043D-4E27-B8F7-9A2537C8E908}");
            public struct Fields
            {
                public static readonly ID CampaginCode = new ID("{72CE6A74-93E1-4B9C-9F68-169E12FEBDD0}");
                public static readonly ID HasSecureContent = new ID("{2F1DBCCB-4C60-42AF-B4D7-71D3576CE216}");
                public static readonly ID DefaultContent = new ID("{7EAEF1C7-C4D4-4C7F-B53E-EAEAEE47DD3E}");
                public static readonly ID SecureContent = new ID("{8CC29950-6EBA-4AEE-BC06-73C6B099E117}");
            }
        }

        public struct DuplicateRegistrationPage
        {
            public static readonly ID ID = new ID("{DFA7FBD1-EB44-4DF3-BF86-1438F7ADEFE4}");
        }
		public struct NotFoundPage
		{
			public static readonly ID ID = new ID("{D99A0545-09D3-4EF9-ADBD-4D0D231E1E03}");
		}
	}
}