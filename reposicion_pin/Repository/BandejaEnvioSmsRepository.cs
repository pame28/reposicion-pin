using Microsoft.EntityFrameworkCore;
using reposicion_pin.Infrastructure.Data;
using reposicion_pin.Models;
using reposicion_pin.Repository.Interface;

namespace reposicion_pin.Repository
{
    public class BandejaEnvioSmsRepository : IBandejaEnvioSmsRepository
    {
        private readonly AppDbContext _context;

        public BandejaEnvioSmsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BandejaEnvioSmsModel>> GetAllAsync()
        {
            return await _context.BandejaEnvioSms.ToListAsync();
        }

        public async Task<BandejaEnvioSmsModel?> GetByIdAsync(int id)
        {
            return await _context.BandejaEnvioSms.FindAsync(id);
        }

        public async Task AddAsync(BandejaEnvioSmsModel entity)
        {
            await _context.BandejaEnvioSms.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BandejaEnvioSmsModel entity)
        {
            _context.BandejaEnvioSms.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.BandejaEnvioSms.FindAsync(id);
            if (entity != null)
            {
                _context.BandejaEnvioSms.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
