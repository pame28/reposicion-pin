using reposicion_pin.DTOs;
using reposicion_pin.Models;
using reposicion_pin.Repository.Interface;
using reposicion_pin.Service.Interface;
using Microsoft.Extensions.Logging;

namespace reposicion_pin.Service
{
    public class As400Service : IAs400Service
    {
        private readonly IAS400Repository _as400Repository;
        private readonly ICatalogoRespuestaRepository _catalogoRepository;
        private readonly ILogger<As400Service> _logger;

        public As400Service(
            IAS400Repository as400Repository,
            ICatalogoRespuestaRepository catalogoRepository,
            ILogger<As400Service> logger)
        {
            _as400Repository = as400Repository;
            _catalogoRepository = catalogoRepository;
            _logger = logger;
        }

        public async Task<ReposicionPinResponseDto> ConsumirAs400Async(TarjetaModel tarjeta, ReposicionPinRequestDto request)
        {
            try
            {
                var as400Request = new AS400RequestDto
                {
                    NumeroTarjeta = tarjeta.NumeroTarjeta.PadLeft(16, ' '),
                    FechaVencimiento = tarjeta.FechaFinVigencia.ToString("yyyyMMdd")
                };

                var as400Response = await _as400Repository.EjecutarProgramaAsync(as400Request);

                var codigoAs400 = as400Response.CodigoError?.Trim();

                if (!string.IsNullOrWhiteSpace(codigoAs400) && codigoAs400 != "00")
                    return await MapearErrorAs400Async(as400Response);

                var respuesta = await ObtenerRespuestaAsync(0);
                respuesta.Pin = as400Response.Pin;
                return respuesta;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al consumir AS400");
                return await ObtenerRespuestaDinamicaAsync(99, "Excepción", ex.Message);
            }
        }

        #region Helpers
        private async Task<ReposicionPinResponseDto> MapearErrorAs400Async(AS400ResponseDto as400)
        {
            var respuesta = await ObtenerRespuestaAsync(9);
            string codigoAs400 = as400.CodigoError?.Trim() ?? "";
            string descripcionAs400 = as400.DescripcionError?.Trim() ?? "";

            respuesta.Descripcion = $"{respuesta.Descripcion} {codigoAs400} {descripcionAs400}".Trim();
            respuesta.Pin = null;
            return respuesta;
        }

        private async Task<ReposicionPinResponseDto> ObtenerRespuestaAsync(int codigo, string? fallbackDescripcion = null)
        {
            string? descripcion = fallbackDescripcion;
            try
            {
                var catalogoDescripcion = await _catalogoRepository.ObtenerDescripcionAsync(codigo);
                if (!string.IsNullOrWhiteSpace(catalogoDescripcion))
                    descripcion = catalogoDescripcion;
            }
            catch { }
            return new ReposicionPinResponseDto { CodigoInt = codigo, Descripcion = descripcion };
        }

        private async Task<ReposicionPinResponseDto> ObtenerRespuestaDinamicaAsync(int codigo, string campo, string detalle, string? fallbackDescripcion = null)
        {
            string? descripcion = fallbackDescripcion;
            try
            {
                var catalogoDescripcion = await _catalogoRepository.ObtenerDescripcionAsync(codigo);
                if (!string.IsNullOrWhiteSpace(catalogoDescripcion))
                    descripcion = catalogoDescripcion;
            }
            catch { }

            string mensajeFinal = $"{descripcion ?? "Error"} {campo}: {detalle}";
            return new ReposicionPinResponseDto { CodigoInt = codigo, Descripcion = mensajeFinal };
        }
        #endregion
    }
}