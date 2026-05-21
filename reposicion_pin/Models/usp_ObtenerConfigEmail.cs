using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace reposicion_pin.Models
{
    public class usp_ObtenerConfigEmail
    {
        [Key]
        public required int IdRows { get; set; }

        public required string CommandTimeOut { get; set; }
        public required string ServerEmail { get; set; }
        public required string MailFrom { get; set; }    
        public required string MailSubject { get; set; }

        public required string Estado { get; set; }

        public required string PlantillaHTML { get; set; }

    }
}
