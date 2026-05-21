using reposicion_pin.DTOs;

namespace reposicion_pin.Service.Interface
{
    public interface IReposicionPinService
    {
        Task<ReposicionPinResponseDto> ProcesarReposicionPinAsync(ReposicionPinRequestDto request);
        Task<ReposicionPinResponseDto?> ValidarMetodoEnvioAsync(ReposicionPinRequestDto request);
        Task<ReposicionPinResponseDto> CrearRespuestaAsync(
            int codigo,
            string? campo = null,
            string? detalle = null);
    }
}
