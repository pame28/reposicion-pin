using Microsoft.Data.SqlClient;
using reposicion_pin.Repository.Interface;
using System.Data;

namespace reposicion_pin.Repository
{
    public class ConfiguracionAs400Repository : IConfiguracionAs400Repository
    {
        private readonly string _sqlConnectionString;

        public ConfiguracionAs400Repository(IConfiguration configuration)
        {
            _sqlConnectionString = configuration.GetConnectionString("ReposicionPinConnection")!;
        }

        public async Task<(string call, List<string> librerias)> ObtenerConfiguracionAsync(int idCall)
        {
            var librerias = new List<string>();
            string call = string.Empty;

            using var sqlConn = new SqlConnection(_sqlConnectionString);
            await sqlConn.OpenAsync();

            using var cmd = new SqlCommand("usp_configCallAs400", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idCallAS400", idCall);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                call = reader["CallAs400"].ToString() ?? "";
            }

            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    librerias.Add(reader["Libreria"].ToString() ?? "");
                }
            }

            return (call, librerias);
        }

    }
}
