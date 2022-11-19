using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Language
{
    public struct Templates
    {
        public struct Home
        {
            public static readonly ID ID = new ID("{545409FC-DB86-4A7F-AC61-F74A274B5E30}");
        }

        public struct SpanishLanguageModal
        {
            public static readonly ID ID = new ID("{5682D99D-FB1F-4027-9712-7DD67F6C070A}");
            public struct Fields
            {
                public static readonly ID Content = new ID("{AF18D301-47D2-4A63-93C9-EDBC24B468DC}");
            }
        }
        public struct LanguageToggle
        {
            public static readonly ID ID = new ID("{CB80D522-886F-47C0-B740-D029FD4DB5A8}");
            public struct Fields
            {
                public static readonly ID English = new ID("{079268F2-EFC7-4F96-BC9D-1D998C536553}");
                public static readonly ID Spanish = new ID("{BA1ADA05-6F08-4FC9-AE96-87E8555600EA}");
                public static readonly ID Default = new ID("{8DCF8F52-E74E-46CE-B897-DFE19D9D4E11}");
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
    }
}