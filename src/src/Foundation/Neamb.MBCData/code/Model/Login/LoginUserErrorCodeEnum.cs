
namespace Neambc.Neamb.Foundation.MBCData.Model.Login
{
    public enum LoginUserErrorCodeEnum
    {
        None=0,
        FailedLogin=12003,
        ErrorToken=401,
        AccountLocked=12004,
        UsernameNotExist = 12002,
        InvalidDataRequiredField = 10001,
        InvalidDataValidationField = 10002,
        ModelError = 10003
    }
}