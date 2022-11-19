namespace Neambc.Neamb.Foundation.MBCData.Enums
{
    /// <summary>
    /// Status that is returned in response to te User Account Rest API
    /// </summary>
    public enum UserAccountErrorCodesEnum
    {
        TokenAuthFailed = 401,
        MdsIdNotFound = 12001,
        UsernameNotFound = 12002,
        UsernamePasswordCombinationNoMatch = 12003,
        UserLocked = 12004,
        ResetTokenNotFound = 12005,
        InvalidDataRequiredField = 10001,
        InvalidDataValidationField = 10002
    }

    public enum SearchUsernameErrorCodes
    {
        InputDataValidationError = 10002   //  was 10001,
    }
}