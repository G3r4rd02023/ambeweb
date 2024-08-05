using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ambe.Frontend.Services
{
    public interface IServicioLista
    {
        Task<IEnumerable<SelectListItem>> GetListaRoles();

        Task<IEnumerable<SelectListItem>> GetListaMarcas();

        Task<IEnumerable<SelectListItem>> GetListaModelos();

        Task<IEnumerable<SelectListItem>> GetListaConductores();

        IEnumerable<SelectListItem> GetListaEstados();

    }
}
