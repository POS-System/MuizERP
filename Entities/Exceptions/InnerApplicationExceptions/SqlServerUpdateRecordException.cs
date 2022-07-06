using System;
using System.Runtime.Serialization;

namespace Entities.Exceptions.InnerApplicationExceptions
{
    public class SqlServerUpdateRecordException : Exception
    {
        public SqlServerUpdateRecordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SqlServerUpdateRecordException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
