namespace reposicion_pin.Repository.Interface
{
    public interface IConfiguracionAs400Repository
    {
        Task<(string call, List<string> librerias)> ObtenerConfiguracionAsync(int idCall);
    }
}
