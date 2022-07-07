using System;

namespace Entities.Base.Utils
{
    public interface ICustomLogger
    {
        void Debug(Exception exception);
        void Debug(string message, Exception exception);
        void Debug(object value);
        void Error(Exception exception);
        void Error(string message, Exception exception);
        void Error(object value);
        void Fatal(Exception exception);
        void Fatal(string message, Exception exception);
        void Fatal(object value);
        void Information(Exception exception);
        void Information(string message, Exception exception);
        void Information(object value);
        void Warning(Exception exception);
        void Warning(string message, Exception exception);
        void Warning(object value);
    }
}
