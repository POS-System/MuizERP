using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Base.Utils
{
    public sealed class LoggerWrapper : ICustomLogger
    {
        public LoggerWrapper()
        { }

        public void Debug(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Debug(object value)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Error(object value)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal(object value)
        {
            throw new NotImplementedException();
        }

        public void Information(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Information(string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Information(object value)
        {
            throw new NotImplementedException();
        }

        public void Warning(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Warning(object value)
        {
            throw new NotImplementedException();
        }
    }
}
