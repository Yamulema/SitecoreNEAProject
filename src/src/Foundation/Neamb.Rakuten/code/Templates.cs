using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Foundation.Rakuten
{
    public struct Templates {
        public struct RakutenCategory {
            public static readonly ID ID = new ID("{A2EB2D67-1461-4D71-BE58-5B7334D6208A}");

            public struct Fields {
                public static readonly ID Id = new ID("{D2C33213-B7F8-49CA-B70E-0D64BB099A8F}");
                public static readonly ID Name = new ID("{0620BA80-EEF0-4489-A4D9-969841A782BA}");
                public static readonly ID NeambEnable = new ID("{22F57430-71CD-4F0E-AB3F-844C3142AF20}");
                public static readonly ID SeiumbEnable = new ID("{E463DD18-756F-47B4-AA6F-0466F923716C}");

            }
        }

        public struct RakutenItemTemplate {
            public static readonly ID ID = new ID("{40E9D39E-1F56-48A8-BB70-CA76353C5E21}");

        }

        public struct RakutenStoreItemTemplate
        {
            public static readonly ID ID = new ID("{26F6C7C8-D74B-474B-A531-4E11F6A07F64}");

        }

        public struct RakutenCategoryParentItem
        {
            public static readonly ID ID = new ID("{837C7EC5-596B-470B-83E6-186B11248919}");

        }

        public struct RakutenStore
        {
            public static readonly ID ID = new ID("{BA4A37A8-53E9-43D8-B4ED-B81B395CCE29}");

            public struct Fields
            {
                public static readonly ID Id = new ID("{3FDDD9D6-0866-4E6D-B02B-E8776F1F799F}");
                public static readonly ID Name = new ID("{365E9AE1-60E6-4889-B838-D66BDE087B8D}");
                public static readonly ID Description = new ID("{5DB6A22E-5709-4AC9-9E8A-8745B3D52176}");
                public static readonly ID ShortDescription = new ID("{9E2D663B-ACF7-4928-862D-D345BA757B9F}");
                public static readonly ID Categories = new ID("{8E5C94E6-FAE2-4495-91B2-F2E0980558E7}");
                public static readonly ID Banner = new ID("{B03CA598-21E3-4271-AF93-863F7075BA17}");
                public static readonly ID SmallLogo = new ID("{C57B9FB4-1A4B-4B05-9067-35B6F90B05F8}");
                public static readonly ID Thumbnail = new ID("{3EF4A5AA-C656-4FFB-A782-B9045E5F3234}");
                public static readonly ID Icon11230 = new ID("{0992A3F9-57A4-4156-B800-41C3628B26B6}");
                public static readonly ID Icon22460 = new ID("{83863813-469F-4F8F-A181-124749E946CA}");
                public static readonly ID Icon33690 = new ID("{80B65725-DB71-4515-9794-5AEFE2590F30}");
                public static readonly ID LogoEmail = new ID("{3DDEDB3C-6E63-4892-ABCE-5288D0F26B3A}");
                public static readonly ID LogoMobile = new ID("{B6641DCD-A356-440E-B377-C0FAB3E00BCD}");
                public static readonly ID LogoMobile2 = new ID("{A100AB13-9695-4088-969B-C23118211EC4}");
                public static readonly ID LogoMobile3 = new ID("{AB7D0A77-6EFC-49C8-A3F0-1529712EFC91}");
                public static readonly ID FeedSquareLogo = new ID("{A40FE503-7E00-46D5-9652-5AE9CCDF48A6}");
                public static readonly ID MembersOnlyNEA = new ID("{EE2C4820-7A60-4614-8217-90FA5E9EC46B}");
                public static readonly ID MembersOnlySEIU = new ID("{32A4A597-B46A-4C1F-A6C4-F8A53DB0CB2B}");
                public static readonly ID NeambEnable = new ID("{EA93F5B7-DC52-49CB-A848-A6DFBE79FC8B}");
                public static readonly ID SeiumbEnable = new ID("{03DD7EA8-9DD1-4768-890C-53BE237A6C73}");
                //calculated
                public static readonly ID BaseReward = new ID("{429902D7-7451-48CB-823F-14D61A7E2B7D}");
                public static readonly ID TotalReward = new ID("{F5619FA7-55E0-40E7-B2E2-F4FB989D3030}");
                public static readonly ID TypeReward = new ID("{338AA4DC-EE49-4A8F-9655-F27E0E96FF64}");
                public static readonly ID TierReward = new ID("{DABFF084-D8C5-4512-99FC-3E67785D62A1}"); //check box
                public static readonly ID ShoppingUrl = new ID("{4C71C274-0D09-4849-A270-666D5042B276}");
                public static readonly ID ShoppingUrlSeiumb = new ID("{D6C16A05-EC6A-4067-814F-42DBBA75EC2D}");
            }
        }

        public struct RakutenStoreParentItem
        {
            public static readonly ID ID = new ID("{9DB4CECA-8191-4E74-903B-2E1F21389A19}");

        }

        public struct RakutenReward
        {
            public static readonly ID ID = new ID("{DE059214-F9BD-405C-AB10-82D9A0F5DE78}");

            public struct Fields
            {
                public static readonly ID Name = new ID("{83E35627-C302-4531-B3F8-B76E48DE4112}");
                public static readonly ID Amount = new ID("{A66876B2-9649-410B-ACBC-2BBAA18456AA}");
                public static readonly ID Display = new ID("{AF0D4178-710F-49F4-8609-D03B9350B0B8}");
                
            }
        }
        public struct RakutenRewardItemTemplate
        {
            public static readonly ID ID = new ID("{9E28EA74-CAB3-4DF8-B3CF-6A87ECAEFFE7}");

        }
    }
}