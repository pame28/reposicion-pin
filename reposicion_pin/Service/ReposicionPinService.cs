using reposicion_pin.DTOs;
using reposicion_pin.Models;
using reposicion_pin.Service.Interface;
using Microsoft.Extensions.Logging;

namespace reposicion_pin.Service
{
    public class ReposicionPinService
    {
        private readonly ITarjetaValidationService _tarjetaValidationService;
        private readonly IAs400Service _as400Service;
        private readonly INotificacionService _notificacionService;
        private readonly ILogger<ReposicionPinService> _logger;

        public ReposicionPinService(
            ITarjetaValidationService tarjetaValidationService,
            IAs400Service as400Service,
            INotificacionService notificacionService,
            ILogger<ReposicionPinService> logger)
        {
            _tarjetaValidationService = tarjetaValidationService;
            _as400Service = as400Service;
            _notificacionService = notificacionService;
            _logger = logger;
        }

        public async Task<ReposicionPinResponseDto> ReposicionPinAsync(ReposicionPinRequestDto request)
        {
            _logger.LogInformation("[ReposicionPin] Iniciando validación de tarjeta | CIF: {CIF}", request.CIF);

            var validacion = await _tarjetaValidationService.ValidarTarjetaAsync(request);
            if (validacion.CodigoInt != 0)
            {
                _logger.LogWarning("[ReposicionPin] Validación fallida | Codigo: {Codigo} | Descripcion: {Desc}",
                    validacion.CodigoInt, validacion.Descripcion);
                return validacion;
            }

            _logger.LogInformation("[ReposicionPin] Validación OK, obteniendo tarjeta...");
            var tarjeta = await _tarjetaValidationService.ObtenerTarjetaAsync(request.CIF, request.UltimosDigitosTC);
            if (tarjeta == null)
            {
                _logger.LogWarning("[ReposicionPin] Tarjeta no encontrada después de validación exitosa | CIF: {CIF}", request.CIF);
                return validacion;
            }

            var respuestaAs400 = await _as400Service.ConsumirAs400Async(tarjeta, request);


            if (!string.IsNullOrWhiteSpace(respuestaAs400.Pin))
            {
                _logger.LogInformation("[ReposicionPin] Enviando notificación | MetodoEnvio: {Metodo}", request.MetodoEnvio);
                var resultadoNotificacion = await _notificacionService.EnviarNotificacionAsync(tarjeta, request, respuestaAs400.Pin);
                if (resultadoNotificacion.CodigoInt != 0)
                {
                    _logger.LogWarning("[ReposicionPin] Fallo en notificación | Codigo: {Codigo} | Descripcion: {Desc}",
                        resultadoNotificacion.CodigoInt, resultadoNotificacion.Descripcion);
                    return resultadoNotificacion;
                }
                _logger.LogInformation("[ReposicionPin] Notificación enviada correctamente");
            }

            _logger.LogInformation("[ReposicionPin] Proceso completado exitosamente | Codigo: {Codigo}", respuestaAs400.CodigoInt);
            return respuestaAs400;
        }

    }
}