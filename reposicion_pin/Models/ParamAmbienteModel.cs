using System.ComponentModel.DataAnnotations;

namespace reposicion_pin.Models
{
    public class ParamAmbienteModel
    {
        [Key]
        public int IdRow { get; set; }

        public string? Ambiente { get; set; }

        public string? Correo { get; set; }

        public string? Telefono { get; set; }

        public string? IdOperador { get; set; }

        public int? Estado { get; set; }
    }
}
