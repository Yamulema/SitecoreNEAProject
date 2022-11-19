using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.Contest.Model
{
    public class UserVote
    {
        public string ContestId { get; set; }
        public string SubmissionId { get; set; }
    }
}