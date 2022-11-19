using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Exceptions
{
    public class NeambWebServiceException : Exception
    {
        public NeambWebServiceException(string message, Exception inner) : base(message, inner) {
        }
    }
}