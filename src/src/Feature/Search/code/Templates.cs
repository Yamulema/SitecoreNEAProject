using Sitecore.Data;

namespace Neambc.Seiumb.Feature.Search
{
    public static class Templates
    {
        public static readonly ID SearchPageItem = new ID("{62DA7E3A-8909-4D44-94AC-4C8D16DC7FDF}");

        public struct SearchResult
        {
            public static readonly ID ID = new ID("{99D64C92-27C5-4939-A217-B97AFA029B87}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{897A9CA4-4AA2-4D97-81D6-AE45E6FA004A}");
                public static readonly ID NoResultsTitle = new ID("{D14B821C-4609-4CAF-ACEF-DD672D403732}");
                public static readonly ID NoResultsMessage = new ID("{9986C3BB-55D2-4BD0-BFDE-5FF73E7E62EF}");
                public static readonly ID PreviousPage = new ID("{CFAA8C64-DE17-4CB8-81EF-AD4D489E5C8B}");
                public static readonly ID CurrentPage = new ID("{765E8F56-C835-402E-83AC-6CDB5E2F63BB}");
                public static readonly ID NextPage = new ID("{BE5F9439-29CC-46BB-A07B-4297FDDFC768}");
                public static readonly ID Page = new ID("{4280BDDE-737A-41EC-AB8E-B8463008F2B8}");
            }
        }
        public struct SiteSettingsGlobal
        {
            public static readonly ID ID = new ID("{46D06DD7-8928-43F3-AC91-64FFC27778DC}");
        }

        public struct SiteSettings
        {
            public static readonly ID ID = new ID("{BDB7DE45-9794-4B0E-AADE-2D9A8D227FCB}");
            public struct Fields
            {
                public static readonly ID SearchText = new ID("{AE3535A3-87A7-4A45-8146-81901DBCA0C7}");
            }
        }
    }
}