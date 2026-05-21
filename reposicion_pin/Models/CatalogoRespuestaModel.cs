using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reposicion_pin.Models
{
    [Table ("CatalogoRespuestas")]
    public class CatalogoRespuestaModel
    {
        [Key]
        [Column("Codigo")]
        public int Codigo { get; set; }
        public string Descripcion { get; set; }=string.Empty;
    }
}
