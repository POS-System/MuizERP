using Entities.Base.Utils.Interface;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Utils
{
    internal static class SqlCommandConfigurator
    {
        public static void Configure(SqlCommand sqlCmd, IParametersContainer parametersContainer)
        {
            var parameters = parametersContainer.GetParameters();

            foreach (var parameter in parameters)
            {
                var sqlParameter = new SqlParameter($"@p_{parameter.Key}", parameter.Value);

                if (sqlParameter.ParameterName == "@p_ID")
                    sqlParameter.Direction = ParameterDirection.InputOutput;

                sqlCmd.Parameters.Add(sqlParameter);
            }
        }
    }
}
