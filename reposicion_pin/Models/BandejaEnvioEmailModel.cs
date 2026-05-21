using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace reposicion_pin.Models
{
    public class BandejaEnvioEmailModel
    {
        [Key]
        public int IdRow { get; set; }
      
        public required string NumeroTarjeta { get; set; }
        public required string CuerpoCorreo { get; set; }
        public required string MailFrom { get; set; }
        public required string MailSubject { get; set; } 
        public required string Correo {  get; set; }
        public required string Referencia { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinal {  get; set; }
        public int Estado { get; set; }
        public string DescripcionEstado { get; set; } = string.Empty;
    }
}
