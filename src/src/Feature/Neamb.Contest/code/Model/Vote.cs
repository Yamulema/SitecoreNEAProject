using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Contest.Model
{
    public class Vote
    {
        public string ContestId { get; set; }
        public string SubmissionId { get; set; }
        public int Total { get; set; }
    }
}