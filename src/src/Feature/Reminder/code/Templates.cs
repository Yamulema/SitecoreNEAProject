using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Reminder
{
    public struct Templates
    {
        public struct Reminder
        {
            public static readonly ID ID = new ID("{70409F72-22A9-47A4-9B58-9E4A3FB519E6}");

            public struct Fields
            {
                public static readonly ID Code = new ID("{5D584A48-FBF4-4AAD-A42F-741654216AE1}");
                public static readonly ID Body = new ID("{44D73723-D751-4FB9-AF2C-EA2572E8572E}");
                public static readonly ID BottomText = new ID("{7AD6C79D-76CE-4A84-9B37-DB20D2834724}");
                public static readonly ID Title = new ID("{45F31DD5-D932-47FD-AAE3-C73CBBA72432}");
                public static readonly ID Image = new ID("{F17D590A-476C-41C3-AB86-619F8ECC01BE}");
                public static readonly ID AnonimousUser = new ID("{6317A76E-AEF3-4C77-856F-B00386ABD743}");
                public static readonly ID AuthenticatedUser = new ID("{5C20CD1C-BC2F-4D08-B91B-101C530EEB55}");
                public static readonly ID EnrolledUser = new ID("{560C0CFC-979B-4D06-A0A7-9CB2FB1B5B0A}");
            }
        }
    }
}