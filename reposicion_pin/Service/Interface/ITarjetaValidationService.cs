using reposicion_pin.DTOs;
using reposicion_pin.Models;

namespace reposicion_pin.Service.Interface
{
    public interface ITarjetaValidationService
    {
        Task<ReposicionPinResponseDto> ValidarTarjetaAsync(ReposicionPinRequestDto request);
        Task<TarjetaModel?> ObtenerTarjetaAsync(string cif, string ultimosDigitos);
    }
}
