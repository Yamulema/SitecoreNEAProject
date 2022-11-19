using System;
using Neambc.Neamb.Foundation.Cache.Managers;
using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    [Service(typeof(IMdsLoggingManager))]
    public class MdsLoggingManager: IMdsLoggingManager
    {
        private readonly IOracleDatabase _oracleManager;
        private readonly ISessionManager _sessionManager;

        public MdsLoggingManager(IOracleDatabase oracleManager, ISessionManager sessionManager) {
            _oracleManager = oracleManager;
            _sessionManager = sessionManager;

        }
        public bool ExecuteMdsLoggingProcessInner(string productCode, string type, string materialId, string mdsId, string cellCode)
        {
            Sitecore.Diagnostics.Log.Debug("Starting ExecuteStoredProcedureOrder", this);
            var resultExecution = false;
            if (productCode.Length == 6 || !string.IsNullOrEmpty(materialId))
            {
                var codeView = string.IsNullOrEmpty(materialId)
                    ? $"{productCode.Substring(0, 3)}{type}{productCode.Substring(4, 2)}"
                    : materialId;
                var message = $"Executing data stored procedure mdsid:{mdsId} codeView:{codeView} cellCode:{cellCode}";
                Sitecore.Diagnostics.Log.Debug(message, this);

                resultExecution = _oracleManager.SelectOrderFulfill(Convert.ToInt32(mdsId), codeView,
                    cellCode,
                    "WEB-MB");
            }
            else
            {
                Sitecore.Diagnostics.Log.Debug(this + "The program code doesn't have the correct format ", this);
            }

            Sitecore.Diagnostics.Log.Debug("Ending ExecuteStoredProcedureOrder", this);
            return resultExecution;
        }
        public void ExecuteMdsLoggingProcess(
            string productCode,
            string nameForSession,
            string typeProcess,
            string mdsId,
            string cellCode,
            string materialId
        )
        {
            if (!string.IsNullOrEmpty(mdsId))
            {
                var keySession = !string.IsNullOrEmpty(productCode)
                    ? $"{nameForSession}{productCode}{mdsId}"
                    : $"{nameForSession}{materialId}{mdsId}";
                var valueSession = _sessionManager.RetrieveFromSession<string>(keySession);
                if (string.IsNullOrEmpty(valueSession))
                {
                    var resultExecution = ExecuteMdsLoggingProcessInner(productCode, typeProcess, materialId, mdsId, cellCode);
                    if (resultExecution)
                    {
                        var valueStore = !string.IsNullOrEmpty(productCode) ? productCode : materialId;
                        _sessionManager.StoreInSession(keySession, valueStore);
                    }
                } else {
                    Sitecore.Diagnostics.Log.Debug($"No Executing Mds Logging {productCode} {nameForSession} {typeProcess} {mdsId}", this);
                }
            }
        }
    }
}