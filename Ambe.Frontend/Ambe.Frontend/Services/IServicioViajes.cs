using Ambe.Frontend.Models.Entidades;

namespace Ambe.Frontend.Services
{
    public interface IServicioViajes
    {
        Task<IEnumerable<Viajes>> GetViajesAsync();

        Task<IEnumerable<BitacoraViaje>> GetBitacoraViajeAsync(int idViaje);

        Task<IEnumerable<RegistroViaje>> GetRegistroViajeAsync(int idViaje);

        Task<IEnumerable<Incidentes>> GetIncidentesAsync(int idViaje);


    }
}
