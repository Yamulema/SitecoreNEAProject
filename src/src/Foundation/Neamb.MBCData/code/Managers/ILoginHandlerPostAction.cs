using System;
using Neambc.Neamb.Foundation.MBCData.Model;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{

    public interface ILoginHandlerPostAction
    {
        bool VerifyExecutionPostAction(string currentRenderingComponentId, string componentIdQueryParameter, LoginHandlerEnum loginHandlerFeature);
        string GetPageToRedirection(string loginPageRedirection);
        void SaveTrackingPageToRedirection(LoginHandlerEnum loginHandlerFeature, string componentIdAuthentication);
    }  
    
}