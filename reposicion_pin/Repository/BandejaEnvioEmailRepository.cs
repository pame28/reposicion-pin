using Microsoft.EntityFrameworkCore;
using reposicion_pin.Infrastructure.Data;
using reposicion_pin.Models;
using reposicion_pin.Repository.Interface;

namespace reposicion_pin.Repository
{
    public class BandejaEnvioEmailRepository : IBandejaEnvioEmailRepository
    {
        private readonly AppDbContext _context;

        public BandejaEnvioEmailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BandejaEnvioEmailModel>> GetAllAsync()
        {
            return await _context.BandejaEnvioEmail.ToListAsync();
        }

        public async Task<BandejaEnvioEmailModel?> GetByIdAsync(int id)
        {
            return await _context.BandejaEnvioEmail.FindAsync(id);
        }

        public async Task AddAsync(BandejaEnvioEmailModel entity)
        {
            await _context.BandejaEnvioEmail.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BandejaEnvioEmailModel entity)
        {
            _context.BandejaEnvioEmail.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.BandejaEnvioEmail.FindAsync(id);
            if (entity != null)
            {
                _context.BandejaEnvioEmail.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
