using Ambe.Frontend.Models.Entidades;

namespace Ambe.Frontend.Services
{
    public interface IServicioPersonas
    {
        Task<IEnumerable<Personas>> GetPersonasAsync();

        Task<IEnumerable<TipoPersonas>> GetTiposPersonaAsync();

        Task<IEnumerable<Contactos>> GetContactosAsync(int idPersona);

    }
}
