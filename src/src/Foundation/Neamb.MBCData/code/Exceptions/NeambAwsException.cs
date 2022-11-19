using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Exceptions
{
    public class NeambAwsException : Exception
    {
        public NeambAwsException() {

        }
        public NeambAwsException(string message, Exception inner) : base($"[AWS] {message}", inner)
        {
        }
    }
}