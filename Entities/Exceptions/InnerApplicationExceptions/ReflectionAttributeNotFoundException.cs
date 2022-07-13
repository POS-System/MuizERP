using System;
using System.Runtime.Serialization;

namespace Entities.Exceptions.InnerApplicationExceptions
{
    public class ReflectionAttributeNotFoundException : Exception
    {
        public ReflectionAttributeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ReflectionAttributeNotFoundException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
