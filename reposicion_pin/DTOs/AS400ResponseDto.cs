namespace reposicion_pin.DTOs
{
    public class AS400ResponseDto
    {
        public string? Pin {  get; set; }=string.Empty;
        public string CodigoError { get; set; } = string.Empty;
        public string DescripcionError { get; set; } = string.Empty;

    }
}
