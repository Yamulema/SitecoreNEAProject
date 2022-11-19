using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Model
{
    public class ChangeUsernameDto
    {
        public string description { get; set; }

        public string newUsername { get; set; }

        public string returncode { get; set; }

        public string status { get; set; }
    }
}