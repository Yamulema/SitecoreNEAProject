using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Banner {
    public struct Templates {
        public struct HeadlineHero {
            public static readonly ID ID = new ID("{3B956BDD-4B5F-49E7-B5C2-606E3FE71420}");

            public struct Fields {
                public static readonly ID HeroImage = new ID("{0B9DD861-0196-463A-B3C4-7BB4ED542DD8}");
                public static readonly ID PageTitle = new ID("{D6D03B91-F197-4B36-8016-5542DB2AA9B2}");
                public static readonly ID Subheadline = new ID("{80C8EA58-DDD7-43E5-AFD3-C4F188A2A806}");
                public static readonly ID LargeHeroImage = new ID("{5F6E1E6E-7D6C-4A79-B97E-11C48E2DA5CD}");
                public static readonly ID QuoteWidget = new ID("{2929303B-7B73-44ED-A322-C641A77169E8}");
                public static readonly ID CenteredText = new ID("{D1126EF3-FD33-4CFE-BA4A-DC902D76910D}");
            }
        }

        public struct PageHeader {
            public static readonly ID ID = new ID("{668D8D54-DC74-475D-902F-611A1EED6371}");

            public struct Fields {
                public static readonly ID LargeHeroImage = new ID("{A67EACA6-6A82-4D9E-8D51-DEF99BA2F221}");
                public static readonly ID HeroImage = new ID("{883F4D18-7002-44CE-8FEB-5A0E17661ECE}");
                public static readonly ID Subheadline = new ID("{5EE361E9-BF9E-4D1F-8CE0-9403D6D7C838}");
                public static readonly ID QuoteWidget = new ID("{ECE94DB0-FE96-4AB3-BECB-8FB8EFC05375}");
                public static readonly ID CenteredText = new ID("{92FB6756-6BB4-432F-9504-CAE07ACB9DDB}");
            }
        }

        public struct PageInfo {
            public static readonly ID ID = new ID("{367B8E27-D435-49A7-BA34-5D8F44FC1EB8}");

            public struct Fields {
                public static readonly ID PageTitle = new ID("{F71F7747-F88D-499B-AC69-D3A6DC9B0A88}");
            }
        }

        public struct Article {
            public static readonly ID ID = new ID("{81549AF0-8E19-4FED-B839-1717A7850DC9}");

            public struct Fields {
                public static readonly ID Subheadline = new ID("{037C5AE6-C685-4707-BDD1-A741D5ECF65F}");
                public static readonly ID HeroImage = new ID("{ADABEDE9-5073-4B40-BD3B-129D451B160B}");
            }
        }

        public struct TopSiteBanner {
            public static readonly ID ID = new ID("{9B0A1205-6419-4A19-B632-15B04F4EB238}");

            public struct Fields {
                public static readonly ID TopBanner = new ID("{A2AB0019-5531-4265-B633-7C4162F73DAB}");
                public static readonly ID TopBackgroundColor = new ID("{444398A9-94AD-4CD2-83F5-4823339381E0}");
                public static readonly ID GTMCode = new ID("{2DF9BFEF-6474-4EFB-976C-47C231B8C4EC}");
            }
        }

        public struct TopSiteBannerSetting {
            public static readonly ID ID = new ID("{9C6C5725-652A-4E05-852A-92AD4000BCC2}");

            public struct Fields {
                public static readonly ID Banner = new ID("{56AF8EEC-CE12-4CEE-BE26-6A2DCBBD9881}");
            }
        }

        public struct SiteSettings {
            public static readonly ID ID = new ID("{C7EADD3C-19BC-463B-B0CC-A862E99E5B50}");

        }
        public struct TwoColumnHeroBanner
        {
            public static readonly ID ID = new ID("{EFCD573B-5E16-480D-9FE9-84D7211604A9}");

            public struct Fields
            {
                public static readonly ID Headline = new ID("{59258618-1E57-418A-BD97-04A330A32201}");
                public static readonly ID SubHeadline = new ID("{1B625A85-E490-4D9A-BAD8-4B9FA932BC1A}");
                public static readonly ID HeroImage = new ID("{5CA177EC-504A-4FB4-B0D9-2193F86403FB}");
                public static readonly ID BackgroundColor = new ID("{AF1F3029-835E-4A07-9CE0-EE877B4E3A00}");
                public static readonly ID HeroCtaLink = new ID("{3D8C9653-F129-4340-A184-D9F48502A5C3}");
                public static readonly ID HeroCtaColor = new ID("{7653675E-A81B-483C-808F-628584286303}");
                public static readonly ID OverlayLink = new ID("{B88ABBD1-A9EC-4C18-AD32-2D2819FC3342}");
                public static readonly ID ModalTitle = new ID("{81580431-1742-48A9-92A3-7178B308BE44}");
                public static readonly ID ModalText = new ID("{6AEAD7B4-0864-49FE-A036-9B4111313FAE}");
                public static readonly ID ModalLink = new ID("{5729314A-2661-4F6C-9E94-F177074892D5}");
            }
        }
        public struct CategoryItem
        {
            public static readonly ID ID = new ID("{D1402B59-E079-4856-9DFB-551B6C87B7AE}");

            public struct Fields
            {
                public static readonly ID Value = new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}");
            }
        }
        public struct HeroBanner
        {
            public static readonly ID ID = new ID("{31F2A4C7-F16D-4444-8103-0D205BD5906E}");

            public struct Fields
            {
                public static readonly ID BackgroundColor = new ID("{AEFF1615-518F-476C-B6A2-A25739CE3DC7}");
                public static readonly ID DesktopBackgroundImage = new ID("{A5A84EBF-3DDC-4D79-944D-AB7C47BD9BCB}");
                public static readonly ID MobileBackgroundImage = new ID("{3C02E8CC-71D4-45BD-8B83-AA94438464A2}");
                public static readonly ID TopLayerImage = new ID("{4470E4CD-35E8-4A35-8061-EABD6242BDC1}");
                public static readonly ID Headline = new ID("{7E4AFA07-1019-43E9-A764-521083AAF322}");
                public static readonly ID Subheadline = new ID("{F47F7454-F4DD-499C-B416-BE84C94D03FA}");
                public static readonly ID Body = new ID("{A5F0EDC6-64A4-4D9B-8FB6-64297B030DDC}");
                public static readonly ID TabText = new ID("{882937ED-57B1-48A6-A39A-6E5BC3A11E72}");
            }
        }
        public struct UnionHeadlineHero
        {
            public static readonly ID ID = new ID("{86F41F69-F3BD-4B6E-89CA-659E9A13FA92}");

            public struct Fields
            {
                public static readonly ID Headline = new ID("{9A1C95A7-0588-4E91-BA29-738B11E7471C}");
                public static readonly ID Subheadline = new ID("{FDF928A5-3963-4566-BE8F-48B280BF299B}");
                public static readonly ID ButtonLink = new ID("{A8734DD0-B1AC-41B0-9681-27E80505D315}");
                public static readonly ID DesktopImage = new ID("{387B5F28-D768-47A7-8388-01ACCF9D4940}");
                public static readonly ID MobileImage = new ID("{B9E04DBC-7FD5-41FA-A775-FE93D2F5E250}");                
            }
        }

        public struct InteractiveTallHero
        {
            public static readonly ID ID = new ID("{6FF13F9C-9F19-4A79-8F0C-360E8C91F02F}");

            public struct Fields
            {
                public static readonly ID HeadlineText = new ID("{0106FB5D-A3D8-4EF7-B1F4-837242797DAB}");
                public static readonly ID FooterText = new ID("{115DF8C0-72BB-4BAA-BAC1-8625FD601589}");
                public static readonly ID ButtonLink = new ID("{9C83F59C-2590-41BB-B865-3D15AE8B732C}");
                public static readonly ID Image = new ID("{F6161389-14BF-4958-B45A-3B3D5623A438}");
            }
        }
    }
}