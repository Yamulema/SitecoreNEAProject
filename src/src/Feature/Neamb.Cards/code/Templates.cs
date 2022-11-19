using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Cards
{
    public struct Templates
    {
        public struct RelatedContent
        {
            public static readonly ID ID = new ID("{7D0364E2-6183-4D42-A413-17C5D29E46D1}");
            public struct Fields
            {
                public static readonly ID BackgroundColor = new ID("{6D28E9EB-B984-4BB1-9F4B-8194D4C73899}");
                public static readonly ID Header = new ID("{1BE4FF35-2710-4789-8C61-03203E03F665}");
                public static readonly ID MatchAttribute = new ID("{2F7EB403-D984-4F90-8653-6EF1C8A37029}");
                public static readonly ID Items = new ID("{1052DE83-96C1-4894-BDD6-29BE0F98B1EA}");
            }
        }
        public struct ProductCardsCarousel
        {
            public static readonly ID ID = new ID("{DF21984B-6ADC-4371-8DBD-D77CEB5CFAE8}");
            public struct Fields
            {
                public static readonly ID BackgroundColor = new ID("{27F558D7-44B9-4585-9D9C-9335C50A19B5}");
                public static readonly ID Headline = new ID("{41426959-84D6-4CDF-BC0A-D614371BEE59}");
                public static readonly ID Subheadline = new ID("{6AFEE5C4-EA8D-4CF6-BF6B-07EBAAEF6366}");
                public static readonly ID MatchAttribute = new ID("{2C6B311A-84F7-4478-9FE2-52FA710FEDA7}");
                public static readonly ID Items = new ID("{87764CBC-4664-414A-9C48-0975D6B9C772}");
                public static readonly ID BottonText = new ID("{57011A27-3B67-4D37-9E3E-A8654461EF30}");
            }
        }
        public struct Attributes
        {
            public static readonly ID ID = new ID("{7E1D57A0-5B8F-4309-B488-1B60C65BF434}");
            public struct Fields
            {
                public static readonly ID Seasonality = new ID("{883C16B8-D1DF-447F-B93A-AFB22D82FF3E}");//
                public static readonly ID Topics = new ID("{CA799891-B9D8-4B09-BCC5-E64AC00FECB0}");//
                public static readonly ID LifeEvents = new ID("{7F1529CB-650D-4495-A362-6F8282881254}");//
                public static readonly ID Genre = new ID("{72428A2D-B547-4E83-8B2A-3319B6FE8394}");
            }
        }
        public struct GoalsAttribute
        {
            public static readonly ID ID = new ID("{A401323D-E10D-40E2-87FA-A286748C47AC}");
            public struct Fields
            {
                public static readonly ID Goals = new ID("{8B5067D5-6BB1-41E3-B253-6B36BF839D12}");//
            }
        }
        public struct PageInfo
        {
            public static readonly ID ID = new ID("{367B8E27-D435-49A7-BA34-5D8F44FC1EB8}");
            public struct Fields
            {
                public static readonly ID PageTitle = new ID("{F71F7747-F88D-499B-AC69-D3A6DC9B0A88}");
                public static readonly ID ShortDescription = new ID("{FEC5D746-A317-48EA-BD97-D81D7FF151D6}");
                public static readonly ID Thumbnail = new ID("{A552581C-7279-4485-9BCD-32CF536565F8}");
                public static readonly ID SmallThumbnail = new ID("{D37A047F-6E3F-4290-B281-6C9C4F71F0E7}");
                public static readonly ID LargeThumbnail = new ID("{C7AAFFE4-93CA-4337-AD6E-FA700F6720EE}");
            }
        }
        public struct Article
        {
            public static readonly ID ID = new ID("{81549AF0-8E19-4FED-B839-1717A7850DC9}");
        }

        public struct ProductCategoriesAttribute
        {
            public static readonly ID ID = new ID("{0559E9DC-8D47-4F19-B3FC-FEB431BA0BB5}");
            public struct Fields
            {
                public static readonly ID ProductCategories = new ID("{E23A009C-D7B3-4202-B7A5-8C7DB3F4290D}");//
            }
        }

        public struct PageTypeTemplates
        {
            public static readonly ID Article = new ID("{15547760-E485-446C-B7C0-98A660FD577E}");
            public static readonly ID VideoPage = new ID("{F228472C-8A4D-4D12-8CC2-216138E6A43A}");
            public static readonly ID Product = new ID("{D1889EB8-BE95-4E99-B8E9-3A0AEB8F4800}");
            public static readonly ID ProductCategory = new ID("{EEA0A232-C12B-4422-99C4-216A4E16FCDF}");
            public static readonly ID Goal = new ID("{71F83DE6-AA9C-4F1D-9FCC-C04B2EF2832D}");
            public static readonly ID MarketPlace = new ID("{75CC98A4-6E6C-481B-ADE6-CB3C3941AD91}");
        }
        public struct PageTypes
        {
            public static readonly ID ID = new ID("{F08FDF64-09DE-407F-9EC6-52ECB3A27648}");
            public struct Article
            {
#pragma warning disable RECS0146 // Member hides static member from outer class
				public static readonly ID ID = new ID("{81549AF0-8E19-4FED-B839-1717A7850DC9}");
#pragma warning restore RECS0146 // Member hides static member from outer class
				public struct Fields
                {
                    public static readonly ID PageInfoPageTitle = new ID("{F71F7747-F88D-499B-AC69-D3A6DC9B0A88}");
                    public static readonly ID PageHeaderSubheadline = new ID("{5EE361E9-BF9E-4D1F-8CE0-9403D6D7C838}");
                    public static readonly ID ArticleHeroImage = new ID("{ADABEDE9-5073-4B40-BD3B-129D451B160B}");
                    public static readonly ID PageBodyBody = new ID("{845D44DB-ABA6-406C-8300-0ABAAA9A19A3}");
                    public static readonly ID PageBodyBodyBackgroundColor = new ID("{CAECAFD2-BA43-4B8E-9CFE-1921500D0F6C}");
                    public static readonly ID BodyCopyBodyHeightLimit = new ID("{2C35129E-3A76-4324-AC98-24572E226A82}");
                }
            }
        }

        public struct ContentDisplayComponents
        {
            public struct StackableItem
            {
                public struct Fields
                {
                    public static readonly ID TextPlacement = new ID("{D44559A0-3C89-4790-B3B9-25D3C0ADAE90}");
                    public static readonly ID Image = new ID("{656522C7-F110-4B03-A699-576E64F710CE}");
                    public static readonly ID Video = new ID("{73FF44C8-8D81-4865-8EAE-3C139D7101EA}");
                    public static readonly ID Callout = new ID("{C1336B16-ECB6-45ED-807F-242CD50243E9}");
                    public static readonly ID TextBlock = new ID("{30503123-7254-4B06-A096-FB0EAC827B26}");
                }
            }
            public struct TwoColumnStackable
            {
                public struct Fields
                {
                    public static readonly ID Header = new ID("{55492B7E-2EEE-49FA-A8FE-EDFE3CDA3C81}");
                    public static readonly ID Subhead = new ID("{9A668405-B0B3-4F4B-9058-C58C389633B4}");
                }
            }
        }

        public struct TabbedHero
        {
            public static readonly ID ID = new ID("{1BF329EC-B5BB-48F8-B637-FCE402385B3B}");
            public struct Fields
            {
                public static readonly ID Timer = new ID("{8C1DFD55-49C3-4B55-B3FE-B4BC55264E19}");
            }
        }
        public struct TabbedHeroItem
        {
            public static readonly ID ID = new ID("{4B61408D-59E2-43A3-ACC9-49D5FB879ECC}");
            public struct Fields
            {
                public static readonly ID Destination = new ID("{9814320F-501A-469A-A6B9-236870F94D5B}");
                public static readonly ID Image = new ID("{DA795E44-7B51-4879-8D64-3BD88B6C98E8}");
            }
        }
        public struct TwoColumCarousel
        {
            public static readonly ID ID = new ID("{93D819E3-5AA4-49F0-A392-D2E80659602E}");
            public struct Fields
            {
                public static readonly ID Header = new ID("{10186210-F257-4806-B344-C2D5E839D6C1}");
            }
        }
        public struct CarouselItem
        {
            public static readonly ID ID = new ID("{DC1D94E5-2226-44FB-BCB0-F8C5AAE57DAB}");
            public struct Fields
            {
                public static readonly ID Image = new ID("{346D7557-569E-4D27-952D-CD2D66C98F94}");
                public static readonly ID PullText = new ID("{AEA99C72-7FB3-4D2A-903F-FEE20C59CF2E}");
                public static readonly ID Destination = new ID("{520ED7D0-3DAC-41DC-A3D0-D81C32DA7BC1}");
                public static readonly ID Headline = new ID("{6B1D73B3-6C24-4888-AB3D-FA6F4EE43D09}");
                public static readonly ID Description = new ID("{A4862438-2F57-427A-A182-294BE5E594BD}");
            }
        }
        public struct MultiRowProductCards
        {
            public static readonly ID ID = new ID("{B4EA30EA-D400-4B08-A880-F305B91EDABC}");
            public struct Fields
            {
                public static readonly ID ResultsCountText = new ID("{D6A40A11-1540-46B3-A696-3E4A4216097E}");
                public static readonly ID Items = new ID("{6BFB5A8F-C459-4D0A-93D2-139764D5B6ED}");
                public static readonly ID CtaText = new ID("{3170C965-8D7D-40F3-879E-3EF488D1D131}");
                public static readonly ID TermsAndConditionsText = new ID("{8A34B6AF-D560-41A9-AE74-30A34A978713}");
            }
        }
        public struct ProductCTAs
        {
            public static readonly ID ID = new ID("{CDCEBEEF-02EB-4CF2-86A3-41BC0D31F613}");

            public struct Fields
            {
                public static readonly ID Highlights = new ID("{E9B0F95E-E944-4541-9D24-635FC272322E}");
                public static readonly ID ComingSoon = new ID("{4040EB4A-616E-4D6E-8ED8-0B24437507B6}");
            }
        }
        public struct ProductCategoriesAttributes
        {
            public static readonly ID ID = new ID("{0559E9DC-8D47-4F19-B3FC-FEB431BA0BB5}");

            public struct Fields
            {
                public static readonly ID ProductCategories = new ID("{E23A009C-D7B3-4202-B7A5-8C7DB3F4290D}");
            }
        }
        public struct FiveContentItems
        {
            public static readonly ID ID = new ID("{CC089AAC-86A2-4D26-813B-03F8CDF87A37}");
            public struct Fields
            {
                public static readonly ID BackgroundColor = new ID("{3828258D-2CB8-4BC1-BEE0-72BCEC4626A5}");
                public static readonly ID Header = new ID("{FA54550C-59A8-493C-B5BB-DAC67877B78F}");
                public static readonly ID MatchAttribute = new ID("{922143F1-2CC3-4F79-84E1-041D0CE12C90}");
                public static readonly ID Items = new ID("{C2ADB603-8366-4AAB-80B4-BB833A628626}");
            }
        }
        public struct CategoryItem
        {
            public static readonly ID ID = new ID("{CC089AAC-86A2-4D26-813B-03F8CDF87A37}");
            public struct Fields
            {
                public static readonly ID Value = new ID("{EBF38A5A-3631-4950-B7D2-D6D9ED8A33B4}");
            }
        }

        public struct ThreeContentItems
        {
            public static readonly ID ID = new ID("{1D99DC57-5025-4028-BE5D-2342F9EC3F7F}");
            public struct Fields
            {
                public static readonly ID Header = new ID("{152FAF6C-A496-40E9-B39E-899C07EDC537}");
            }
        }

        public struct ThreeColMultirowContentItems
        {
            public static readonly ID ID = new ID("{4D00261A-19FE-47E5-8DF3-D2FEB07E8ED1}");
            public struct Fields
            {
                public static readonly ID Header = new ID("{725EA6AF-B6CF-4DA3-A912-E02A32E01695}");
                public static readonly ID BackgroundColor = new ID("{BB150D84-A6B8-434D-81C6-F681E57917DB}");
                public static readonly ID Items = new ID("{FA31052D-DC2F-4E5B-81CD-D45FB385BE92}");
            }
        }

        public struct TwoColumnGrid
        {
            public static readonly ID ID = new ID("{B57E3F6F-9906-40CD-BFE9-9BE52EE8C16A}");
            public struct Fields
            {
                public static readonly ID Header = new ID("{24086B25-E099-41F7-B74D-60B5AC7068B0}");
                public static readonly ID Subhead = new ID("{39C9C3CE-805B-4F9C-9E85-554BF5A45228}");
            }
        }

        public struct GridItem
        {
            public static readonly ID ID = new ID("{E8C2CC9F-7038-4C75-8209-D3F8129A3D49}");
            public struct Fields
            {
                public static readonly ID BackgroundColor = new ID("{887547A4-B7D7-4A25-A02A-3DAC33B51D77}");
                public static readonly ID Header = new ID("{4BAD3D15-8634-4E01-9905-F8B6D00A0615}");
                public static readonly ID Subhead = new ID("{32F3C1FA-BB37-4CCD-A609-269D2E0FDB9A}");
                public static readonly ID CTA = new ID("{D281069B-337C-4B45-849B-E55574358F37}");
                public static readonly ID Image = new ID("{D292C6B9-B199-4E5E-9ED1-43C39A928389}");
            }
        }

        public struct TwoColumnStackable
        {
            public static readonly ID ID = new ID("{BC734A88-599C-4189-8128-B197A6D5D517}");
            public struct Fields
            {
                public static readonly ID Header = new ID("{55492B7E-2EEE-49FA-A8FE-EDFE3CDA3C81}");
                public static readonly ID Subhead = new ID("{9A668405-B0B3-4F4B-9058-C58C389633B4}");
            }
        }
        public struct Product
        {
            public static readonly ID ID = new ID("{9D5243AD-9807-4335-9777-C9E0419BFE77}");
            public struct Fields
            {
                public static readonly ID TermsAndConditions = new ID("{2BEC068C-14CB-41BA-92C7-C9F871B33EA1}");
                public static readonly ID ProductCardLink = new ID("{11E45E93-607A-4721-B5A1-9EC447248159}");
                public static readonly ID HideTermsAndConditionsOnProductCard = new ID("{EBF13EC3-7CBA-4F37-9216-E09BEAA7323A}");
            }
        }
	    public struct StatisticsCustom
	    {
		    public static readonly ID ID = new ID("{4D53FB3F-03CD-44FC-9CEC-EE81A5E0AD51}");
		    public struct Fields
		    {
			    public static readonly ID LastPublishDate = new ID("{AC72BAE6-B9F8-450D-94F5-87EEF71C5B03}");
		    }
	    }
        public struct FlipCards
        {
            public static readonly ID ID = new ID("{6E574753-5738-4179-A45F-510CA6807CD0}");
            public struct Fields
            {
                public static readonly ID Headline = new ID("{D4BE584D-5646-4F39-96B5-E829779DA95E}");
                public static readonly ID SubHeadline = new ID("{5229663F-1D47-48DF-A1B2-3904061FB18C}");
            }
        }
        public struct FlipCardItem
        {
            public static readonly ID ID = new ID("{100FC06D-CBA5-4EF2-A60E-2027580652FE}");
            public struct Fields
            {
                public static readonly ID BackgroundImage = new ID("{79C636E2-1B75-4289-91E5-34D7B0546FB0}");
                public static readonly ID DefaultText = new ID("{0692CB3B-9EBC-4E28-BCD9-076C0149184F}");
                public static readonly ID FlipText = new ID("{B8818A5F-37E3-43A4-A6A7-41938497324C}");
                public static readonly ID TabText = new ID("{34BBBFE0-9809-492B-B72A-5DE8D697DE52}");
                public static readonly ID FlipButton = new ID("{06DB40D6-428E-4CC5-9224-8CE64356BE87}");
                public static readonly ID FlipBackgroundColor = new ID("{18620376-4C09-4E07-8D0B-92A934D6C5A1}");
            }
        }

        public struct PlanCard
        {
            public static readonly ID ID = new ID("{984E1EC6-EF71-497D-9BF6-CD1ACE65CE6E}");
            public struct Fields
            {
                public static readonly ID Headline = new ID("{2023772C-D46F-4A97-A581-0978B544312F}");
                public static readonly ID CTALink = new ID("{F3D85821-485E-49A6-A9B7-E0EA75C6A7E8}");
                public static readonly ID Description = new ID("{770D3A43-9A45-4447-B272-F2E7D18A4616}");
                public static readonly ID LearnMore = new ID("{9689B547-1780-4FAD-8981-87A0F7D4099E}");
            }
        }

        public struct CarrouselPlanCards
        {
            public static readonly ID ID = new ID("{BD69B4EB-07F5-49C6-9BF2-0BCBD58ACF76}");
            public struct Fields
            {
                public static readonly ID Headline = new ID("{9CE4275A-11E1-4381-B509-65E1F25F91CF}");
                public static readonly ID Cards = new ID("{AF305841-82EA-41C6-AA59-4CAD9B577FD0}");
            }
        }

        public struct TabCard
        {
            public static readonly ID ID = new ID("{CFDF007D-2153-473E-A336-D69EEAE8E58D}");
            public struct Fields
            {
                public static readonly ID TabName = new ID("{94C76A30-0A01-4DD3-BD25-66D637CA3866}");
                public static readonly ID Description = new ID("{FF6E82DD-C5AE-4A38-8A2A-8490E5239310}");
                public static readonly ID Image = new ID("{5024367A-F4BD-4F35-BC8E-830097DAD8E7}");
                public static readonly ID LearnMore = new ID("{21604C96-295B-416A-864A-0EE60F306FB4}");
            }
        }

        public struct TabbedCards
        {
            public static readonly ID ID = new ID("{BE42AC4B-CB49-493E-AF55-3345DFE78B58}");
            public struct Fields
            {
                public static readonly ID Headline = new ID("{FB3F7F99-53C7-49A3-B847-08808E1E523C}");
                public static readonly ID Description = new ID("{00936E02-8F51-48E0-8DC5-7EEE79B1B82A}");
                public static readonly ID Cards = new ID("{8A2C07CC-06D4-45D3-BD6E-B647700A5B47}");
            }
        }

        public struct ThreeColumnContentItem
        {
            public static readonly ID ID = new ID("{63E229D4-43A2-43AF-9C2E-33DCEBF9658C}");
            public struct Fields
            {
                public static readonly ID Image = new ID("{523CA48D-5CE8-412C-99C1-DF60FA24A804}");
                public static readonly ID Headline = new ID("{6E002B0E-1ADC-4035-9479-A04FD2922A53}");
                public static readonly ID Title = new ID("{A5619201-0834-4145-8CFB-50C57AAE4132}");
                public static readonly ID Description = new ID("{F9CC1FF8-77C2-4B87-9B44-CDB7134A5FF2}");
                public static readonly ID LinkAction = new ID("{9944277D-670A-46E0-A108-FE7E908BA452}");
            }
        }
    }
}