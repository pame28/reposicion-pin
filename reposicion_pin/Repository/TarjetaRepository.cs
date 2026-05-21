using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using reposicion_pin.DTOs;
using reposicion_pin.Infrastructure.Data;
using reposicion_pin.Models;
using reposicion_pin.Repository.Interface;
using System.Data;

namespace reposicion_pin.Repository
{
    public class TarjetaRepository : ITarjetaRepository
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        public TarjetaRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ReposicionPinConnection")!;
        }

        public async Task<TarjetaModel?> ObtenerTarjetaAsync(string cif, string ultimosDigitos)
        {

            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@CIF", cif, DbType.String);
            parameters.Add("@UltimosDigitos", ultimosDigitos, DbType.String);

            var tarjeta = await connection.QueryFirstOrDefaultAsync<TarjetaModel>(
                "dbo.usp_ObtenerTarjetaPorCifYUltimosDigitos",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return tarjeta;
        }

    }
}