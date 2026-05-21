using reposicion_pin.Infrastructure.Data;
using reposicion_pin.Models;
using reposicion_pin.Repository.Interface;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;


namespace reposicion_pin.Repository
{
    public class EnvioEmailRepository : IEnvioEmailRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<EnvioEmailRepository> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ParamAmbienteModel? _paramAmbiente;

        private readonly string _serverEmail;
        private readonly string _mailFrom;
        private readonly string _mailSubject;
        private readonly string _plantillaHTML;

        public EnvioEmailRepository(
            AppDbContext dbContext,
            ILogger<EnvioEmailRepository> logger,
            IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _logger = logger;
            _env = env;

            var configEmail = _dbContext
                .usp_ObtenerConfigEmail
                .AsEnumerable()
                .FirstOrDefault();

            if (configEmail == null)
                throw new Exception("No se encontró configuración de ConfigEmail.");

            _serverEmail = configEmail.ServerEmail;
            _mailFrom = configEmail.MailFrom;
            _mailSubject = configEmail.MailSubject;
            _plantillaHTML = configEmail.PlantillaHTML;

            _paramAmbiente = _dbContext.ParamAmbiente
                .Where(p => p.Estado == 1)
                .OrderBy(p => p.IdRow)
                .FirstOrDefault();
        }

        public async Task<(int Codigo, string Mensaje, string MailFrom, string MailSubject, string CuerpoCorreo)> EnviarCorreoAsync(
     string correoDestino,
  //   string cuerpoBase,
     string pin,
     string referencia,
     CancellationToken cancellationToken)
        {
            try
            {
                string correoFinal = _paramAmbiente?.Correo ?? correoDestino;

              //  string cuerpoFinal = cuerpoBase.Replace("@PIN", pin);
                string htmlFinal = _plantillaHTML.Replace("@PIN", pin);
                //   string htmlFinal = _plantillaHTML.Replace("@ContenidoEmail", cuerpoFinal);

                using var smtp = new SmtpClient(_serverEmail)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true
                };

                using var mail = new MailMessage
                {
                    From = new MailAddress(_mailFrom),
                    Subject = _mailSubject,
                    Body = htmlFinal,
                    IsBodyHtml = true
                };

                mail.To.Add(correoFinal);

                await smtp.SendMailAsync(mail, cancellationToken);

                _logger.LogInformation(
                    "Correo enviado Ref:{Referencia} Destino:{Destino}",
                    referencia, correoFinal);

                return (0, "Correo enviado correctamente.", _mailFrom, _mailSubject, _plantillaHTML);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error enviando correo Ref:{Referencia} Destino:{Destino}",
                    referencia, correoDestino);

                return (8, ex.Message, _mailFrom, _mailSubject, "--");
            }
        }
    }
}
