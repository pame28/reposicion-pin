namespace reposicion_pin.Repository.Interface
{
    public interface IEnvioEmailRepository
    {
        Task<(int Codigo, string Mensaje, string MailFrom, string MailSubject, string CuerpoCorreo)> EnviarCorreoAsync(
           string correoDestino,
         //  string cuerpoBase,
           string pin,
           string referencia,    
           CancellationToken cancellationToken);
    }
}
