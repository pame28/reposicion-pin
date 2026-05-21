using System.ComponentModel.DataAnnotations;

namespace reposicion_pin.Models
{
    public class BandejaEnvioSmsModel
    {

        [Key]
        public int IdRow { get; set; }

        public string? NumeroTarjeta { get; set; }

        public string? Sms { get; set; }

        public string? Celular { get; set; }

        public string? Telco { get; set; }

        public string? Referencia { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFinal { get; set; }

        public int? Estado { get; set; }

        public string? DescripcionEstado { get; set; }
    }
}
