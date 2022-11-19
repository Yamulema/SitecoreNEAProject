using System;
using log4net;
using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    [Service(typeof(IExactTargetLogger))]
    public class ExactTargetLogger : IExactTargetLogger
    {
        private ILog _log;
        public ExactTargetLogger() {
            _log = LogManager.GetLogger("Sitecore.Diagnostics.ExactTarget");
        }

        public void Debug(object message)
        {
            _log.Debug(message);
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Error(object message, Exception t)
        {
            _log.Error(message, t);
        }

        public void Info(object message)
        {
            _log.Info(message);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }
    }
}