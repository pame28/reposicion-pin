using System.Text.Json.Serialization;

namespace reposicion_pin.DTOs
{
    public class ReposicionPinResponseDto
    {
        [JsonIgnore]
        public int CodigoInt { get; set; }
        public string Codigo => CodigoInt < 10 ? $"0{CodigoInt}" : CodigoInt.ToString();
        public string? Descripcion { get; set; } 

        [JsonIgnore]
        public string? Pin { get; set; }
    }
}
