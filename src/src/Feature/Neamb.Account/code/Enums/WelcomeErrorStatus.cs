using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Account.Enums
{
    [Flags]
    public enum WelcomeErrorStatus
    {
        None = 0,
        InvalidCode = 1,
        GeneralError = 2,
        InvalidPassword = 4,
        InvalidZip = 8,
        ZipMatchNotFound = 16,
        Duplicated = 32,
        TooManyAttempts = 64,
        AccountLocked = 128,
        AccountAlreadyLockedValidToken = 256,
        TimeOut = 512,
    }
}