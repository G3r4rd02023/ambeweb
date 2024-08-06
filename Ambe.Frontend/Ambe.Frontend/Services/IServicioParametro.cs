namespace Ambe.Frontend.Services
{
    public interface IServicioParametro
    {
        Task<string> ObtenerValor(string nombre);
    }
}
