using System;
using System.Runtime.Serialization;

namespace Entities.Exceptions.InnerApplicationExceptions
{
    public class ReflectionPropertyNotFoundException : Exception
    {
        public ReflectionPropertyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ReflectionPropertyNotFoundException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
