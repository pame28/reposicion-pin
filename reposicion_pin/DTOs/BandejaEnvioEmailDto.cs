namespace reposicion_pin.DTOs
{
    public class BandejaEnvioEmailDto
    {
        public required int IdRow { get; set; }
        public required string Correo { get; set; }
        public required string MailFrom { get; set; }
        public required string MailSubject { get; set; }
        public required string Mensaje { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
