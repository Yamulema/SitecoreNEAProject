using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Exceptions
{
    public class NeambDatabaseException : Exception
    {
        public NeambDatabaseException(string message, Exception inner) : base($"[Oracle] {message}", inner)
        {
        }
    }
}