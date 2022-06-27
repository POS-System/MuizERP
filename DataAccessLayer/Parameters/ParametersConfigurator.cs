using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer.Parameters
{
    static class ParametersConfigurator
    {
        public static void ConfigureSqlCommand(SqlCommand sqlCmd, IParametersContainer parametersContainer)
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
