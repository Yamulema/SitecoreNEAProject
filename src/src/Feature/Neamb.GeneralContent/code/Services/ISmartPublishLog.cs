using System;
namespace Neambc.Neamb.Feature.GeneralContent.Services
{

    public interface ISmartPublishLog
    {
        void Info(object message);
        void Error(object message, Exception t);
        void Debug(object message);
        void Warn(object message);
    }
}