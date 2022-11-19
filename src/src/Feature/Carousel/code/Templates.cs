using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Carousel
{
    public class Templates
    {
        public struct Carousel
        {
            public static readonly ID ID = new ID("{6AA9915B-CEBF-476C-B13F-8D4E19A188A6}");
            public struct Fields
            {
                public static readonly ID Time = new ID("{CA6CF9B1-96A3-4512-96FE-BA565CB2A244}");
                public static readonly ID Slides = new ID("{86329FAD-3D3D-4D3C-B5E1-EA28C8D485E1}");
            }
        }

        public struct CarouselSlide
        {
            public static readonly ID ID = new ID("{CC9636F1-0C30-4B53-871D-1E2712598158}");
            public struct Fields
            {
                public static readonly ID Headline = new ID("{CBBAE54A-B5DF-4630-B803-9C941F1C0388}");
                public static readonly ID Subheadline = new ID("{11929B80-2748-4662-9529-FB2D7B3B6247}");
                public static readonly ID ReadMore = new ID("{1AB01646-FA21-4F15-9AB9-6B50DA9E76C7}");
                public static readonly ID Image = new ID("{CCA0B4A8-BE58-4ED7-A6FB-A6948FFEA43C}");
            }
        }
    }
}