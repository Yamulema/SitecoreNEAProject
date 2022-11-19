using Sitecore.Data;

namespace Neambc.Neamb.Feature.Rakuten
{
    public struct Templates
    {
        public struct StoresList
        {
            public static readonly ID ID = new ID("{D31E8AF4-08C6-4255-88ED-2A3F4F51189D}");

            public struct Fields
            {
                public static readonly ID StoresListLabel = new ID("{35BB9375-3545-4FB3-9D6A-1222B7FDB783}");
                public static readonly ID SeeMoreText = new ID("{CCD8F631-E199-4D09-B1FE-74B97D9AB9AE}");
                public static readonly ID NoResultsText = new ID("{433FF397-4368-4CE0-A8A3-EA8670CE4585}");
                public static readonly ID NoFavoriteStores = new ID("{2C0B2B43-35EA-477A-ADD7-48E2A86BACF1}");
                public static readonly ID Notification = new ID("{1915201E-E0F5-4063-9B8B-CCAC15D1ABB9}");
                public static readonly ID EligibilityOverlay = new ID("{CA8C9AD0-883B-4AE0-A6CB-FC19A9EEF4C7}");
                public static readonly ID RegistrationTitle = new ID("{43D7FFA7-C35C-45A3-8800-65E9759DD81C}");
                public static readonly ID Subheadline = new ID("{594AE445-2ABF-4D72-9B83-0951BDFB9A97}");
                public static readonly ID TermsConditions = new ID("{E228A4CE-B5A9-4F30-8B4D-C3032A08A37B}");
                public static readonly ID ConfirmationButtonLabel = new ID("{00DB4323-F3FA-485B-866B-3F2349DDB7C1}");
                public static readonly ID CancelButtonLabel = new ID("{D259EDB2-1A5B-4969-8F1D-A8D51EE06C8F}");
                public static readonly ID RegistrationBanner = new ID("{DFD10365-C9A8-4C71-ABC1-D09ADCA11BD2}");
                public static readonly ID MoreQuestionsLink = new ID("{0F7A57A3-4427-4C55-8E3F-AC8D78AF9C5E}");
            }
        }

        public struct ExclusiveOffersCarousel
        {
            public static readonly ID CarouselHeadline = new ID("{1AA835F0-1BA6-45C2-9F73-4193CF1A960C}");
            public static readonly ID CarouselOffers = new ID("{63D2A3E8-8136-40EA-A918-E169A4BEEF88}");

            public struct Fields
            {
                public static readonly ID CardHeadline = new ID("{8E8AD43D-D067-4C4E-BCBF-C92CD4F3C110}");
                public static readonly ID CardColor = new ID("{1AF03C1E-4806-453D-81AF-83A9C673816C}");
                public static readonly ID CardOffer = new ID("{623FF8C6-C882-49A7-9789-03280F9B7E25}");
            }
        }

        public struct Store {
            public struct Fields {
                public static readonly ID BaseReward = new ID("{429902D7-7451-48CB-823F-14D61A7E2B7D}");
                public static readonly ID TotalReward = new ID("{F5619FA7-55E0-40E7-B2E2-F4FB989D3030}");
                public static readonly ID Type = new ID("{338AA4DC-EE49-4A8F-9655-F27E0E96FF64}");
                public static readonly ID Tier = new ID("{DABFF084-D8C5-4512-99FC-3E67785D62A1}");
                public static readonly ID ShoppingUrl = new ID("{4C71C274-0D09-4849-A270-666D5042B276}");
                public static readonly ID Name = new ID("{365E9AE1-60E6-4889-B838-D66BDE087B8D}");
                public static readonly ID Banner = new ID("{B03CA598-21E3-4271-AF93-863F7075BA17}");
                public static readonly ID SmallLogo = new ID("{C57B9FB4-1A4B-4B05-9067-35B6F90B05F8}");
                public static readonly ID Thumbnail = new ID("{3EF4A5AA-C656-4FFB-A782-B9045E5F3234}");
                public static readonly ID Icon11230 = new ID("{0992A3F9-57A4-4156-B800-41C3628B26B6}");
                public static readonly ID Icon22460 = new ID("{83863813-469F-4F8F-A181-124749E946CA}");
                public static readonly ID Icon33690 = new ID("{80B65725-DB71-4515-9794-5AEFE2590F30}");
                public static readonly ID LogoEmail = new ID("{3DDEDB3C-6E63-4892-ABCE-5288D0F26B3A}");
                public static readonly ID LogoMobile = new ID("{B6641DCD-A356-440E-B377-C0FAB3E00BCD}");
                public static readonly ID LogoMobile2x = new ID("{A100AB13-9695-4088-969B-C23118211EC4}");
                public static readonly ID LogoMobile3x = new ID("{AB7D0A77-6EFC-49C8-A3F0-1529712EFC91}");
                public static readonly ID FeedSquareLogo = new ID("{A40FE503-7E00-46D5-9652-5AE9CCDF48A6}");
                public static readonly ID MembersOnlyNEA = new ID("{EE2C4820-7A60-4614-8217-90FA5E9EC46B}");
                public static readonly ID MembersOnlySEIU = new ID("{32A4A597-B46A-4C1F-A6C4-F8A53DB0CB2B}");
                public static readonly ID NeambEnable = new ID("{EA93F5B7-DC52-49CB-A848-A6DFBE79FC8B}");
            }
        }
    }
}