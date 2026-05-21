namespace reposicion_pin.Repository.Interface
{
    public interface IOperadorRepository
    {
        Task<bool> ExisteOperadorAsync(string operador);
    }
}
