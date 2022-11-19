using System;

namespace Neambc.Neamb.Foundation.Analytics.Interfaces
{
    public interface IGTMLog {
        void Info(object message);
        void Error(object message, Exception t);
        void Debug(object message);
        void Warn(object message);
    }
}