using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Neambc.Neamb.Feature.GeneralContent.Interfaces
{
    public interface IComplimentaryLifeWizardService : IWizardEventHandler
    {
        string GetBeneficiariesAddCta();
        string GetParentStepUrl();
        bool IsWizardStep();
        string GetBeneficiariesEditCta();
    }
}