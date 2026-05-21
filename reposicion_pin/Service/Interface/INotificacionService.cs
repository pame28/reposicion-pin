using reposicion_pin.DTOs;
using reposicion_pin.Models;

namespace reposicion_pin.Service.Interface
{
    public interface INotificacionService
    {
        Task<ReposicionPinResponseDto> EnviarNotificacionAsync(TarjetaModel tarjeta, ReposicionPinRequestDto request, string pin);
    }
}
