namespace reposicion_pin.DTOs
{
    public class NotificacionDto
    {
        public string NumeroTarjeta { get; set; }
        public string PIN { get; set; }
        public string ContenidoSMS { get; set; }
        public string ContenidoCorreo { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public string Referencia { get; set; }
        public int MetodEnvio { get; set; } // 1 = SMS, 2 = Email
        public string TipoTarjeta { get; set; }
    }
}
