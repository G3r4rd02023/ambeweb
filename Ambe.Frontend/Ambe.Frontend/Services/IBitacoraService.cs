using Ambe.Frontend.Models.Entidades;

namespace Ambe.Frontend.Services
{
    public interface IBitacoraService
    {
        Task<Bitacora> AgregarRegistro(BitacoraViewModel model);

        

    }
}
