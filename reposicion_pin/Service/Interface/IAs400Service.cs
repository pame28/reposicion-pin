using reposicion_pin.DTOs;
using reposicion_pin.Models;

namespace reposicion_pin.Service.Interface
{
    public interface IAs400Service
    {
        Task<ReposicionPinResponseDto> ConsumirAs400Async(TarjetaModel tarjeta, ReposicionPinRequestDto request);
    }
}
