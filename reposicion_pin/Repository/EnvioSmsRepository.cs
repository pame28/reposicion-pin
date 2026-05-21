using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using reposicion_pin.Infrastructure.Data;
using reposicion_pin.Models;
using reposicion_pin.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using wsBIMovil;

namespace reposicion_pin.Repository
{
    public class EnvioSmsRepository : IEnvioSmsRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<EnvioSmsRepository> _logger;
        private readonly MensajesSoapClient _mensajesSoapClient;
        private readonly string _UsrBiMovil;
        private readonly string _PassBiMovil;
        private readonly string _PlantillaSMS;
        private readonly ParamAmbienteModel? _paramAmbiente;

        public EnvioSmsRepository(AppDbContext dbContext, IConfiguration configuration, ILogger<EnvioSmsRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

            // Obtener configuración de BiMovil desde SP
            var configBiMovil = _dbContext
                .usp_ObtenerConfigBiMovil
                .AsEnumerable()
                .FirstOrDefault();
            if (configBiMovil == null)
                throw new Exception("No se encontró configuración de BiMovil.");

            _UsrBiMovil = configBiMovil.UserBiMovil;
            _PassBiMovil = configBiMovil.PassBiMovil;
            _PlantillaSMS = configBiMovil.PlantillaSMS;

            if (string.IsNullOrWhiteSpace(_PlantillaSMS))
                throw new Exception("La plantilla SMS no está configurada.");

            _paramAmbiente = _dbContext.ParamAmbiente
                .Where(p => p.Estado ==1)
                .OrderBy(p => p.IdRow)
                .FirstOrDefault();

            _mensajesSoapClient = new MensajesSoapClient(MensajesSoapClient.EndpointConfiguration.MensajesSoap);
        }

        public async Task<(int Codigo, string Mensaje, string SmsEnviado)>
       EnviarSmsDirectoAsync(
           string telefono,
           string operador,
       //    string mensajeBase,
           string pin,
           string referencia,
           CancellationToken cancellationToken)
        {
            try
            {
                string telefonoFinal = _paramAmbiente?.Telefono ?? telefono;
                string operadorFinal = _paramAmbiente?.IdOperador ?? operador;             

                // Validaciones básicas
  

                if (string.IsNullOrWhiteSpace(pin))
                {
                    _logger.LogError("PIN vacío. Ref:{Referencia}", referencia);
                    return (99, "PIN inválido", string.Empty);
                }
                // Construcción del mensaje
                string mensajeFinal = _PlantillaSMS.Replace("@PIN", pin);

                _logger.LogInformation("MensajeFinal: {MensajeFinal}", mensajeFinal);

             

                // Buscar PIN dentro del mensaje
                int posicionPin = mensajeFinal.IndexOf(pin, StringComparison.Ordinal);

                if (posicionPin < 0)
                {
                    _logger.LogError("PIN no encontrado dentro del mensaje. Ref:{Referencia}", referencia);
                    return (99, "Error interno al procesar PIN", string.Empty);
                }

                // SOAP trabaja con posiciones 1-based
                int enmascararDesde = posicionPin + 1;
                int enmascararHasta = posicionPin + pin.Length;

              
                string mensajeLog = mensajeFinal.Replace(pin, "****");

                _logger.LogDebug(
                    "BiMovil Request -> Telefono:{Telefono}, Operador:{Operador}, Mensaje:{Mensaje}, Referencia:{Referencia}, EnmascararDesde: {EnmascararDesde}, EnmascararHasta: {EnmascararHasta}",
                    telefonoFinal,
                    operadorFinal,
                    mensajeLog,
                    referencia,
                    enmascararDesde,
                    enmascararHasta
                );

                var response = await _mensajesSoapClient.Envia_MensajeAsync(
                    _UsrBiMovil,
                    _PassBiMovil,
                    telefonoFinal,
                    operadorFinal,
                    mensajeFinal,
                    referencia,
                    enmascararDesde.ToString(),
                    enmascararHasta.ToString()
                  
                );

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(response.Envia_MensajeResult.Any1.InnerXml);



                XmlNode? nodoCod = xmlDoc.GetElementsByTagName("cod_ret")[0];
                XmlNode? nodoMensaje = xmlDoc.GetElementsByTagName("mensaje")[0];

               
                int codigo = nodoCod != null ? int.Parse(nodoCod.InnerText) : 99;
                string mensajeRespuesta = nodoMensaje?.InnerText ?? "Respuesta inválida";

                _logger.LogInformation(
                    "BiMovil Ref:{Referencia} Codigo:{Codigo} Mensaje:{Mensaje}",
                    referencia, codigo, mensajeRespuesta);

                return (codigo, mensajeRespuesta, _PlantillaSMS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando SMS Ref:{Referencia}", referencia);
                return (99, ex.Message, string.Empty);
            }
        }
      

        
    }
}