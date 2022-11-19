using System.Collections.Generic;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Contest.Model;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.Contest.Interfaces
{
    public interface IContestSubmissionService
    {
	    void FillModelContestSubmission(ContestSubmissionDto model);

	    void ExecuteSubmission(ContestSubmissionDto model, ViewDataDictionary viewData,
		    ModelStateDictionary modelState);
        IEnumerable<string> GetAllowedTypes(Item contestPage);
    }
}
