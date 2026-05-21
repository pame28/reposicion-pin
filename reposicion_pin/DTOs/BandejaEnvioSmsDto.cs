namespace reposicion_pin.DTOs
{
    public class BandejaEnvioSmsDto
    {
        public required int IdRow { get; set; }
        public required string Celular { get; set; }
        public required string Sms { get; set; }
        public required string Telco { get; set; } 
        public DateTime FechaRegistro { get; set; }
    }
}
