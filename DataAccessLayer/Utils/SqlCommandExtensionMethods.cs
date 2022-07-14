using Entities.Base.Utils.Interface;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Utils
{
    internal static class SqlCommandExtensionMethods
    {
        public static void ConfigureParameters(this SqlCommand cmd, IParametersContainer parameters)
        {
            foreach (var parameter in parameters.GetParameters())
            {
                var sqlParameter = new SqlParameter($"@p_{parameter.Key}", parameter.Value);

                if (sqlParameter.ParameterName == "@p_ID")
                    sqlParameter.Direction = ParameterDirection.InputOutput;

                cmd.Parameters.Add(sqlParameter);
            }
        }
    }
}
