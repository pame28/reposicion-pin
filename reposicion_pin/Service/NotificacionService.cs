using reposicion_pin.DTOs;
using reposicion_pin.Models;
using reposicion_pin.Repository.Interface;
using reposicion_pin.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace reposicion_pin.Service
{
    public class NotificacionService : INotificacionService
    {
        private readonly IEnvioSmsRepository _envioSmsRepository;
        private readonly IBandejaEnvioSmsRepository _bandejaSmsRepository;
        private readonly IEnvioEmailRepository _envioEmailRepository;
        private readonly IBandejaEnvioEmailRepository _bandejaEmailRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<NotificacionService> _logger;
        private readonly ICatalogoRespuestaRepository _catalogoRepository; 

        public NotificacionService(
            IEnvioSmsRepository envioSmsRepository,
            IBandejaEnvioSmsRepository bandejaSmsRepository,
            IEnvioEmailRepository envioEmailRepository,
            IBandejaEnvioEmailRepository bandejaEmailRepository,
            IConfiguration configuration,
            ICatalogoRespuestaRepository catalogoRepository, 
            ILogger<NotificacionService> logger)
        {
            _envioSmsRepository = envioSmsRepository;
            _bandejaSmsRepository = bandejaSmsRepository;
            _envioEmailRepository = envioEmailRepository;
            _bandejaEmailRepository = bandejaEmailRepository;
            _configuration = configuration;
            _catalogoRepository = catalogoRepository;
            _logger = logger;
        }

        public async Task<ReposicionPinResponseDto> EnviarNotificacionAsync(TarjetaModel tarjeta, ReposicionPinRequestDto request, string pin)
        {
            try
            {
                if (request.MetodoEnvio == 1)
                    return await EnviarSmsAsync(tarjeta, request, pin);
                else if(request.MetodoEnvio == 2)
                    return await EnviarEmailAsync(tarjeta, request, pin);
                else
                    return new ReposicionPinResponseDto { CodigoInt = 10, Descripcion = "Error en validación de campo"};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en EnviarNotificacionAsync");
                return new ReposicionPinResponseDto { CodigoInt = 99, Descripcion = ex.Message };
            }
        }

        #region Métodos privados
        private async Task<ReposicionPinResponseDto> EnviarSmsAsync(TarjetaModel tarjeta, ReposicionPinRequestDto request, string pin)
        {
            long numeroTransaccion = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string referencia = numeroTransaccion.ToString();

            var resultado = await _envioSmsRepository.EnviarSmsDirectoAsync(
                request.Telefono,
                request.OperadorTelefono,
              //  request.ContenidoSMS,
                pin,
                referencia,
                CancellationToken.None
            );

            await InsertarRegistroEnvioSmsAsync(tarjeta, request, referencia, resultado.Codigo, resultado.Mensaje, resultado.SmsEnviado);

            if (resultado.Codigo != 0)
            {
                var respuesta = await ObtenerRespuestaCatalogoAsync(7); // 7 = Error BiMovil
                respuesta.Descripcion = $"{respuesta.Descripcion} {resultado.Mensaje}";
                return respuesta;
            }

            return new ReposicionPinResponseDto { CodigoInt = 0, Descripcion = "Exitoso" };
        }

        private async Task<ReposicionPinResponseDto> EnviarEmailAsync(TarjetaModel tarjeta, ReposicionPinRequestDto request, string pin)
        {
            long numeroTransaccion = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string referencia = numeroTransaccion.ToString();

            var resultado = await _envioEmailRepository.EnviarCorreoAsync(
                request.Correo,
              //  request.ContenidoCorreo,
                pin,
                referencia,
                CancellationToken.None
            );

            await InsertarRegistroEnvioEmailAsync(
                tarjeta,
                request,
                referencia,
                resultado.Codigo,
                resultado.Mensaje,
                resultado.MailFrom,
                resultado.MailSubject,
                resultado.CuerpoCorreo
            );

            if (resultado.Codigo != 0)
            {
                var respuesta = await ObtenerRespuestaCatalogoAsync(8); // 8 = Error Email
                respuesta.Descripcion = $"{respuesta.Descripcion} {resultado.Mensaje}";
                return respuesta;
            }

            return new ReposicionPinResponseDto { CodigoInt = 0, Descripcion = "Exitoso" };
        }

        private async Task InsertarRegistroEnvioSmsAsync(TarjetaModel tarjeta, ReposicionPinRequestDto request, string referencia, int codigo, string mensaje, string SmsEnviado)
        {
            try
            {
                var entidad = new BandejaEnvioSmsModel
                {
                    NumeroTarjeta = tarjeta.NumeroTarjeta,
                    Celular = request.Telefono,
                    Telco = request.OperadorTelefono,
                    Sms = SmsEnviado,
                    Referencia = referencia,
                    FechaInicio = DateTime.UtcNow,
                    FechaFinal = DateTime.UtcNow,
                    Estado = codigo,
                    DescripcionEstado = mensaje
                };

                await _bandejaSmsRepository.AddAsync(entidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando resultado SMS");
            }
        }

        private async Task InsertarRegistroEnvioEmailAsync(
            TarjetaModel tarjeta,
            ReposicionPinRequestDto request,
            string referencia,
            int codigo,
            string mensaje,
            string mailFrom,
            string mailSubject,
            string cuerpoCorreo)
        {
            try
            {
                var entidad = new BandejaEnvioEmailModel
                {
                    NumeroTarjeta = tarjeta.NumeroTarjeta,
                    CuerpoCorreo = cuerpoCorreo,
                    MailFrom = mailFrom,
                    MailSubject = mailSubject,
                    Correo = request.Correo,
                    Referencia = referencia,
                    FechaInicio = DateTime.UtcNow,
                    FechaFinal = DateTime.UtcNow,
                    Estado = codigo,
                    DescripcionEstado = mensaje
                };

                await _bandejaEmailRepository.AddAsync(entidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando resultado Email");
            }
        }

        private async Task<ReposicionPinResponseDto> ObtenerRespuestaCatalogoAsync(int codigo)
        {
            string? descripcion = null;
            try
            {
                var catalogoDescripcion = await _catalogoRepository.ObtenerDescripcionAsync(codigo);
                if (!string.IsNullOrWhiteSpace(catalogoDescripcion))
                    descripcion = catalogoDescripcion;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"No se pudo obtener descripción del catálogo para código {codigo}");
            }

            return new ReposicionPinResponseDto
            {
                CodigoInt = codigo,
                Descripcion = descripcion
            };
        }
        #endregion
    }
}