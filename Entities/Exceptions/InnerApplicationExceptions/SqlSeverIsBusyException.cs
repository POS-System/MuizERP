using System;
using System.Runtime.Serialization;

namespace Entities.Exceptions.InnerApplicationExceptions
{
    [Serializable]
    public class SqlSeverIsBusyException : Exception
    {
        public SqlSeverIsBusyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SqlSeverIsBusyException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
