using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Contest.Model
{
    public class Submission
    {
        public string Id { get; set; }
        public string ContestId { get; set; }
        public ContestFileItem Metadata { get; set; }
        public string ImageSrc { get; set; }
    }
}