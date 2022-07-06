using System;
using System.Runtime.Serialization;

namespace Entities.Exceptions.InnerApplicationExceptions
{
    public class SqlServerInsertRecordException : Exception
    {
        public SqlServerInsertRecordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SqlServerInsertRecordException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
