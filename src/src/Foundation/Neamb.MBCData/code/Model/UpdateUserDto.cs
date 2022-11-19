using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
    public class UpdateUserDto
    {
        public string description { get; set; }

        public string newmdsid { get; set; }

        public string returncode { get; set; }

        public string status { get; set; }
		public int registrationcount { get; set; }
		public string registrations { get; set; }
    }
}