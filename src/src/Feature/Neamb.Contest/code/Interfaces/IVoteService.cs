using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neambc.Neamb.Feature.Contest.Model;

namespace Neambc.Neamb.Feature.Contest.Interfaces
{
    public interface IVoteService
    {
        void PerformIntegritySync(string contestId);
        void AddUserVote(UserVote userVote);
        UserVote GetUserVote(string contestId);
        IList<Vote> GetVotes(string contestId);
        Vote GetVote(string contestId, string submissionId);
        IEnumerable<Submission> GetSubmissions(string contestId, IEnumerable<string> allowedTypes);
	    bool ExecuteContestLoggingProcess(string itemcode, string mdsid);
    }
}
