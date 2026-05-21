using reposicion_pin.Models;

namespace reposicion_pin.Repository.Interface
{
    public interface IBandejaEnvioEmailRepository
    {
        Task<IEnumerable<BandejaEnvioEmailModel>> GetAllAsync();
        Task<BandejaEnvioEmailModel?> GetByIdAsync(int id);
        Task AddAsync(BandejaEnvioEmailModel entity);
        Task UpdateAsync(BandejaEnvioEmailModel entity);
        Task DeleteAsync(int id);
    }
}
