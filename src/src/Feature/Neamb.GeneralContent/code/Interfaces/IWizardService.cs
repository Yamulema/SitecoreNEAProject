using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Feature.GeneralContent.Models;
using Sitecore.Data.Items;

namespace Neambc.Neamb.Feature.GeneralContent.Interfaces
{
    public interface IWizardService {
        Wizard GetWizard(Item datasource, HttpRequestBase request);
        Step GetStep(Item datasource);
        string GetNextStepUrl();
    }
}