using System;
using System.Runtime.Serialization;

namespace Entities.Exceptions.InnerApplicationExceptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SqlDataReaderExpectsOnlyOneRecordException : Exception
    {
        public SqlDataReaderExpectsOnlyOneRecordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {}

        public SqlDataReaderExpectsOnlyOneRecordException()
            : base("SqlDataReader ожидает только одну запись.")
        {}
    }
}
