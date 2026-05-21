using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reposicion_pin.Models
{
    [Table("Tarjetas")]
    public class TarjetaModel
    {
        [Key]
        public int Id {  get; set; }
        public string EquivalenteTC { get; set; } = string.Empty;
        public string NumeroTarjeta { get; set; } = string.Empty;
        public string CIF { get; set; }=string.Empty;
        public DateTime FechaFinVigencia { get; set; }
        public short EstadoTarjeta { get; set; }
        public string EstadoHabilitar { get; set; } = string.Empty;
    }
}
