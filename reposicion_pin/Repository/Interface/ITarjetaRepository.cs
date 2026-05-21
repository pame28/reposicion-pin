using reposicion_pin.DTOs;
using reposicion_pin.Models;

namespace reposicion_pin.Repository.Interface
{
    public interface ITarjetaRepository
    {
        Task<TarjetaModel?> ObtenerTarjetaAsync(string cif, string ultimosDigitos);
    }
}
