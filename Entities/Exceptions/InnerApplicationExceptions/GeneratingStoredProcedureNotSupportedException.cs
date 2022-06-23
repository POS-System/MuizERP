using System;
using System.Runtime.Serialization;

namespace Entities.Exceptions.InnerApplicationExceptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GeneratingStoredProcedureNotSupportedException : Exception
    {
        public GeneratingStoredProcedureNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public GeneratingStoredProcedureNotSupportedException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
