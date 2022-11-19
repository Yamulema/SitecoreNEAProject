using Neambc.Seiumb.Feature.Forms.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Seiumb.Feature.Forms.Utilities
{
    public static class Utilities
    {
        public static string FormatDate(string date)
        {
            return date.Replace("/", string.Empty);
        }

        public static string FormatPhone(string phone)
        {
            return new String(phone.Where(Char.IsDigit).ToArray());
        }

        public static string FormatPermission(bool permission)
        {
            return permission ? ((int)MyAccountProfile.EMAIL_PERMISSION_CHEKED).ToString() : ((int)MyAccountProfile.EMAIL_PERMISSION_UNCHEKED).ToString();
        }
    }
}