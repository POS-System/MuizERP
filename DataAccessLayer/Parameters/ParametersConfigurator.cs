using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Parameters
{
    static class ParametersConfigurator
    {
        public static void ConfigureSqlCommand(SqlCommand sqlCmd, ParametersContainer parametersContainer)
        {
            var parameters = parametersContainer.GetParameters();

            foreach (var parameter in parameters)
            {
                //sqlCmd.Parameters.AddWithValue($"@p_{parameter.Key}", parameter.Value);

                var sqlParameter = new SqlParameter($"@p_{parameter.Key}", parameter.Value);

                if (sqlParameter.ParameterName == "@p_ID")
                    sqlParameter.Direction = ParameterDirection.InputOutput;

                sqlCmd.Parameters.Add(sqlParameter);
            }
        }
    }
}
