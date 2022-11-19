using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
    public class ValidateEmailDomainDto
    {
        public string Description { get; set; }
        public string ReturnCode { get; set; }
        public string Status { get; set; }
        public string Valid { get; set; }
    }
}