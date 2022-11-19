using System;

namespace Neambc.Neamb.Foundation.MBCData.Enums
{
    [Flags]
    public enum S3ObjectTypeFilter
    {
        None = 0,
        File = 1,
        Folder = 2
    }
}