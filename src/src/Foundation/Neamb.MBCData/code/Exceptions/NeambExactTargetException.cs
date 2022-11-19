using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Exceptions
{
    public class NeambExactTargetException : Exception
    {
        public NeambExactTargetException()
        {

        }
        public NeambExactTargetException(string message, Exception inner) : base($"[ExactTarget] {message}", inner)
        {
        }
    }
}