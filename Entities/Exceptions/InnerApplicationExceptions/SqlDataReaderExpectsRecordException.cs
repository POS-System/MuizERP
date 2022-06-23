using System;
using System.Runtime.Serialization;

namespace Entities.Exceptions.InnerApplicationExceptions
{
    [Serializable]
    public class SqlDataReaderExpectsRecordException : Exception
    {
        public SqlDataReaderExpectsRecordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {}

        public SqlDataReaderExpectsRecordException()
            : base("SqlDataReader ожидает хотябы одну запись.")
        {}
    }
}