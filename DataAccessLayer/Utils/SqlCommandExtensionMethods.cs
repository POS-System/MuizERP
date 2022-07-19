using Entities.Base;
using Entities.Base.Attributes;
using Entities.Base.Utils.Interface;
using System.Data;
using System.Data.SqlClient;
using Entities.Base.Utils;

namespace DataAccessLayer.Utils
{
    internal static class SqlCommandExtensionMethods
    {
        public static void AddParameters(this SqlCommand cmd, IParametersContainer parameters)
        {
            foreach (var parameter in parameters.GetParameters())
            {
                var sqlParameter = new SqlParameter($"@p_{parameter.Key}", parameter.Value);

                if (sqlParameter.ParameterName == "@p_ID")
                    sqlParameter.Direction = ParameterDirection.InputOutput;

                cmd.Parameters.Add(sqlParameter);
            }
        }

        public static void AddIdentifier(this SqlCommand cmd, int id)
        {
            var sqlParameter = new SqlParameter($"@p_ID", id);
            sqlParameter.Direction = ParameterDirection.InputOutput;

            cmd.Parameters.Add(sqlParameter);
        }

        public static void AddForignKey<T>(this SqlCommand cmd, int id)
        {
            var sqlParameter = new SqlParameter($"@p_{typeof(T).Name}ID", id);

            cmd.Parameters.Add(sqlParameter);
        }
    }
}
