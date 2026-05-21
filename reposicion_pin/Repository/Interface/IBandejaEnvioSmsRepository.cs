using reposicion_pin.Models;

namespace reposicion_pin.Repository.Interface
{
    public interface IBandejaEnvioSmsRepository
    {
        Task<IEnumerable<BandejaEnvioSmsModel>> GetAllAsync();
        Task<BandejaEnvioSmsModel?> GetByIdAsync(int id);
        Task AddAsync(BandejaEnvioSmsModel entity);
        Task UpdateAsync(BandejaEnvioSmsModel entity);
        Task DeleteAsync(int id);
    }
}
