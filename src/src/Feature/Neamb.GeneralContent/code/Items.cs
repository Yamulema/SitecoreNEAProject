using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.GeneralContent
{
    public struct Items
    {
        public static readonly ID StateAffiliate = new ID("{C833087B-842E-4C7A-9825-D7E74690D98E}");
        public static readonly ID ContactUsTopic = new ID("{2E2CF3E4-181D-4717-A8E8-3D09FF1F954C}");
        public static readonly ID ContactUsTopicEs = new ID("{B5637AB0-41F6-4AB5-98B1-1EE4118F7EA9}");
        public static readonly ID EnglishLanguage = new ID("{BDDB4716-B807-4A40-BD12-837942A63D82}");
        public static readonly ID SpanishLanguage = new ID("{CAF9B31B-9D5F-409D-A51E-E6AD12F51D83}");
        public static readonly ID SiteSettings = new ID("{C7EADD3C-19BC-463B-B0CC-A862E99E5B50}");
    }
    public struct Categories
    {
        public struct MediaPlacements
        {
            public static readonly ID Left = new ID("{21700213-AFC0-4DCA-9507-EF3523887CB4}");
            public static readonly ID Right = new ID("{C495553A-3FDB-4AE5-BDD9-CBE44645A3BD}");
        }
    }
}