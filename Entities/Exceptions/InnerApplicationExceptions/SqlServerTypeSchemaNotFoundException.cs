using System;
using System.Runtime.Serialization;

namespace Entities.Exceptions.InnerApplicationExceptions
{
    public class SqlServerTypeSchemaNotFoundException : Exception
    {
        public SqlServerTypeSchemaNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SqlServerTypeSchemaNotFoundException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
