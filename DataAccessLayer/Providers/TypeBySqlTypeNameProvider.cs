using Entities.Base.Utils.Providers;
using System;

namespace DataAccessLayer.Providers
{
    internal sealed class TypeBySqlTypeNameProvider : IKeyedProvider<string, Type>
    {
        public Type GetByValue(string sqlTypeName)
        {
            switch (sqlTypeName)
            {
                case "date":
                case "datetime":
                    return typeof(DateTime);
                case "bit":
                    return typeof(bool);
                case "tinyint":
                    return typeof(byte);
                case "int":
                    return typeof(int);
                case "bigint":
                    return typeof(long);
                case "varbinary":
                case "timestamp":
                    return typeof(byte[]);
                case "money":
                    return typeof(decimal);
                case "char":
                case "nchar":
                    return typeof(char);
                case "varchar":
                case "nvarchar":
                    return typeof(string);
                default:
                    return typeof(object);
            }
        }
    }
}
