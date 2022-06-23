using System;
using System.Runtime.Serialization;

namespace Entities.Exceptions.InnerApplicationExceptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class MappingException : Exception
    {
        public MappingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public MappingException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
