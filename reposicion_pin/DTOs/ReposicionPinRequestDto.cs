namespace reposicion_pin.DTOs
{
    public class ReposicionPinRequestDto
    {
        public string CIF { get; set; } = string.Empty;
        public string DNI { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public int MetodoEnvio { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string OperadorTelefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string UltimosDigitosTC { get; set; } = string.Empty;
        public string ContenidoSMS { get; set; } = string.Empty;
        public string ContenidoCorreo { get; set; } = string.Empty;
    }
}
