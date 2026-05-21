using System.ComponentModel.DataAnnotations;

namespace reposicion_pin.Models
{
    public class usp_ObtenerConfigBiMovil
    {
        [Key]
        public int IdRows { get; set; }
        public string UserBiMovil { get; set; } = "";
        public string PassBiMovil { get; set; } = "";

        public string PlantillaSMS {  get; set; } = string.Empty;
    }
}
