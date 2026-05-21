using Dapper;
using Microsoft.Data.SqlClient;
using reposicion_pin.Repository.Interface;
using System.Data;

namespace reposicion_pin.Repository
{
    public class OperadorRepository:IOperadorRepository
    {
        private readonly string _connectionString;

        public OperadorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ReposicionPinConnection")!;
        }

        public async Task<bool> ExisteOperadorAsync(string operador)
        {
            using var conn = new SqlConnection(_connectionString);

            var result = await conn.QueryFirstOrDefaultAsync<int>(
                "usp_OperadoresT",
                new { idOperador = operador },
                commandType: CommandType.StoredProcedure);

            return result > 0;
        }
    }
}
