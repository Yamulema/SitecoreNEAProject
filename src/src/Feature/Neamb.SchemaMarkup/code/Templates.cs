using Sitecore.Data;


namespace Neambc.Neamb.Feature.SchemaMarkup
{
    public static class Templates
    {
        public struct Articles
        {
            public static ID Id = new ID("{15547760-E485-446C-B7C0-98A660FD577E}");

            public struct Fields
            {
                public static ID HeroImage = new ID("{ADABEDE9-5073-4B40-BD3B-129D451B160B}");
                public static ID PageTitle = new ID("{F71F7747-F88D-499B-AC69-D3A6DC9B0A88}");
                public static ID Author = new ID("{0CCD3F73-1942-4EEC-8383-884507E75041}");
                public static ID AuthorUrl = new ID("{A1EFA66F-4F68-453D-BF0F-FBED7AAB3685}");
                public static ID PublishDate = new ID("{D7DF4284-50D1-4F7E-BF50-4D038B6C6E58}");
                public static ID LastUpdatedDate = new ID("{F0A62EF6-4CB8-4AB8-A239-591D2D0658F3}");
            }
        }

        public struct LandingPage
        {
            public static ID Id = new ID("{E34BACDF-686B-4D8A-8FD1-D6A2E52EA1D9}");
            public static ID FAQPageId = new ID("{8FE4B8BF-A2DC-4B19-978E-8343BFF1A19A}");
        }

        public struct Product
        {
            public static ID Id = new ID("{D1889EB8-BE95-4E99-B8E9-3A0AEB8F4800}");

            public struct Fields
            {
                public static ID EyeBrow = new ID("{27E8BA31-6953-411D-9F7E-E578D38A74EF}");
            }
        }

        public struct ProductCategory
        {
            public static ID Id = new ID("{EEA0A232-C12B-4422-99C4-216A4E16FCDF}");
        }

        public struct MarketPlace
        {
            public static ID Id = new ID("{75CC98A4-6E6C-481B-ADE6-CB3C3941AD91}");
        }

        public struct GoalTopic
        {
            public static ID Id = new ID("{71F83DE6-AA9C-4F1D-9FCC-C04B2EF2832D}");
        }

        public struct SmallAccordionItem
        {
            public static ID Id = new ID("{61C93F47-3975-4AEC-AFB4-86CBDEFF631A}");

            public struct Fields
            {
                public static ID Header = new ID("{2CCDA41E-0525-4EF5-9A02-0A9C87D2075D}");
                public static ID Subhead = new ID("{9F8E9C78-17BB-4EB3-931C-5F326A8182A3}");
            }
        }

        public struct Folder
        {
            public static ID Id = new ID("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");
        }

        public struct LogoNeamb
        {
            public static ID Id = new ID("{ABDA9E9C-218D-416A-8E84-6B6C86F44F09}");
        }

        public struct CommonFields
        {
            public static ID MetaTitle = new ID("{A3258719-614E-4F9E-BCE6-DFF1E9E66226}");
        }
    }
}