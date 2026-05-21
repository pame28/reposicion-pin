namespace reposicion_pin.DTOs
{
    public class AS400CallConfigDto
    {
        public required string CallAs400 { get; set; }
        public string TipoCall { get; set; } = string.Empty;
        public List<string> Librerias { get; set; } = new();
    }
}
