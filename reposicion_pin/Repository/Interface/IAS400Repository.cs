using reposicion_pin.DTOs;

namespace reposicion_pin.Repository.Interface
{
    public interface IAS400Repository
    {
        Task<AS400ResponseDto> EjecutarProgramaAsync(AS400RequestDto request);
    }
}
