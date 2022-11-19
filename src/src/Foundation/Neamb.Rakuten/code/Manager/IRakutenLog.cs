using System;


namespace Neambc.Neamb.Foundation.Rakuten.Manager
{
    public interface IRakutenLog {
        void Info(object message);
        void Error(object message, Exception t);
        void Debug(object message);
        void Warn(object message);
    }
}