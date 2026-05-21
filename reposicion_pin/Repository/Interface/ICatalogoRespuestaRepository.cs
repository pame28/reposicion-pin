namespace reposicion_pin.Repository.Interface
{
    public interface ICatalogoRespuestaRepository
    {
        Task<string?> ObtenerDescripcionAsync(int codigo);
    }
}
