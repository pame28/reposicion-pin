using Microsoft.EntityFrameworkCore;
using reposicion_pin.Infrastructure.Data;
using reposicion_pin.Repository.Interface;

namespace reposicion_pin.Repository
{
    public class CatalogoRespuestaRepository : ICatalogoRespuestaRepository
    {
        private readonly AppDbContext _context;

        public CatalogoRespuestaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string?> ObtenerDescripcionAsync(int codigo)
        {
            var catalogo = await _context.CatalogoRespuestas
                .Where(c=> c.Codigo == codigo)
                .Select(c=>c.Descripcion)
                .FirstOrDefaultAsync();
            return catalogo;
        }
    }
}
