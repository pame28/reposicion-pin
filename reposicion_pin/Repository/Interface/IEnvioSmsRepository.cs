namespace reposicion_pin.Repository.Interface
{
    public interface IEnvioSmsRepository
    {


        Task<(int Codigo, string Mensaje, string SmsEnviado)>
           EnviarSmsDirectoAsync(
               string telefono,
               string operador,
             //  string mensajeBase,
               string pin,
               string referencia,           
               CancellationToken cancellationToken);

    }
}
