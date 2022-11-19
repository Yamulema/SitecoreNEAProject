using System;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    public interface IExactTargetLogger
    {
        void Info(object message);
        void Error(object message);
        void Error(object message, Exception t);
        void Debug(object message);
        void Warn(object message);
    }
}
