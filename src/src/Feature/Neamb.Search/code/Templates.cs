using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Neambc.Neamb.Feature.Search
{
    public struct Templates
    {
        public struct ContentFilter
        {
            public static readonly ID ID = new ID("{D8BE87A5-AD90-45B3-9B5B-AF2963F14423}");

            public struct Fields
            {
                public static readonly ID Header = new ID("{83FD5A48-2CFA-4322-9A7D-DC035613EE4B}");
                public static readonly ID SeeMoreText = new ID("{F1CFE31C-4C09-483F-A5D7-D9C0E993295D}");
                public static readonly ID MatchAttribute = new ID("{6F1B79D1-C060-45A9-BAE9-4B1512C27216}");
                public static readonly ID AddGoal = new ID("{EA2DA15C-C22E-4110-9F2A-3E4DB27DAB3F}");
                public static readonly ID AddTopic = new ID("{660DC894-21B1-428C-8DB7-5FC7782E8BE5}");
                public static readonly ID AddProductCategory = new ID("{A99060ED-9C4D-43A1-A466-634C35F8D7B1}");
                public static readonly ID AddLifeEvent = new ID("{CFA5555B-6948-4495-8361-5C726C0AE4CA}");
                public static readonly ID AddSeasonality = new ID("{D7EE5543-4C6E-4A22-9B9C-F25E94649278}");
                public static readonly ID Items = new ID("{3C6A9660-B4D0-4ACF-BAF8-E69B72C2A0FC}");
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
            public static readonly ID Home = new ID("{A2CE3A94-3500-4BBF-94B3-67A637097DD0}");
            public static readonly ID Goal = new ID("{71F83DE6-AA9C-4F1D-9FCC-C04B2EF2832D}");
            public static readonly ID ProductCategory = new ID("{EEA0A232-C12B-4422-99C4-216A4E16FCDF}");
            public static readonly ID Product = new ID("{D1889EB8-BE95-4E99-B8E9-3A0AEB8F4800}");
            public static readonly ID Article = new ID("{15547760-E485-446C-B7C0-98A660FD577E}");
            public static readonly ID LandingPage = new ID("{E34BACDF-686B-4D8A-8FD1-D6A2E52EA1D9}");
            public static readonly ID About = new ID("{12EF3A31-C3C4-49C3-A1CC-AAA54364C242}");
            public static readonly ID VideoPage = new ID("{F228472C-8A4D-4D12-8CC2-216138E6A43A}");
            public static readonly ID GenericPageContentPage = new ID("{B496C634-6BCC-43BD-A465-6095DD468467}");
            public static readonly ID Newsletter = new ID("{AF363696-12E4-47F8-8D6D-1F71E45BE12A}");
            public static readonly ID Sweepstakes = new ID("{0B3845C3-566D-4100-A961-0C32C9018320}");
            public static readonly ID ContactUs = new ID("{073A5AAF-424C-410F-8958-F6F650920186}");
            public static readonly ID GuidePage = new ID("{473F7893-BE7D-4E9E-9F88-2823CB79E36D}");
            public static readonly ID CalculatorPage = new ID("{75EF483A-4C9D-4215-A1F6-2A901B9B562E}");
            public static readonly ID ContestSubmissionPage = new ID("{3821604B-5F69-4D8B-91DB-2D9D6907C134}");
            public static readonly ID ConstestVotePage = new ID("{CEFF0DD5-20FD-40A9-ACD3-C624736B57A2}");
            public static readonly ID MarketplacePage = new ID("{75CC98A4-6E6C-481B-ADE6-CB3C3941AD91}");
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
            }
        }
        public struct Article
        {
            public static readonly ID ID = new ID("{81549AF0-8E19-4FED-B839-1717A7850DC9}");
            public struct Fields
            {
                public static readonly ID Genre = new ID("{72428A2D-B547-4E83-8B2A-3319B6FE8394}");
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
        public struct SearchResults
        {
            public static readonly ID ID = new ID("{4DA134F9-C31D-40A2-94BD-F11726984040}");
            public struct Fields
            {
                public static readonly ID SearchBoxPlaceholder = new ID("{7C0DC4D6-B0A2-491F-982E-F6777EE814B0}");
                public static readonly ID ResultsCountLabel = new ID("{9AC4C202-9569-4C3B-817E-6F13DEDFA031}");
                public static readonly ID MoreText = new ID("{2BDBA689-CD6E-446E-A25D-26A1066D5805}");
                public static readonly ID NoResults = new ID("{7EC31966-1C76-4894-90FE-7CE6107E3FC0}");
            }
        }
        public struct SiteSettings
        {
            public static readonly ID ID = new ID("{4A236EC7-6029-4230-A16D-688A1CA89832}");

            public struct Fields
            {
                public static readonly ID SearchPage = new ID("{826ECFA8-7526-42A1-BA6B-CDDD30C0A7A3}");
                public static readonly ID Seasonality = new ID("{50CB5E07-0A92-4D9C-8F66-5F0422DE119C}");
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

        public struct Store
        {
            public static readonly ID ID = new ID("{26F6C7C8-D74B-474B-A531-4E11F6A07F64}");
            public struct Child
            {
                public const string BaseChild = "Base";
                public const string TotalChild = "Total";
            }
            public struct Fields
            {
                public static readonly ID Name = new ID("{365E9AE1-60E6-4889-B838-D66BDE087B8D}");
                public static readonly ID Categories = new ID("{8E5C94E6-FAE2-4495-91B2-F2E0980558E7}");
                public static readonly ID NeambEnable = new ID("{22F57430-71CD-4F0E-AB3F-844C3142AF20}");
                public static readonly List<string> Images = new List<string> {
                    "icon22460_t", "icon33690_t", "smalllogo_t", "logomobile_t", "logoemail_t",
                    "icon11230_t", "banner_t", "logomobile2x_t", "logomobile3x_t", "feedsquarelogo_t"
                };
            }
        }

        public struct StoreReward {
            public static readonly ID ID = new ID("{9E28EA74-CAB3-4DF8-B3CF-6A87ECAEFFE7}");

            public struct Fields {
                public static readonly ID Name = new ID("{83E35627-C302-4531-B3F8-B76E48DE4112}");
                public static readonly ID Amount = new ID("{A66876B2-9649-410B-ACBC-2BBAA18456AA}");
                public static readonly ID Display = new ID("{AF0D4178-710F-49F4-8609-D03B9350B0B8}");
            }
        }
    }
}