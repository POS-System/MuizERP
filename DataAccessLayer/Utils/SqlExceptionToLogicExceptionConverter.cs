using System;
using System.Data.SqlClient;
using Entities.Base.Utils.Converters;
using Entities.Exceptions.InnerApplicationExceptions;

namespace DataAccessLayer.Utils
{
    public sealed class SqlExceptionToLogicExceptionConverter : IConverter<SqlException, Exception>
    {
        public Exception Convert(SqlException sqlException)
        {
            switch (sqlException.Number)
            {
                case -2:   // Client Timeout
                case 701:  // Out of Memory
                case 1204: // Lock Issue 
                case 1205: // >>> Deadlock Victim
                case 1222: // Lock Request Timeout
                case 8645: // Timeout waiting for memory resource 
                case 8651: // Low memory condition 
                    return new SqlSeverIsBusyException(sqlException.Message, sqlException);
                case 50001: // Can't insert record
                    return new SqlServerInsertRecordException(sqlException.Message, sqlException);
                case 50002: // Can't update record
                    return new SqlServerUpdateRecordException(sqlException.Message, sqlException);
                case 50003: // Can't update record
                    return new SqlServerDeleteRecordException(sqlException.Message, sqlException);

                default:
                    return sqlException;
            }
        }
    }
}
