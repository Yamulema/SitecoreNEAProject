using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Foundation.Authentication.Enums
{
    public enum LoginErrors
    {
        EMPTY_FIELDS = 999,
        INVALID_USERNAME_PASSWORD = 3,
        //ALREADY_SENT_EMAIL_RESET_PWD = 2,
        ACCOUNT_LOCKED_NOT_SENT_MAIL = 2,
        ACCOUNT_LOCKED_SENT_MAIL = 4,
        //SENT_EMAIL_RESET_PWD = 4,
        USERNAME_DOES_NOT_EXIST = 1,
        ERROR = -1,
        INVALID_DATA = 10001,
        ALREADY_REGISTERED,
		TIME_OUT,
        MODEL_ERROR,
        NONE = 0
	}

    public enum AuthenticatePasswordErrors
    {
        USERNAME_PASSWORD_NOT_MATCH = 3,
        USERNAME_DOES_NOT_EXIST = 1,
        VALIDATE_ERROR = 10001,
        PARTNER_ERROR = 10000,
        ERROR = -1
    }
}