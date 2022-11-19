using System;
using log4net;
using log4net.spi;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Foundation.Analytics.Managers
{
    [Service(typeof(IGTMLog))]
    public class GTMLogger : IGTMLog
    {
        private ILog _log;
        public GTMLogger() {
            _log = LogManager.GetLogger("Sitecore.Diagnostics.GTM");
        }
        
        public ILogger Logger { get; }

        public void Debug(object message) {
            _log.Debug(message);
        }

        public void Debug(object message, Exception t) {
            throw new NotImplementedException();
        }

        public void Info(object message) {
            _log.Info(message);
        }

        public void Info(object message, Exception t) {
            throw new NotImplementedException();
        }

        public void Warn(object message) {
            _log.Warn(message);
        }

        public void Warn(object message, Exception t) {
            _log.Warn(message, t);
        }

        public void Error(object message) {
            _log.Warn(message);
        }

        public void Error(object message, Exception t) {
            _log.Error(message,t);
        }
        public void Fatal(object message) {
            _log.Fatal(message);
        }
        public void Fatal(object message, Exception t) {
            throw new NotImplementedException();
        }
        public bool IsDebugEnabled { get; }
        public bool IsInfoEnabled { get; }
        public bool IsWarnEnabled { get; }
        public bool IsErrorEnabled { get; }
        public bool IsFatalEnabled { get; }
    }
}