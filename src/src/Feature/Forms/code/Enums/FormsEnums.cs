using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Forms.Enums
{
    public enum RegistrationErrors
    {
        INVALID_USERNAME,
        PASSWORD_MISSMATCH,
        USER_ALREADY_EXIST,
        USERNAME_NOT_AVAILABLE,
        INVALID_CHARACTERS,
        FIRST_NAME_INVALID_CHARACTARES,
        LAST_NAME_INVALID_CHARACTARES,
        ADDRESS_INVALID_CHARACTARES,
        CITY_INVALID_CHARACTARES,
    }    

    public enum ZipCodeValidationErrors
    {
        WRONG_ZIPCODE,
        INVALID_ZIPCODE
    }

    public enum ProfileErrors
    {
        VALUES_REQUIRED,
        PASSWORD_DONT_MATCH,
        USERNAME_NOT_AVAILABLE,
        INVALID_USERNAME
    }

    public enum ChangeUsernameErrors
    {
        INPUT_DATA_INVALID = 10002, //was 10001
        PROVIDE_USERNAME_DOES_NOT_MATCH = 10003
    }

    public enum MyAccountProfile
    {
        EMAIL_PERMISSION_CHEKED=5,
        EMAIL_PERMISSION_UNCHEKED=4
    }
}