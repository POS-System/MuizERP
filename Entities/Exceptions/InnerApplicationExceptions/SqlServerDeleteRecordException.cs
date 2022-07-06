using System;
using System.Runtime.Serialization;

namespace Entities.Exceptions.InnerApplicationExceptions
{
    public class SqlServerDeleteRecordException : Exception
    {
        public SqlServerDeleteRecordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SqlServerDeleteRecordException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
