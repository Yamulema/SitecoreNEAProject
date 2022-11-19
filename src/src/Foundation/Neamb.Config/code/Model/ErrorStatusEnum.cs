

using System;

namespace Neambc.Neamb.Foundation.Config.Models
{
    [Flags]
	public enum ErrorStatusEnum
	{
        None = 0,
		Required = 1,
        InvalidCharacters = 2,
        Length = 4,
        AgeRequirement = 8,
        Warning = 16,
        EmailInUse = 32,
        PasswordRequirement = 64,
        PasswordNotEqual = 128,
        MatchNotFound = 256,
        InvalidValue = 512,
        PayoutTotalError = 1024,
        GeneralError = 2048,
		EmailFormat = 4096,
	    MinLength = 8192,
        InvalidDate = 16384,
    }
}